using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
	public Vector3 Position;
	public List<Direction> Directions = new();
	public Dictionary<Direction, RoomData> Neighbours = new();
	public Room InstantiatedRoom = null;
	public RoomConfiguration Configuration;
	public bool IsMergedToBigRoom = false;
	public List<RoomData> BossRooms;

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