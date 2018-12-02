using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIManager uiManager;
    public WorldManager worldManager;
    public EnemyManager enemyManager;
    public ParticlesManager particlesManager;
    public CameraMovement cameraMovement;
    public Transform player;

    public Transform powerUpPrefab;

    private const float sacriBarMax = 100f;
    private const float sacriBarDecline = 1f;
    private float currentSacriBarAmount;

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
    }

    private void Update()
    {
        currentSacriBarAmount -= sacriBarDecline * Time.deltaTime;
        uiManager.SetSacriBarFillAmount(currentSacriBarAmount / sacriBarMax);

        if(currentSacriBarAmount <= 0f)
        {
            OnSacriBarEmpty();
        }
    }

    private void OnSacriBarEmpty()
    {
        // spawn boss
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
}
