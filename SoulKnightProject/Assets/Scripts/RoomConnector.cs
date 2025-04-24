using UnityEngine;


/*public class RoomConnector : MonoBehaviour
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    [System.Serializable]
    public class DoorAnchor
    {
        public Direction direction;
        public Transform doorTransform;
    }

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
}*/

// un peu rincé, j'arrive à comprendre le principes mais la manière de faire est un peu bizarre