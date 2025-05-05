using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private PlayerReferenceData _playerReferenceData = null;

	public Health PlayerHealth = null;
	public WeaponHandler PlayerWeaponHandler = null;

	private void Awake()
	{
		_playerReferenceData.SetPlayerReference(this);
	}
}
