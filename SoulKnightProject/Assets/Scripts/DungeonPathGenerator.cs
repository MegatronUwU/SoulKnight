using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonPathGenerator : MonoBehaviour
{
	[SerializeField] private Room _roomPrefab;
	[SerializeField] private int _pathLength = 10;
	[SerializeField] private float _roomSpacing = 12f;
	[SerializeField] private Transform _dungeonParent;

	[SerializeField] private GameObject _corridorPrefab;

	private readonly List<RoomData> _roomDatas = new();

	[SerializeField] private RoomConfiguration _safeRoomConfiguration = null;
	[SerializeField] private RoomConfiguration _normalRoomConfiguration = null;
	[SerializeField] private RoomConfiguration _treasureRoomConfiguration = null;
	[SerializeField] private RoomConfiguration _bossRoomConfiguration = null;

	[SerializeField] private GameObject _floorLinePrefab;
	[SerializeField] private GameObject _groundTilePrefab = null;



	private void Start()
	{
		GeneratePath();
		FindRoomsNeighbours();
		SpawnRooms();
		TryMergeBigRoom();
		GenerateCorridors();
		PopulateRooms();
	}

	private void GeneratePath()
	{
		Vector3 currentPosition = Vector3.zero;
		RoomData startRoom = new RoomData(currentPosition);
		startRoom.Configuration = _safeRoomConfiguration;

		_roomDatas.Add(startRoom);

		for (int i = 1; i < _pathLength; i++)
		{
			Vector3 direction = GetRandomCardinalDirection();
			Vector3 nextPosition = currentPosition + direction * _roomSpacing;

			if (_roomDatas.Exists(r => r.Position == nextPosition))
				continue;

			RoomData newRoom = new RoomData(nextPosition);

			if (i == _pathLength - 1)
				newRoom.Configuration = _bossRoomConfiguration;
			else if (i == _pathLength - 2)
				newRoom.Configuration = _treasureRoomConfiguration;
			else
				newRoom.Configuration = _normalRoomConfiguration;

			_roomDatas.Add(newRoom);
			currentPosition = nextPosition;
		}
	}

	private void SpawnRooms()
	{
		foreach (RoomData data in _roomDatas)
		{
			data.InstantiatedRoom = Instantiate(_roomPrefab, data.Position, Quaternion.identity, _dungeonParent);
		}
	}

	private void GenerateCorridors()
	{
		HashSet<(RoomData, RoomData)> corridorsCreated = new HashSet<(RoomData, RoomData)>();

		foreach (RoomData roomData in _roomDatas)
		{
			RoomConnector connector = roomData.InstantiatedRoom.GetComponent<RoomConnector>();

			foreach (KeyValuePair<Direction, RoomData> neighbour in roomData.Neighbours)
			{
				Transform doorWaypoint = connector.GetDoorWaypointTransform(neighbour.Key);
				connector.OpenDoor(neighbour.Key);

				RoomConnector neightboorRoomConnector = neighbour.Value.InstantiatedRoom.GetComponent<RoomConnector>();
				Direction oppositeDirection = GetOppositeDirection(neighbour.Key);
				Transform neighbourDoorWaypoint = neightboorRoomConnector.GetDoorWaypointTransform(oppositeDirection);

				if (corridorsCreated.Contains((roomData, neighbour.Value)) || corridorsCreated.Contains((neighbour.Value, roomData)))
					continue;

				if (roomData.IsMergedToBigRoom && neighbour.Value.IsMergedToBigRoom)
				{
					connector.RemoveWallsAndDoors(neighbour.Key);

					continue;
				}

				SpawnCorridor(doorWaypoint.position, neighbourDoorWaypoint.position);
				corridorsCreated.Add((roomData, neighbour.Value));
			}
		}
	}

	private void PopulateRooms()
	{
		foreach (RoomData data in _roomDatas)
		{
			//data.InstantiatedRoom.InitializeRoom(_normalRoomConfiguration);
			data.InstantiatedRoom.InitializeRoom(data.Configuration);
		}
	}

	private Direction GetOppositeDirection(Direction direction)
	{
		return direction switch
		{
			Direction.Up => Direction.Down,
			Direction.Down => Direction.Up,
			Direction.Left => Direction.Right,
			Direction.Right => Direction.Left,
			_ => Direction.Down,
		};

		//return (Direction)((int)(direction + 2) % 4);
	}

	// On cherche les connexions pour chaque pièce
	private void FindRoomsNeighbours()
	{
		foreach (RoomData roomData in _roomDatas)
		{
			RoomData neighbour = _roomDatas.FirstOrDefault(rd => rd.Position == roomData.Position + Vector3.left * _roomSpacing);
			if (neighbour != null)
				roomData.AddNeighbourPosition(Direction.Left, neighbour);

			neighbour = _roomDatas.FirstOrDefault(rd => rd.Position == roomData.Position + Vector3.right * _roomSpacing);
			if (neighbour != null)
				roomData.AddNeighbourPosition(Direction.Right, neighbour);

			neighbour = _roomDatas.FirstOrDefault(rd => rd.Position == roomData.Position + Vector3.forward * _roomSpacing);
			if (neighbour != null)
				roomData.AddNeighbourPosition(Direction.Up, neighbour);

			neighbour = _roomDatas.FirstOrDefault(rd => rd.Position == roomData.Position + Vector3.back * _roomSpacing);
			if (neighbour != null)
				roomData.AddNeighbourPosition(Direction.Down, neighbour);
		}
	}

	// On spawn un couloir entre deux positions
	private void SpawnCorridor(Vector3 from, Vector3 to)
	{
		Vector3 direction = to - from;
		Vector3 position = (from + to) / 2f;
		Quaternion rotation = Quaternion.LookRotation(direction);

		GameObject corridor = Instantiate(_corridorPrefab, position, rotation, _dungeonParent);

		//Debug.Log($"Spawned corridor at position: {position}", corridor);
	}

	private Vector3 GetRandomCardinalDirection()
	{
		Vector3[] directions = new[]
		{
			Vector3.forward,
			Vector3.right,
			Vector3.back,
			Vector3.left
		};

		return directions[Random.Range(0, directions.Length)];
	}

	// On convertit la direction en vector 3
	private Vector3 DirectionToVector3(Direction direction)
	{
		return direction switch
		{
			Direction.Up => Vector3.forward,
			Direction.Down => Vector3.back,
			Direction.Left => Vector3.left,
			Direction.Right => Vector3.right,
			_ => Vector3.zero
		};
	}

	private void TryMergeBigRoom()
	{
		foreach (RoomData current in _roomDatas)
		{
			if (current.IsMergedToBigRoom)
				continue;

			if (!current.Neighbours.TryGetValue(Direction.Right, out RoomData right)) continue;
			if (!current.Neighbours.TryGetValue(Direction.Up, out RoomData up)) continue;

			if (!up.Neighbours.TryGetValue(Direction.Right, out RoomData upRight)) continue;

			if (right.IsMergedToBigRoom || up.IsMergedToBigRoom || upRight.IsMergedToBigRoom)
				continue;

			Vector3 roomPosition = new((current.InstantiatedRoom.transform.position.x + upRight.InstantiatedRoom.transform.position.x) / 2,
				(current.InstantiatedRoom.transform.position.y + upRight.InstantiatedRoom.transform.position.y) / 2,
				(current.InstantiatedRoom.transform.position.z + upRight.InstantiatedRoom.transform.position.z) / 2);

			MergeRoomsToBigRoom(roomPosition, current, right, up, upRight);
			break;
		}
	}

	private void MergeRoomsToBigRoom(Vector3 roomPosition, params RoomData[] roomDatas)
	{
		foreach (RoomData roomData in roomDatas)
		{
			roomData.Configuration = _bossRoomConfiguration;
			roomData.IsMergedToBigRoom = true;
		}

		RoomData current = roomDatas[0];
		RoomData right = roomDatas[1];
		RoomData up = roomDatas[2];
		RoomData upRight = roomDatas[3];

		Room bossRoom = current.InstantiatedRoom;
		if (bossRoom != null && bossRoom.BossPrefab != null)
		{
			Vector3 bossSpawnPosition = roomPosition + Vector3.up * 0.5f;
			GameObject bossInstance = Instantiate(bossRoom.BossPrefab, bossSpawnPosition, Quaternion.identity, bossRoom.transform);

			if (bossInstance.TryGetComponent(out Enemy enemyBoss))
			{
				Vector3 doorSpawnPosition = roomPosition + Vector3.back * 2f;
				bossRoom.SetBoss(enemyBoss, doorSpawnPosition);
			}
		}

		RemoveWallsBetween(current, Direction.Right, right);
		RemoveWallsBetween(current, Direction.Up, up);
		RemoveWallsBetween(up, Direction.Right, upRight);
		RemoveWallsBetween(right, Direction.Up, upRight);

		InstantiateFloorLineBetween(current, right, roomPosition);
		InstantiateFloorLineBetween(current, up, roomPosition);
		InstantiateFloorLineBetween(up, upRight, roomPosition);
		InstantiateFloorLineBetween(right, upRight, roomPosition);

		Instantiate(_groundTilePrefab, roomPosition, Quaternion.identity, _dungeonParent);
	}

	private void RemoveWallsBetween(RoomData a, Direction dirToB, RoomData b)
	{
		RoomConnector connectorA = a.InstantiatedRoom.GetComponent<RoomConnector>();

		if (connectorA != null)
			connectorA.DestroyWall(dirToB);

		RoomConnector connectorB = b.InstantiatedRoom.GetComponent<RoomConnector>();

		if (connectorB != null)
			connectorB.DestroyWall(GetOppositeDirection(dirToB));
	}

	private void InstantiateFloorLineBetween(RoomData a, RoomData b, Vector3 roomPosition)
	{
		if (_floorLinePrefab == null)
			return;

		Vector3 pos = (a.Position + b.Position) / 2f;
		Vector3 dir = b.Position - a.Position;

		Quaternion rotation;

		if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
		{
			if(a.Position.z < roomPosition.z)
				rotation = Quaternion.Euler(0, 90f, 0);
			else
				rotation = Quaternion.Euler(0, -90f, 0);
		}
		else
		{
			if(a.Position.x > roomPosition.x)
				rotation = Quaternion.identity;
			else
				rotation = Quaternion.Euler(0, 180f, 0);
		}

		Instantiate(_floorLinePrefab, pos, rotation, _dungeonParent);
	}

	private class RoomData
	{
		public Vector3 Position;
		public List<Direction> Directions = new();
		public Dictionary<Direction, RoomData> Neighbours = new();
		public Room InstantiatedRoom = null;
		public RoomConfiguration Configuration;
		public bool IsMergedToBigRoom = false;

		public RoomData(Vector3 pos)
		{
			Position = pos;
		}

		public void AddNeighbourPosition(Direction direction)
		{
			if (!Directions.Contains(direction))
				Directions.Add(direction);
		}

		public void AddNeighbourPosition(Direction direction, RoomData neighbour)
		{
			if (!Neighbours.TryAdd(direction, neighbour))
			{
				Debug.LogError("Duplicate neighbour");
			}
		}
	}
}
