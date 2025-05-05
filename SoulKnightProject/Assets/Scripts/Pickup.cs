using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Ammo, Health, Weapon }

    [SerializeField] private PickupType _pickupType;
    [SerializeField] private int _amount = 1;
    [SerializeField] private WeaponData _weaponToGive;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}");

        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        switch (_pickupType)
        {
            case PickupType.Ammo:
                WeaponHandler handler = player.GetComponent<WeaponHandler>();
                if (handler != null)
                {
                    handler.AddAmmo(_amount);
                    Debug.Log($"+{_amount} Ammo");
                }
                break;

            case PickupType.Health:
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.Heal(_amount);
                    Debug.Log($"+{_amount} HP");
                }
                break;

            case PickupType.Weapon:
                if (_weaponToGive != null)
                {
                    WeaponHandler weaponHandler = player.GetComponent<WeaponHandler>();
                    if (weaponHandler != null)
                    {
                        weaponHandler.SetWeapon(_weaponToGive);
                        Debug.Log($"Picked up weapon: {_weaponToGive.WeaponName}");
                    }
                }
                break;
        }

        Destroy(gameObject);
    }
}
