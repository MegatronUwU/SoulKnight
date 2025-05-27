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

    public WeaponData Phase1Weapon => _phase1Weapon;
    public WeaponData Phase2Weapon => _phase2Weapon;

    private Animator _animator;
    public Animator Animator => _animator;

    [SerializeField] private PlayerReferenceData _playerReferenceData;
    public Transform PlayerTarget => _playerReferenceData.Player.transform;

    public Transform BossTransform => transform;


    private void Start()
    {
        SetState(new BossPhase1State(this, _shootOrigin));
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
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
