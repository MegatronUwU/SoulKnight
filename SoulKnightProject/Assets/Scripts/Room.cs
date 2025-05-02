using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    //[SerializeField] private GameObject[] _possibleObjectsToSpawn;
    //[SerializeField] private GameObject[] _possibleEnemiesToSpawn;
    //[SerializeField] private int _maxObjects = 5;
    //[SerializeField] private int _maxEnemies = 3;
    [SerializeField] private Transform _spawnArea;

    //private RoomType _roomType = RoomType.Normal;

    private bool _activated = false;
    private List<GameObject> _spawnedEnemies = new();
    private RoomConfiguration _configuration;
    private RoomConnector _connector;

    private void Awake()
    {
        _connector = GetComponent<RoomConnector>();
    }

    public void InitializeRoom(RoomConfiguration configuration)
    {
        _configuration = configuration;
        SpawnObjects(configuration.MaxObjectsCount, configuration.PossibleObjectsToSpawn);

        //switch (_roomType)
        //{
        //    case RoomType.Normal:
        //        SpawnObjects();
        //        SpawnEnemies();
        //        break;

        //    case RoomType.Treasure:
        //        SpawnTreasure();
        //        break;

        //    case RoomType.Boss:
        //        SpawnBoss();
        //        break;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activated || !other.CompareTag("Player")) return;

        _activated = true;
        _connector.CloseAllDoors(); // active portes + bloque les colliders
        SpawnEnemies(_configuration.MaxEnemiesCount, _configuration.PossibleEnemiesToSpawn);
        StartCoroutine(CheckEnemiesCoroutine());
    }

    private IEnumerator CheckEnemiesCoroutine()
    {
        while (_spawnedEnemies.Exists(e => e != null))
        {
            yield return new WaitForSeconds(1f);
        }

        _connector.OpenAllDoors(); 
    }

    private void SpawnObjects(int maxObjectsCount, GameObject[] objects)
    {
        if (maxObjectsCount == 0)
            return;

        int objectsCount = Random.Range(0, maxObjectsCount);

        for (int i = 0; i < objectsCount; i++)
        {
            if (objects.Length == 0) return;

            GameObject obj = objects[Random.Range(0, objects.Length)];
            Vector3 spawnPos = GetRandomPositionInArea();
            Instantiate(obj, spawnPos, Quaternion.identity, transform);
        }
    }

    private void SpawnEnemies(int maxEnemiesCount, GameObject[] enemies)
    {
        if (maxEnemiesCount == 0)
            return;

        int enemiesCount = Random.Range(0, maxEnemiesCount);

        for (int i = 0; i < enemiesCount; i++)
        {
            if (enemies.Length == 0) return;

            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            Vector3 spawnPos = GetRandomPositionInArea();
            GameObject instance = Instantiate(enemy, spawnPos, Quaternion.identity, transform);
            _spawnedEnemies.Add(instance);
        }
    }

    //private void SpawnTreasure()
    //{
    //    Debug.Log("Spawn coffre");
    //    // TODO Prefabs de coffre
    //}

    //private void SpawnBoss()
    //{
    //    Debug.Log("Spawn boss");
    //    // TODO Prefabs de boss
    //}

    // On récupère une position aléatoire dans la zone de spawn
    private Vector3 GetRandomPositionInArea()
    {
        if (_spawnArea == null)
            _spawnArea = this.transform;

        Vector3 randomPoint = new Vector3(
            Random.Range(-_spawnArea.localScale.x / 2, _spawnArea.localScale.x / 2),
            0f,
            Random.Range(-_spawnArea.localScale.z / 2, _spawnArea.localScale.z / 2)
        );

        return _spawnArea.position + randomPoint;
    }
}

// Les Type de room
//public enum RoomType
//{
//    Normal,
//    Treasure,
//    Boss
//}

/*
public static class RoomFactory
{
    public static RoomConfig CreateRoom()
    {
        return new RoomConfig();
    }

    public static RoomConfig SetMaxTreasureCount(this RoomConfig config, int count)
    {
        config.maxTreasureCount = count;
        return config;
    }

    public static RoomConfig SetMaxEnemyCount(this RoomConfig config, int count)
    {
        config.maxEnemyCount = count;
        return config;
    }
}

RoomConfig config = RoomFactory.CreateRoom()
    .SetMaxEnemyCount(12)
    .SetMaxTreasureCount(8);
*/
