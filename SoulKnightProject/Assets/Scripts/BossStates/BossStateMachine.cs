using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    private IBossState _currentState;
    [SerializeField]
    private Health _health = null;
    public Health Health => _health;

	[SerializeField] private Transform _shootOrigin;
    [SerializeField] private WeaponData _phase1Weapon;
    [SerializeField] private WeaponData _phase2Weapon;
    public WeaponData Phase2Weapon => _phase2Weapon;

    private void Start()
    {
        SetState(new BossPhase1State(this, _phase1Weapon, _shootOrigin));
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void SetState(IBossState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}
