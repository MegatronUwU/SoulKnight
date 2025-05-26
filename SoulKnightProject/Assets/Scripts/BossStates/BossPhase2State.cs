using UnityEngine;
using System.Collections.Generic;

public class BossPhase2State : IBossState
{
    private BossStateMachine _boss;
    private Transform _origin;
    private List<IBossAttack> _attacks;
    private float _cooldown = 1.5f;
    private float _timer;

    public BossPhase2State(BossStateMachine boss, Transform origin)
    {
        _boss = boss;
        _origin = origin;

        _attacks = new List<IBossAttack>
        {
            new FanShot(_boss.Phase2Weapon, 7, 60f),
            new SpiralShot(_boss.Phase2Weapon, 12, 30f)
        };
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
            var attack = _attacks[Random.Range(0, _attacks.Count)];
            attack.Execute(_origin);
            _timer = _cooldown;
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Phase 2");
    }
}
