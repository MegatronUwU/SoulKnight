using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Pickup Data", menuName = "Scriptable Objects/Pickups/Ammo Pickup Data")]
public class AmmoPickupData : PickupData
{
	[SerializeField] private int _ammoAmount = 1;

	public override bool CanPickup(Player player)
	{
		if (player.PlayerWeaponHandler == null)
			return false;

		return player.PlayerWeaponHandler.GetCurrentAmmo() < player.PlayerWeaponHandler.GetMaxAmmo();
	}

	public override void PickupItem(Player player)
	{
		player.PlayerWeaponHandler.AddAmmo(_ammoAmount);
	}
}
