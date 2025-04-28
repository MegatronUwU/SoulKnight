using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private DoorAnchor[] _doorAnchors;
    [SerializeField] private GameObject _doorLeft;
    [SerializeField] private GameObject _doorRight;
    [SerializeField] private GameObject _doorUp;
    [SerializeField] private GameObject _doorDown;

    [SerializeField] private GameObject _wallLeft;
    [SerializeField] private GameObject _wallRight;
    [SerializeField] private GameObject _wallUp;
    [SerializeField] private GameObject _wallDown;

    private void Awake()
    {
        if (_doorLeft != null) _doorLeft.SetActive(false);
        if (_doorRight != null) _doorRight.SetActive(false);
        if (_doorUp != null) _doorUp.SetActive(false);
        if (_doorDown != null) _doorDown.SetActive(false);
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

    public void OpenDoor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (_doorLeft != null) _doorLeft.SetActive(true);
                if (_wallLeft != null) _wallLeft.SetActive(false);
                break;
            case Direction.Right:
                if (_doorRight != null) _doorRight.SetActive(true);
                if (_wallRight != null) _wallRight.SetActive(false);
                break;
            case Direction.Up:
                if (_doorUp != null) _doorUp.SetActive(true);
                if (_wallUp != null) _wallUp.SetActive(false);
                break;
            case Direction.Down:
                if (_doorDown != null) _doorDown.SetActive(true);
                if (_wallDown != null) _wallDown.SetActive(false);
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