using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField]
	private GameObject _closedCollider = null;
	[SerializeField]
	private Animator _doorAnimator = null;

	private Room _room = null;

	public void Initialize(Room room)
	{
		_room = room;
	}

	public void EnterRoom()
	{
		_room.StartRoom();
	}

	public void OpenDoor()
	{
		if (!gameObject.activeSelf)
			return;

		_closedCollider.SetActive(false);
		_doorAnimator.SetTrigger("Open");
		
		SoundManager.Instance.Play("DoorOpen");
	}

	public void CloseDoor()
	{
		if (!gameObject.activeSelf)
			return;

		_closedCollider.SetActive(true);
		_doorAnimator.SetTrigger("Close");
	}
}
