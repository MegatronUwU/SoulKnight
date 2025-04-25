using System.Collections.Generic;
using UnityEngine;

public class DungeonPathGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private int _pathLength = 10;
    [SerializeField] private float _roomSpacing = 12f;
    [SerializeField] private Transform _dungeonParent;

    private readonly List<RoomData> _roomDatas = new();

    private void Start()
    {
        GeneratePath();
        SpawnRooms();
    }

    private void GeneratePath()
    {
        Vector3 currentPosition = Vector3.zero;
        _roomDatas.Add(new RoomData(currentPosition, null)); 

        for (int i = 1; i < _pathLength; i++)
        {
            Vector3 direction = GetRandomCardinalDirection();
            Vector3 nextPosition = currentPosition + direction * _roomSpacing;

            if (_roomDatas.Exists(r => r.Position == nextPosition))
                continue;

            RoomConnector.Direction entryDir = GetOppositeDirection(direction);
            _roomDatas.Add(new RoomData(nextPosition, entryDir));

            currentPosition = nextPosition;
        }
    }

        /*

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

            if (data.EntryDirection.HasValue)
            {
                RoomConnector connector = room.GetComponent<RoomConnector>();
                Transform door = connector.GetDoorTransform(data.EntryDirection.Value);
                if (door != null)
                    door.gameObject.SetActive(false); 
            }
        }
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

    // On return la direction opposé pour savoir où est la porte d'entrée
    private RoomConnector.Direction GetOppositeDirection(Vector3 dir)
    {
        if (dir == Vector3.forward) return RoomConnector.Direction.Down;
        if (dir == Vector3.back) return RoomConnector.Direction.Up;
        if (dir == Vector3.left) return RoomConnector.Direction.Right;
        return RoomConnector.Direction.Left; 
    }

    private class RoomData
    {
        public Vector3 Position;
        public RoomConnector.Direction? EntryDirection;

        public RoomData(Vector3 pos, RoomConnector.Direction? entryDir)
        {
            Position = pos;
            EntryDirection = entryDir;
        }
    }
}


