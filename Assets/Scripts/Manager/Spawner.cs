using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    [Header("Game objects")]
    [SerializeField] private GameObject[] Enemes;
    [SerializeField] private Transform[] SpawnPoints;

    [Header("Spawn control")]
    [SerializeField] private int[] MaxSpawn;
    private List<GameObject> CurrentOnField;

    [SerializeField] private float TimeBetweenSpawn = 3;
    private float TimeSinceLastSpawn = 0;

    [SerializeField] private GameObject waveBox;

    private int SpawnCount = 0;
    private int CurrentWave = 0;
    private bool CanSpawn = false;
    private bool StartSpawnWave = false;

    private int CurrentMobs;
    private bool isBossWave = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        CurrentMobs = 0;
        CurrentOnField = new List<GameObject>();
        GameEvents.EnemyDestroyed += Remove;
        if (SaveLoad.SaveExits("GameTime"))
        {
            if (SaveLoad.Load<GameSave>("GameTime").lastScene == SceneManager.GetActiveScene().buildIndex)
                gameObject.SetActive(false);
        }
        isBossWave = (gameObject.name.CompareTo("BossStage") == 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!StartSpawnWave)
            return;
        if (!CanSpawn)
            return;
        Transform SpawnPoint = GetRandomSpawnPoint();
        if (SpawnCount > 0 && (TimeSinceLastSpawn += Time.fixedDeltaTime) >= TimeBetweenSpawn)
        {
            var target = Instantiate(Enemes[CurrentMobs], SpawnPoint.position, Quaternion.identity);
            CurrentOnField.Add(target);
            ChangeMob();
            SpawnCount--;
            TimeSinceLastSpawn = 0;
            return;
        }
        if (CurrentWave > MaxSpawn.Length)
        {
            CanSpawn = false;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        boxCollider2D.enabled = false; // turn off collision check
        StartSpawnWave = true;
        CanSpawn = true;
        TimeSinceLastSpawn = TimeBetweenSpawn;
        NextWave();
        if (waveBox != null)
            waveBox.SetActive(true);
    }

    private Transform GetRandomSpawnPoint()
    {
        var randomSpawn = new System.Random();
        int maxSpawnPoint = SpawnPoints.Length;
        return SpawnPoints[randomSpawn.Next(maxSpawnPoint)];
    }

    private void ChangeMob()
    {
        CurrentMobs++;
        if (CurrentMobs >= Enemes.Length)
        {
            CurrentMobs = 0;
        }
    }

    private void SetSpawn()
    {
        if (CanSpawn)
        {
            CanSpawn = false;
            return;
        }
        CanSpawn = true;
        return;
    }

    private bool IsEndWave()
    {
        try
        {
            foreach (GameObject target in CurrentOnField)
            {
                if (target == null)
                {
                    CurrentOnField.Remove(target);
                }
            }
        }
        catch { }

        if (CurrentOnField.Count <= 0)
        {
            return true;
        }
        return false;
    }

    private void NextWave()
    {
        if (CurrentWave == MaxSpawn.Length)
        {
            gameObject.SetActive(false);
            GameEvents.OnWaveEndUpgrade();
            return;
        }
        SpawnCount = MaxSpawn[CurrentWave++];
    }

    private void Remove(int id)
    {
        try
        {
            foreach (GameObject target in CurrentOnField)
            {
                if (target.GetComponent<EnemyHealth>().GetInstanceID() == id)
                {
                    Destroy(target);
                    CurrentOnField.Remove(target);
                }
            }
        }
        catch { }
        if (IsEndWave() && SpawnCount == 0)
        {
            NextWave();
        }
    }

    private void OnDestroy()
    {
        GameEvents.EnemyDestroyed -= Remove;
    }
}
