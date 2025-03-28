using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    [SerializeField] private ARSession _arSession;

    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Enemy Setting")] [SerializeField]
    private int enemyCount = 2;

    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float deSpawnrate = 4f;

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int _score = 0;

    private bool _gameStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.OnUIStartButtonPressed += StartGame;
    }


    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;

        _planeManager.enabled = false;
        foreach (var plane in _planeManager.trackables)
        {
            var meshVisualize = plane.GetComponent<ARPlaneManager>();
            if (meshVisualize) meshVisualize.enabled = false;

            var lineVisualize = plane.GetComponent<LineRenderer>();
            if (lineVisualize) lineVisualize.enabled = false;
            
            
            
            
        }

        StartCoroutine(SpawnEnemies());
    }

    void RestartGame()
    {
        _gameStarted = false;
        _planeManager.enabled = true;
        _arSession.Reset();
        _score = 0;
        uiManager.UpdateScore(_score);
    }

    void SpawnEnemy()
    {
       if (_planeManager.trackables.count == 0) return;
       List<ARPlane> planes = new List<ARPlane>();
       foreach (var plane in _planeManager.trackables)
       {
           planes.Add(plane);
       }

       var randomPlane = planes[Random.Range(0, planes.Count)];
       var randomPlanePosition = GetRandomPosition(randomPlane);
       var enemy = Instantiate(enemyPrefab, randomPlanePosition, Quaternion.identity);
       _spawnedEnemies.Add(enemy);

       var enemyScript = enemy.GetComponentInChildren < EnemyScript >();
       if (enemyScript != null)
       {
           enemyScript.OnEnemyDestroyed += AddScore;
       }

       StartCoroutine(DespawnEnemies(enemy));


       Vector3 GetRandomPosition(ARPlane plane)
       {
           var center = plane.center;
           var size = plane.size * 0.5f;
           var randomX = Random.Range(-size.x, size.x);
           var randomZ = Random.Range(-size.y, size.y);

           return new Vector3(center.x + randomX, center.y,center.z + randomZ);
       }
    }

    IEnumerator SpawnEnemies()
    {
        while (_gameStarted)
        {
            if (_spawnedEnemies.Count < enemyCount)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnRate);
        } 
    }

    IEnumerator DespawnEnemies(GameObject enemy)
    {
        yield return new WaitForSeconds(deSpawnrate);
        if (_spawnedEnemies.Contains(enemy))
        {
            _spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
        
        
    }

    private void AddScore()
    {
        _score++;
        uiManager.UpdateScore(_score);
    }
}
