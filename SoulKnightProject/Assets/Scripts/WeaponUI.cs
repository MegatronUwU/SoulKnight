using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _ammoText;

    [SerializeField] private Image _cooldownBar;

    public void UpdateUI(string weaponName, int currentAmmo, int maxAmmo)
    {
        _weaponNameText.text = weaponName;
        _ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    public void UpdateCooldown(float fillAmount)
    {
        if (_cooldownBar != null)
            _cooldownBar.fillAmount = fillAmount;
    }
}