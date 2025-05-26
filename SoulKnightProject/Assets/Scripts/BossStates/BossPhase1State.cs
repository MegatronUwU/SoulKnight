using UnityEngine;
using System.Collections.Generic;

public class BossPhase1State : IBossState
{
    private BossStateMachine _boss;
    private Transform _origin;
    private List<IBossAttack> _attacks;
    private float _cooldown = 2f;
    private float _timer;

    public BossPhase1State(BossStateMachine boss, Transform origin)
    {
        _boss = boss;
        _origin = origin;

        // Choisis ici les attaques de la phase 1
        _attacks = new List<IBossAttack>
        {
            new SimpleShot(_boss.Phase1Weapon),
            new FanShot(_boss.Phase1Weapon, 5, 45f)
        };
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
            var attack = _attacks[Random.Range(0, _attacks.Count)];
            attack.Execute(_origin);
            _timer = _cooldown;
        }
<<<<<<< HEAD

        if (_boss.Health.CurrentHealth <= _boss.Health.MaxHealth / 2)
        {
            _boss.SetState(new BossPhase2State(_boss, _origin));
        }
=======
>>>>>>> b02e063ea034a49a43fb9856cb2a96aaf448c8c7
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
