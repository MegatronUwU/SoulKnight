using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth => _maxHealth;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;

    public bool IsFullHealth => _currentHealth == _maxHealth;

	public UnityEvent OnDeath;

    public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
    public event HealthChangedEventHandler HealthChanged;

    private Animator _animator;

    private bool _isDead = false;



    private void Awake()
    {
        _currentHealth = _maxHealth;
	}

	private IEnumerator Start()
	{
        _animator = GetComponentInChildren<Animator>();
        yield return null;
		HealthChanged?.Invoke(_currentHealth, _maxHealth);
	}

	public void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0);
		HealthChanged?.Invoke(_currentHealth, _maxHealth);

		if (_currentHealth == 0)
            Die();
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
		HealthChanged?.Invoke(_currentHealth, _maxHealth);
        
        SoundManager.Instance.Play("Heal");
    }

	private void Die()
    {
        if (_isDead) return;      
        _isDead = true;

        if (_animator != null)
            _animator.SetLayerWeight(1, 0f);
            _animator.SetTrigger("Die");
        
        SoundManager.Instance.Play("Die");

        OnDeath?.Invoke();
        Destroy(gameObject, 3f);
    }
}
