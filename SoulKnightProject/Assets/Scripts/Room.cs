using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] _possibleObjectsToSpawn;
    [SerializeField] private GameObject[] _possibleEnemiesToSpawn;
    [SerializeField] private int _maxObjects = 5;
    [SerializeField] private int _maxEnemies = 3;
    [SerializeField] private Transform _spawnArea;

    [SerializeField] private RoomType _roomType = RoomType.Normal;

    private void Start()
    {
        InitializeRoom();
    }

    private void InitializeRoom()
    {
        switch (_roomType)
        {
            case RoomType.Normal:
                SpawnObjects();
                SpawnEnemies();
                break;

            case RoomType.Treasure:
                SpawnTreasure();
                break;

            case RoomType.Boss:
                SpawnBoss();
                break;
        }
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < _maxObjects; i++)
        {
            if (_possibleObjectsToSpawn.Length == 0) return;

            GameObject obj = _possibleObjectsToSpawn[Random.Range(0, _possibleObjectsToSpawn.Length)];
            Vector3 spawnPos = GetRandomPositionInArea();
            Instantiate(obj, spawnPos, Quaternion.identity, transform);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _maxEnemies; i++)
        {
            if (_possibleEnemiesToSpawn.Length == 0) return;

            GameObject enemy = _possibleEnemiesToSpawn[Random.Range(0, _possibleEnemiesToSpawn.Length)];
            Vector3 spawnPos = GetRandomPositionInArea();
            Instantiate(enemy, spawnPos, Quaternion.identity, transform);
        }
    }

    private void SpawnTreasure()
    {
        Debug.Log("Spawn coffre");
        // TODO Prefabs de coffre
    }

    // Spawn d'un boss
    private void SpawnBoss()
    {
        Debug.Log("Spawn boss!");
        // TODO Prefabs de boss
    }

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
public enum RoomType
{
    Normal,
    Treasure,
    Boss
}
