using System.Collections.Generic;
using UnityEngine;

public class DungeonPathGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private int _pathLength = 10;
    [SerializeField] private float _roomSpacing = 12f;
    [SerializeField] private Transform _dungeonParent;

    private readonly List<Vector3> _generatedPositions = new();

    private void Start()
    {
        GeneratePath();
    }

    // Pour générer les pièces
    private void GeneratePath()
    {
        Vector3 currentPosition = Vector3.zero;
        _generatedPositions.Add(currentPosition);

        Instantiate(_roomPrefab, currentPosition, Quaternion.identity, _dungeonParent);

        for (int i = 1; i < _pathLength; i++)
        {
            Vector3 direction = GetRandomCardinalDirection();
            currentPosition += direction * _roomSpacing;

            if (_generatedPositions.Contains(currentPosition))
                continue;

            _generatedPositions.Add(currentPosition);
            Instantiate(_roomPrefab, currentPosition, Quaternion.identity, _dungeonParent);
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
}
