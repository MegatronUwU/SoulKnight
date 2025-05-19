using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _renderer;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _shootOffset = 1f;
    
    [SerializeField] private float _stoppingDistance = 7f;
    [SerializeField] private float _retreatDistance = 3f;

    [SerializeField]
	private PlayerReferenceData _playerReferenceData = null;

	private Transform _playerTransform;
    private float _lastShotTime;
    private RoomConnector _currentRoomConnector;

    private Animator _animator;
    private bool _isDead = false;




    private void Start()
    {
        _playerTransform = _playerReferenceData.Player.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_playerTransform == null) return;
        if (_isDead) return;

        HandleMovement();
        TryShootAtPlayer();
    }

    private void HandleMovement()
    {
        Vector3 playerPosition = _playerTransform.position;
        Vector3 directionToPlayer = playerPosition - transform.position;
        Vector3 directionToPlayerNormalized = directionToPlayer.normalized;
        float distanceToPlayer = directionToPlayer.magnitude;

        _animator.SetFloat("Speed", directionToPlayer.magnitude);

        if (_currentRoomConnector != null && !IsPlayerInSameRoom())
        {
            Direction directionToPlayerRoom = GetDirectionToPlayerRoom(); 
            Transform doorWaypoint = _currentRoomConnector.GetDoorWaypointTransform(directionToPlayerRoom);

            if (doorWaypoint != null)
            {
                playerPosition = doorWaypoint.position;
                directionToPlayerNormalized = (playerPosition - transform.position).normalized;
            }
        }

        if (distanceToPlayer > _stoppingDistance)
            transform.position += _moveSpeed * Time.deltaTime * directionToPlayerNormalized;
        else if (distanceToPlayer < _retreatDistance)
            transform.position -= _moveSpeed * Time.deltaTime * directionToPlayerNormalized;

        if (_renderer != null && directionToPlayerNormalized != Vector3.zero)
            _renderer.forward = directionToPlayerNormalized;
    }


    private void TryShootAtPlayer()
    {
        if (_weaponData == null)
            return;

        if (Time.time - _lastShotTime < 1f / _weaponData.FireRate)
            return;

        _lastShotTime = Time.time;

        _animator.SetTrigger("Shoot");

        //Vector3 spawnPos = _renderer.position + _renderer.forward * _shootOffset;
        _weaponData.Shoot(_renderer, Team.Enemy);

		//Projectile projectile = Instantiate(_weaponData.ProjectilePrefab, spawnPos, Quaternion.identity);
  //      projectile.Initialize(_renderer.forward, Team.Enemy);
    }

    private bool IsPlayerInSameRoom()
    {
        //TODO: Swich "25f" room size to a ScriptableObject
        Vector3Int enemyGridPos = Vector3Int.RoundToInt(transform.position / 25f); 
        Vector3Int playerGridPos = Vector3Int.RoundToInt(_playerTransform.position / 25f);

        return enemyGridPos == playerGridPos;
    }

    private Direction GetDirectionToPlayerRoom()
    {
        Vector3 diff = _playerTransform.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
        {
            return diff.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return diff.z > 0 ? Direction.Up : Direction.Down;
        }
    }
}
