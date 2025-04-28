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

    private void Start()
    {
        GeneratePath();
        FindRoomsNeighbours(); 
        SpawnRooms();
    }

    private void GeneratePath()
    {
        Vector3 currentPosition = Vector3.zero;
        _roomDatas.Add(new RoomData(currentPosition));

        for (int i = 1; i < _pathLength; i++)
        {
            Vector3 direction = GetRandomCardinalDirection();
            Vector3 nextPosition = currentPosition + direction * _roomSpacing;

            if (_roomDatas.Exists(r => r.Position == nextPosition))
                continue;

            _roomDatas.Add(new RoomData(nextPosition));
            currentPosition = nextPosition;
        }
    }

    /*
     Exemple de vérification des positions :

     Vector3(1,0,8)

     Vector(0,0,8) => true
     Vector(2,0,8) => false

     Vector3(1,0,7) => false
     Vector3(1,0,9) => true

     Direction.Left
     Direction.Up
     */

    // On instancie les salles puis on enlève la porte d’entrée pour chaque pièce 
    private void SpawnRooms()
    {
        foreach (RoomData data in _roomDatas)
        {
            data.InstantiatedRoom = Instantiate(_roomPrefab, data.Position, Quaternion.identity, _dungeonParent);
        }

        HashSet<(RoomData, RoomData)> corridorsCreated = new HashSet<(RoomData, RoomData)>();

        foreach (RoomData data in _roomDatas)
		{
			RoomConnector connector = data.InstantiatedRoom.GetComponent<RoomConnector>();

            foreach(KeyValuePair<Direction, RoomData> neighbour in data.Neighbours)
            {
				Transform doorWaypoint = connector.GetDoorWaypointTransform(neighbour.Key);
                connector.OpenDoor(neighbour.Key);

                if (doorWaypoint != null)
				{
					//Destroy(door.gameObject);
					//=> supprimer la porte
					//=> connector.DestroyDoor(InverseDirection(data.Direction));
					//=> Destroy(doorWaypoint.GetChild(0));
				}

                RoomConnector neightboorRoomConnector = neighbour.Value.InstantiatedRoom.GetComponent<RoomConnector>();
                Direction oppositeDirection = GetOppositeDirection(neighbour.Key);
                Transform neighbourDoorWaypoint = neightboorRoomConnector.GetDoorWaypointTransform(oppositeDirection);

                if (!corridorsCreated.Contains((data, neighbour.Value)) && !corridorsCreated.Contains((neighbour.Value, data)))
                {
                    SpawnCorridor(doorWaypoint.position, neighbourDoorWaypoint.position);
                    corridorsCreated.Add((data, neighbour.Value));
                }
            }
            /*
                foreach (var direction in data.Directions)
                {
                    Transform doorWaypoint = connector.GetDoorWaypointTransform(direction);

                    if (doorWaypoint != null)
                    {
                        //Destroy(door.gameObject);
                        //=> supprimer la porte
                        //=> connector.DestroyDoor(InverseDirection(data.Direction));
                        //=> Destroy(doorWaypoint.GetChild(0));
                    }

                    Vector3 neighborPos = data.Position + DirectionToVector3(direction) * _roomSpacing;
                    SpawnCorridor(data.Position, neighborPos);
                }
            */
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

    private class RoomData
    {
        public Vector3 Position;
        public List<Direction> Directions = new();
        public Dictionary<Direction, RoomData> Neighbours = new();
        public Room InstantiatedRoom = null;

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

