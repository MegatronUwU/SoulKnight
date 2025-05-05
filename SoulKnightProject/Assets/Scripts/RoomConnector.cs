using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private DoorAnchor[] _doorAnchors;

    private void Awake()
    {
        foreach (DoorAnchor doorAnchor in _doorAnchors)
            doorAnchor.Door.gameObject.SetActive(false);
    }

    public void InitializeDoors(Room room)
    {
        foreach(DoorAnchor doorAnchor in _doorAnchors)
        {
            if (doorAnchor.Door == null)
                continue;

            doorAnchor.Door.Initialize(room);
        }
    }


	public Transform GetDoorWaypointTransform(Direction direction)
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.direction == direction)
                return anchor.doorTransform;
        }

        return null;
    }

    public DoorAnchor GetDoorAnchorFromDirection(Direction direction)
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.direction == direction)
                return anchor;
        }

        return null;
    }

    public void OpenDoor(Direction direction)
    {
        DoorAnchor anchor = GetDoorAnchorFromDirection(direction);

        if (anchor == null || anchor.Door == null || anchor.Wall == null)
            return;

        Destroy(anchor.Wall);
        anchor.Door.gameObject.SetActive(true);
    }

    public void CloseAllDoors()
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.Door != null)
                anchor.Door.CloseDoor();
        }
    }

    public void OpenAllDoors()
    {
		foreach (DoorAnchor anchor in _doorAnchors)
		{
			if (anchor.Door != null)
				anchor.Door.OpenDoor();
		}
	}

    public void DestroyWall(Direction direction)
    {
        DoorAnchor anchor = GetDoorAnchorFromDirection(direction);

        if (anchor != null && anchor.Wall != null)
        {
            Destroy(anchor.Wall);
            anchor.Wall = null;
        }
    }

    [System.Serializable]
    public class DoorAnchor
    {
        public Direction direction;
        public Transform doorTransform;
        public GameObject Wall;
        public Door Door;
    }
}

public enum Direction
{
    Up, Down, Left, Right
}

// un peu rincé, j'arrive à comprendre le principes mais la manière de faire est un peu bizarre
