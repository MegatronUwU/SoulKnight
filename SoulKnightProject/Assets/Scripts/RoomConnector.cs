using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private DoorAnchor[] _doorAnchors;

    private void Awake()
    {
        foreach (DoorAnchor doorAnchor in _doorAnchors)
            doorAnchor.Door.SetActive(false);
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
        anchor.Door.SetActive(true);
    }

    public void CloseAllDoors()
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.Door != null)
            {
                anchor.Door.SetActive(true);

                var col = anchor.Door.GetComponent<Collider>();
                if (col != null) col.enabled = true;

                var animator = anchor.Door.GetComponent<Animator>();
                if (animator != null) animator.SetTrigger("Close");
            }

            if (anchor.Wall != null)
                anchor.Wall.SetActive(false);
        }
    }

    public void OpenAllDoors()
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.Door != null)
            {
                var col = anchor.Door.GetComponent<Collider>();
                if (col != null) col.enabled = false;

                var animator = anchor.Door.GetComponent<Animator>();
                if (animator != null) animator.SetTrigger("Open");

                anchor.Door.SetActive(false);
            }

            if (anchor.Wall != null)
                anchor.Wall.SetActive(true);
        }
    }

    [System.Serializable]
    public class DoorAnchor
    {
        public Direction direction;
        public Transform doorTransform;
        public GameObject Wall;
        public GameObject Door;
    }
}

public enum Direction
{
    Up, Down, Left, Right
}

// un peu rincé, j'arrive à comprendre le principes mais la manière de faire est un peu bizarre
