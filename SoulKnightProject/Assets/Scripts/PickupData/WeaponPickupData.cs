using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Pickup Data", menuName = "Scriptable Objects/Pickups/Weapon Pickup Data")]
public class WeaponPickupData : PickupData
{
	[SerializeField] private WeaponData _weapon = null;

	public override void PickupItem(Player player)
	{
		player.PlayerWeaponHandler.SetWeapon(_weapon);
	}
}
