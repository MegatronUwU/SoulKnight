using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Slider _healthSlider;

    private void Start()
    {
        if (_playerHealth != null && _healthSlider != null)
        {
            _healthSlider.maxValue = _playerHealth.GetMaxHealth();
            _healthSlider.value = _playerHealth.GetCurrentHealth();
        }
    }

    private void Update()
    {
        if (_playerHealth != null && _healthSlider != null)
        {
            _healthSlider.value = _playerHealth.GetCurrentHealth();
        }
    }
}
