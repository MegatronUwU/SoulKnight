using UnityEngine;

public class BossPhase1State : IBossState
{
    private BossStateMachine _boss;
    private WeaponData _weapon;
    private Transform _origin;
    private float _cooldown = 2f;
    private float _timer;

    public BossPhase1State(BossStateMachine boss, WeaponData weapon, Transform origin)
    {
        _boss = boss;
        _weapon = weapon;
        _origin = origin;
    }

    public void Enter()
    {
        Debug.Log("Boss Phase 1");
        _timer = _cooldown;

        _boss.Health.HealthChanged += OnHealthChanged;
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _weapon.Shoot(_origin, Team.Enemy);
            _timer = _cooldown;
        }
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
	{
		if (currentHealth <= maxHealth / 2)
		{
			_boss.SetState(new BossPhase2State(_boss, _boss.Phase2Weapon, _origin));
		}
	}

	public void Exit()
    {
        _boss.Health.HealthChanged -= OnHealthChanged;
        Debug.Log("Exit Phase 1");
	}
}
