using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthSlider;
    [SerializeField] private TMP_Text _bossNameText;

    private Health _bossHealth;

    private void Start()
    {
        gameObject.SetActive(false); 
    }
    private void OnDisable()
    {
        if (_bossHealth != null)
            _bossHealth.HealthChanged -= OnHealthChanged;
    }

    public void Init(Health bossHealth, string bossName)
    {
        _bossHealth = bossHealth;

        if (_bossHealth != null)
        {
            _bossHealth.HealthChanged += OnHealthChanged;
            OnHealthChanged(_bossHealth.CurrentHealth, _bossHealth.MaxHealth);
        }

        if (_bossNameText != null)
            _bossNameText.text = bossName;

        gameObject.SetActive(true);
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        if (_healthSlider == null)
            return;

        _healthSlider.fillAmount = (float)currentHealth / maxHealth;
    }
}
