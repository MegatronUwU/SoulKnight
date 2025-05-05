using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupData _pickupData = null;

    private void OnTriggerEnter(Collider other)
    {
		if (!other.TryGetComponent(out Player player))
            return;

		if (!_pickupData.CanPickup(player))
            return;

        _pickupData.PickupItem(player);

        Destroy(gameObject);
    }
}