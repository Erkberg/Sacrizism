﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool introPlayed = false;
    public static bool restartFromBoss = false;

    public GameState gameState = GameState.Intro;

    public UIManager uiManager;
    public WorldManager worldManager;
    public EnemyManager enemyManager;
    public PostProcessingManager postProcessingManager;
    public ParticlesManager particlesManager;
    public AudioManager audioManager;
    public CameraMovement cameraMovement;
    public Transform player;
    public PlayerPowerUps playerPowerUps;
    public Transform bossCollidersPrefab;
    public Transform bossPrefab;
    private Transform boss;

    public Transform powerUpPrefab;

    private const float sacriBarMax = 100f;
    private const float sacriBarDecline = 0.666f;
    private float currentSacriBarAmount;

    private const int bossBaseHealth = 100;
    private const int bossHealthGainPerPowerup = 40;
    private float bossMaxHealth;
    private float bossCurrentHealth;
    private bool truePacifist = true;

    private readonly Vector3 bossOffset = new Vector3(0f, 4.67f, 0f);
    private readonly Vector3 bossCameraOffset = new Vector3(0f, 3f, 0f);

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Init();
    }

    private void Init()
    {
        if(restartFromBoss)
        {            
            StartCoroutine(playerPowerUps.RestorePowerUps());
            uiManager.blackBackground.SetActive(false);
            currentSacriBarAmount = 0f;
            worldManager.CreateWorld();
            enemyManager.CreateEnemies();

            gameState = GameState.Level;
            audioManager.PlayMusic();
            audioManager.PlayAtmo();
        }
        else
        {
            currentSacriBarAmount = sacriBarMax / 2f;
            worldManager.CreateWorld();
            enemyManager.CreateEnemies();
            SetPlayerActive(false);

            if(introPlayed)
            {
                OnIntroEnded();
            }
            else
            {
                introPlayed = true;
                StartCoroutine(uiManager.PlayIntro());
            }            
        }
    }

    public void OnIntroEnded()
    {
        gameState = GameState.Level;        
        SetPlayerActive(true);
        audioManager.PlayMusic();
        audioManager.PlayAtmo();

        if(introPlayed)
        {
            uiManager.DisplayTutorial(4f);
        }
        else
        {
            uiManager.DisplayTutorial(8f);
        }
    }

    private void Update()
    {
        if(gameState == GameState.Level)
        {
            currentSacriBarAmount -= sacriBarDecline * (1f + playerPowerUps.powerUpsCollected * 0.055f) * Time.deltaTime;
            uiManager.SetSacriBarFillAmount(currentSacriBarAmount / sacriBarMax);

            if (currentSacriBarAmount <= 0f)
            {
                OnSacriBarEmpty();
            }

            if(currentSacriBarAmount >= sacriBarMax)
            {
                OnSacriBarFull();
            }
        }

        WantedCheat();
        // TODO: Remove before final build!!!
        Cheats();
    }

    public void OnBossTakeDamage(float amount)
    {
        truePacifist = false;
        bossCurrentHealth -= amount;

        uiManager.SetSacriBarFillAmount(1f - (bossCurrentHealth / bossMaxHealth));

        if(bossCurrentHealth <= 0)
        {
            StartCoroutine(BossDeathSequence());
        }
    }

    private IEnumerator BossDeathSequence()
    {
        SetPlayerActive(false);
        gameState = GameState.Sequence;
        yield return StartCoroutine(boss.GetComponent<BossEnemy>().Die());
        if(enemyManager.enemiesPeaceful)
        {
            StartCoroutine(uiManager.PlayOutroPacifist());
        }
        else
        {
            StartCoroutine(uiManager.PlayOutroRegular());
        }
    }

    private void OnSacriBarEmpty()
    {
        gameState = GameState.Boss;        
        uiManager.SetSacriBarFillAmount(0f);
        StartCoroutine(BossAppearSequence());
    }

    private void OnSacriBarFull()
    {
        gameState = GameState.Sequence;
        StartCoroutine(uiManager.PlayOutroSacribarFull());
    }

    private IEnumerator BossAppearSequence()
    {
        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Unanger();
        }

        foreach(Bullet bullet in FindObjectsOfType<Bullet>())
        {
            Destroy(bullet.gameObject);
        }

        Vector3 playerPosition = player.position;
        Instantiate(bossCollidersPrefab, playerPosition + bossCameraOffset, Quaternion.identity);

        SetPlayerActive(false);

        if(restartFromBoss)
        {
            restartFromBoss = false;
        }
        else
        {
            if (enemyManager.enemiesPeaceful)
            {
                yield return StartCoroutine(uiManager.PlayBossIntroPacifist());
            }
            else
            {
                yield return StartCoroutine(uiManager.PlayBossIntro());
            }            
        }
        uiManager.SetSacriBarFillAmount(0f);
        cameraMovement.isFollowing = false;
        cameraMovement.MoveToTargetPosition(playerPosition + bossCameraOffset);

        yield return new WaitForSeconds(1f);
        bossMaxHealth = bossBaseHealth + bossHealthGainPerPowerup * playerPowerUps.powerUpsForBossHealthCollected;
        bossCurrentHealth = bossMaxHealth;

        audioManager.PlayBossArrivalSound();
        boss = Instantiate(bossPrefab, playerPosition + bossOffset * 3f, Quaternion.identity);

        while(boss.position.y > playerPosition.y + bossOffset.y)
        {
            boss.transform.position -= new Vector3(0f, 10f, 0f) * Time.deltaTime;
            yield return null;
        }

        boss.transform.position = playerPosition + bossOffset;

        uiManager.OnBossStarted();

        worldManager.DestroyAllRocksAndTrees();
        enemyManager.DestroyAllEnemies();

        cameraMovement.Shake(new Vector2(0f, 2f), 1f, 4f);
        audioManager.PlayWamsSound();

        yield return new WaitForSeconds(1f);

        SetPlayerActive(true);

        boss.GetComponent<BossEnemy>().bossEyes.StartLoop();
    }

    private void SetPlayerActive(bool active)
    {
        player.GetComponent<PlayerMovement>().EnableMovement(active);
        player.GetComponent<PlayerCombat>().shootingEnabled = active;
    }

    public void OnDeath()
    {
        SetPlayerActive(false);

        if(gameState == GameState.Level)
        {
            if(enemyManager.enemiesPeaceful)
            {
                StartCoroutine(uiManager.PlayOutroSelfSacrifice());
            }
            else
            {
                StartCoroutine(uiManager.PlayRegularDeath());
            }            
        }

        if (gameState == GameState.Boss)
        {
            StartCoroutine(uiManager.PlayBossDeath());
        }

        gameState = GameState.Sequence;
    }

    public void OnEnemyKilled(int level)
    {
        currentSacriBarAmount += level + 1;
        uiManager.SetSacriBarFillAmount(currentSacriBarAmount / sacriBarMax);
    }

    public float GetSmallRandomizer()
    {
        return Random.Range(0.9f, 1.1f);
    }

    public void SpawnPowerUp(Vector3 position)
    {
        Instantiate(powerUpPrefab, position, Quaternion.identity);
    }

    private void WantedCheat()
    {
        if(Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.G))
        {
            uiManager.tutorial.SetActive(false);
            playerPowerUps.OnPowerUpPickedUp();
        }
    }

    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentSacriBarAmount = 1f;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentSacriBarAmount = 1f;
            uiManager.tutorial.SetActive(false);

            for(int i = 0; i < EnemyManager.amountOfEnemyGroups / 2; i++)
            {
                playerPowerUps.OnPowerUpPickedUp();
            }
        }
    }

    public void RestartGame(bool restartAtBoss = false)
    {
        if(restartAtBoss)
        {
            restartFromBoss = true;
            playerPowerUps.SavePowerUps();
        }
        else
        {
            restartFromBoss = false;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum GameState
{
    Intro,
    Level,
    Boss,
    Outro,
    Sequence
}