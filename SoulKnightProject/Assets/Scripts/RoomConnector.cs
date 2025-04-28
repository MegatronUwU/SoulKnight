using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private DoorAnchor[] _doorAnchors;
    [SerializeField] private GameObject _doorLeft;
    [SerializeField] private GameObject _doorRight;
    [SerializeField] private GameObject _doorUp;
    [SerializeField] private GameObject _doorDown;

    public Transform GetDoorWaypointTransform(Direction direction)
    {
        foreach (DoorAnchor anchor in _doorAnchors)
        {
            if (anchor.direction == direction)
                return anchor.doorTransform;
        }

        return null;
    }

    public void OpenDoor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (_doorLeft != null) _doorLeft.SetActive(true);
                break;
            case Direction.Right:
                if (_doorRight != null) _doorRight.SetActive(true);
                break;
            case Direction.Up:
                if (_doorUp != null) _doorUp.SetActive(true);
                break;
            case Direction.Down:
                if (_doorDown != null) _doorDown.SetActive(true);
                break;
        }
    }

    [System.Serializable]
    public class DoorAnchor
    {
        public Direction direction;
        public Transform doorTransform;
    }
}

public enum Direction
{
    Up, Down, Left, Right
}


// un peu rincé, j'arrive à comprendre le principes mais la manière de faire est un peu bizarre