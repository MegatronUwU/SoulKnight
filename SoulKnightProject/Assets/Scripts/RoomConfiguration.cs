using UnityEngine;

[CreateAssetMenu(fileName = "New RoomConfiguration", menuName = "Scriptable Objects/RoomConfiguration")]
public class RoomConfiguration : ScriptableObject
{
	public int MaxObjectsCount = 5;
	public GameObject[] PossibleObjectsToSpawn;

	public int MaxEnemiesCount = 3;
	public Enemy[] PossibleEnemiesToSpawn;

	public bool IsBossRoom => PossibleEnemiesToSpawn.Length == 0 && BossPrefab != null;
	public GameObject BossPrefab;
}
