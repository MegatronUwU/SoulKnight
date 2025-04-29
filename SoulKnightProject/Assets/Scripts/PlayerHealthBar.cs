using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Image _healthSlider;

	private void Start()
	{
		_playerHealth.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(int currentHealth, int maxHealth)
	{
		if (_healthSlider == null)
			return;

		_healthSlider.fillAmount = currentHealth / maxHealth;
	}
}