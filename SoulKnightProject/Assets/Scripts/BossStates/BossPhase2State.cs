using UnityEngine;

public class BossPhase2State : IBossState
{
    private BossStateMachine _boss;
    private WeaponData _weapon;
    private Transform _origin;
    private float _cooldown = 1f;
    private float _timer;

    public BossPhase2State(BossStateMachine boss, WeaponData weapon, Transform origin)
    {
        _boss = boss;
        _weapon = weapon;
        _origin = origin;
    }

    public void Enter()
    {
        Debug.Log("Boss Phase 2");
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
    }

    public void Exit()
    {
        Debug.Log("Exit Phase 2");
    }
}
