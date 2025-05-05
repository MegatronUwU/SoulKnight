using UnityEngine;

public abstract class PickupData : ScriptableObject
{
	public virtual bool CanPickup(Player player) => true;

	public abstract void PickupItem(Player player);
}
