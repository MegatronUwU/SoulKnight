using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonPathGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
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
            GameObject room = Instantiate(_roomPrefab, data.Position, Quaternion.identity, _dungeonParent);
            RoomConnector connector = room.GetComponent<RoomConnector>();

            foreach (var direction in data.Directions)
            {
                Transform door = connector.GetDoorTransform(direction);
                if (door != null)
                    door.gameObject.SetActive(false);

                Vector3 neighborPos = data.Position + DirectionToVector3(direction) * _roomSpacing;
                SpawnCorridor(data.Position, neighborPos);
            }
        }
    }

    // On cherche les connexions pour chaque pièce
    private void FindRoomsNeighbours()
    {
        foreach (RoomData roomData in _roomDatas)
        {
            if (_roomDatas.Any(rd => rd.Position == roomData.Position + Vector3.left))
                roomData.AddNeighbourPosition(RoomConnector.Direction.Left);

            if (_roomDatas.Any(rd => rd.Position == roomData.Position + Vector3.right))
                roomData.AddNeighbourPosition(RoomConnector.Direction.Right);

            if (_roomDatas.Any(rd => rd.Position == roomData.Position + Vector3.forward))
                roomData.AddNeighbourPosition(RoomConnector.Direction.Up);

            if (_roomDatas.Any(rd => rd.Position == roomData.Position + Vector3.back))
                roomData.AddNeighbourPosition(RoomConnector.Direction.Down);
        }
    }

    // On spawn un couloir entre deux positions
    private void SpawnCorridor(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        Vector3 position = from + direction / 2f;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Instantiate(_corridorPrefab, position, rotation, _dungeonParent);
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
    private Vector3 DirectionToVector3(RoomConnector.Direction direction)
    {
        return direction switch
        {
            RoomConnector.Direction.Up => Vector3.forward,
            RoomConnector.Direction.Down => Vector3.back,
            RoomConnector.Direction.Left => Vector3.left,
            RoomConnector.Direction.Right => Vector3.right,
            _ => Vector3.zero
        };
    }

    private class RoomData
    {
        public Vector3 Position;
        public List<RoomConnector.Direction> Directions = new();

        public RoomData(Vector3 pos)
        {
            Position = pos;
        }

        public void AddNeighbourPosition(RoomConnector.Direction direction)
        {
            if (!Directions.Contains(direction))
                Directions.Add(direction);
        }
    }
}

