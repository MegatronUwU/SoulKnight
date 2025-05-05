using UnityEngine;

[CreateAssetMenu(fileName = "New Health Pickup Data", menuName = "Scriptable Objects/Pickups/Health Pickup Data")]
public class HealthPickupData : PickupData
{
	[SerializeField] private int _healAmount = 10;

	public override bool CanPickup(Player player)
	{
		return !player.PlayerHealth.IsFullHealth;
	}

	public override void PickupItem(Player player)
	{
		player.PlayerHealth.Heal(_healAmount);
	}
}
