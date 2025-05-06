using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _ammoText;

    public void UpdateUI(string weaponName, int currentAmmo, int maxAmmo)
    {
        _weaponNameText.text = weaponName;
        _ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }
}