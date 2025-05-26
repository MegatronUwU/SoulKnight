using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour
{
	//[SerializeField] private GameObject[] _possibleObjectsToSpawn;
	//[SerializeField] private GameObject[] _possibleEnemiesToSpawn;
	//[SerializeField] private int _maxObjects = 5;
	//[SerializeField] private int _maxEnemies = 3;
	[SerializeField] private Transform _spawnArea;
	[SerializeField] private Transform _enemySpawnPointsParent;
	private List<Transform> _enemySpawnPoints = new();

	public GameObject FloorLinePrefab;

	//private RoomType _roomType = RoomType.Normal;

	private bool _activated = false;
	private int _enemiesCount = 0;
	private RoomConfiguration _configuration;
	private RoomConnector _connector;

	[SerializeField] private Transform _bossSpawnPoint;
	[SerializeField] private GameObject _bossPrefab;
	public GameObject BossPrefab => _bossPrefab;

	private static bool _hasSpawnedBoss = false;

	[SerializeField] private RestartDoor _restartDoorPrefab;
	private RestartDoor _spawnedRestartDoor;

	private void Awake()
	{
		_connector = GetComponent<RoomConnector>();
		_enemySpawnPoints = new(_enemySpawnPointsParent.childCount);

		foreach (Transform child in _enemySpawnPointsParent)
		{
			if (child == _enemySpawnPointsParent)
				continue;

			_enemySpawnPoints.Add(child);
		}
	}

	public void InitializeRoom(RoomConfiguration configuration)
	{
		_configuration = configuration;

		_connector.InitializeDoors(this);

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

	public void StartRoom()
	{
		if (_activated) return;
		_activated = true;

		if (_configuration.MaxEnemiesCount <= 0)
			return;

		_connector.CloseAllDoors(); // active portes + bloque les colliders
		SpawnEnemies(_configuration.MaxEnemiesCount, _configuration.PossibleEnemiesToSpawn);
		//StartCoroutine(CheckEnemiesCoroutine());
	}

	//private IEnumerator CheckEnemiesCoroutine()
	//{
	//    while (_spawnedEnemies.Exists(e => e != null && e.activeInHierarchy))
	//    {
	//        yield return new WaitForSeconds(1f);
	//    }

	//    _connector.OpenAllDoors(); 
	//}

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

	private void SpawnEnemies(int maxEnemiesCount, Enemy[] enemies)
	{
		if (maxEnemiesCount == 0)
			return;

		_enemiesCount = Random.Range(1, maxEnemiesCount);

		while (_enemiesCount > _enemySpawnPoints.Count)
			_enemiesCount--;

		for (int i = 0; i < _enemiesCount; i++)
		{
			if (enemies.Length == 0) return;
			if (_enemySpawnPoints.Count == 0) return;

			Enemy enemy = enemies[Random.Range(0, enemies.Length)];

			Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
			_enemySpawnPoints.Remove(spawnPoint);

			Enemy instance = Instantiate(enemy, spawnPoint.position, Quaternion.identity, transform);
			instance.Health.OnDeath.AddListener(OnEnemyDeath);
		}
	}

	private void OnEnemyDeath()
	{
		_enemiesCount--;

		if (_enemiesCount > 0)
			return;

		_connector.OpenAllDoors();

		if (_spawnedRestartDoor != null)
			_spawnedRestartDoor.ActivateDoor();
	}

	public void SetBoss(Enemy boss, Vector3 restartDoorPosition)
	{
		_enemiesCount = 1;
		boss.Health.OnDeath.AddListener(OnEnemyDeath);

		if (_restartDoorPrefab != null && _spawnedRestartDoor == null)
		{
			_spawnedRestartDoor = Instantiate(_restartDoorPrefab, restartDoorPosition, Quaternion.identity, transform);
			_spawnedRestartDoor.gameObject.SetActive(false);
		}
	}

	//private void SpawnTreasure()
	//{
	//    Debug.Log("Spawn coffre");
	//    // TODO Prefabs de coffre
	//}

	//private void SpawnBoss()
	//{
	//    if (_bossPrefab == null || _bossSpawnPoint == null)
	//    {
	//        Debug.LogWarning("BossPrefab manquant");
	//        return;
	//    }

	//    Enemy boss = Instantiate(_bossPrefab, _bossSpawnPoint.position, Quaternion.identity, transform).GetComponent<Enemy>();

	//    if (boss == null)
	//    {
	//        Debug.LogError("Script Enemy manquant");
	//        return;
	//    }

	//    _enemiesCount = 1; 
	//    boss.Health.OnDeath.AddListener(OnEnemyDeath);
	//}

	// On r�cup�re une position al�atoire dans la zone de spawn
	private Vector3 GetRandomPositionInArea()
	{
		if (_spawnArea == null)
			_spawnArea = this.transform;

		Vector3 randomPoint = new Vector3(
			Random.Range(-_spawnArea.localScale.x / 2, _spawnArea.localScale.x / 2),
			.5f,
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
