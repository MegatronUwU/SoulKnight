using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private DoorAnchor[] _doorAnchors;

    public Transform GetDoorTransform(Direction direction)
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.direction == direction)
                return anchor.doorTransform;
        }

        return null;
    }

	[System.Serializable]
	public class DoorAnchor
	{
		public Direction direction;
		public Transform doorTransform;
	}

	public enum Direction
	{
		Up, Down, Left, Right
	}
}

// un peu rinc�, j'arrive � comprendre le principes mais la mani�re de faire est un peu bizarre