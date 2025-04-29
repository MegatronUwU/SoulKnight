using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private PlayerReferenceData _playerReferenceData = null;

	private void Awake()
	{
		_playerReferenceData.SetPlayerReference(this);
	}
}
