﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState = GameState.Intro;

    public UIManager uiManager;
    public WorldManager worldManager;
    public EnemyManager enemyManager;
    public PostProcessingManager postProcessingManager;
    public ParticlesManager particlesManager;
    public AudioManager audioManager;
    public CameraMovement cameraMovement;
    public Transform player;
    public Transform bossCollidersPrefab;
    public Transform bossPrefab;
    private Transform boss;

    public Transform powerUpPrefab;

    private const float sacriBarMax = 100f;
    private const float sacriBarDecline = 1f;
    private float currentSacriBarAmount;

    private const int bossBaseHealth = 100;
    private const int bossHealthGainPerPowerup = 50;
    private int bossMaxHealth;
    private int bossCurrentHealth;

    private readonly Vector3 bossOffset = new Vector3(0f, 4.67f, 0f);
    private readonly Vector3 bossCameraOffset = new Vector3(0f, 3f, 0f);

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Init();
        }
    }

    private void Init()
    {
        currentSacriBarAmount = sacriBarMax / 2f;
        worldManager.CreateWorld();
        enemyManager.CreateEnemies();
        SetPlayerActive(false);

        StartCoroutine(uiManager.PlayIntro());
    }

    public void OnIntroEnded()
    {
        gameState = GameState.Level;
        uiManager.DisplayTutorial();
        SetPlayerActive(true);
    }

    private void Update()
    {
        if(gameState == GameState.Level)
        {
            currentSacriBarAmount -= sacriBarDecline * Time.deltaTime;
            uiManager.SetSacriBarFillAmount(currentSacriBarAmount / sacriBarMax);

            if (currentSacriBarAmount <= 0f)
            {
                OnSacriBarEmpty();
            }
        }

        WantedCheat();
        // TODO: Remove before final build!!!
        Cheats();
    }

    public void OnBossTakeDamage(int amount)
    {
        bossCurrentHealth -= amount;

        uiManager.SetSacriBarFillAmount(1f - ((float) bossCurrentHealth / bossMaxHealth));

    }

    private void OnSacriBarEmpty()
    {
        gameState = GameState.Boss;
        bossMaxHealth = bossBaseHealth + bossHealthGainPerPowerup * player.GetComponent<PlayerPowerUps>().powerUpsCollected;
        bossCurrentHealth = bossMaxHealth;
        uiManager.SetSacriBarFillAmount(0f);
        StartCoroutine(BossAppearSequence());
    }

    private IEnumerator BossAppearSequence()
    {
        Vector3 playerPosition = player.position;
        Instantiate(bossCollidersPrefab, playerPosition + bossCameraOffset, Quaternion.identity);

        SetPlayerActive(false);

        cameraMovement.isFollowing = false;
        cameraMovement.MoveToTargetPosition(playerPosition + bossCameraOffset);

        yield return new WaitForSeconds(1f);

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            player.GetComponent<PlayerPowerUps>().OnPowerUpPickedUp();
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

            PlayerPowerUps powerUps = player.GetComponent<PlayerPowerUps>();

            for(int i = 0; i < EnemyManager.amountOfEnemyGroups / 2; i++)
            {
                powerUps.OnPowerUpPickedUp();
            }
        }
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