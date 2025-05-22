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
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _weapon.Shoot(_origin, Team.Enemy);
            _timer = _cooldown;
        }

        if (_boss.Health.CurrentHealth <= _boss.Health.MaxHealth / 2)
        {
            _boss.SetState(new BossPhase2State(_boss, _boss.Phase2Weapon, _origin));
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Phase 1");
    }
}
