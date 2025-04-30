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


    private void Start()
    {
        _playerTransform = _playerReferenceData.Player.transform;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        HandleMovement();
        TryShootAtPlayer();
    }

    private void HandleMovement()
    {
        Vector3 targetPosition = _playerTransform.position;
        Vector3 directionToPlayer = targetPosition - transform.position;
        float distance = directionToPlayer.magnitude;
        Vector3 direction = directionToPlayer.normalized;

        if (_currentRoomConnector != null && !IsPlayerInSameRoom())
        {
            Direction directionToPlayerRoom = GetDirectionToPlayerRoom(); 
            Transform doorWaypoint = _currentRoomConnector.GetDoorWaypointTransform(directionToPlayerRoom);

            if (doorWaypoint != null)
            {
                targetPosition = doorWaypoint.position;
                direction = (targetPosition - transform.position).normalized;
            }
        }

        if (distance > _stoppingDistance)
            transform.position += direction * _moveSpeed * Time.deltaTime;
        else if (distance < _retreatDistance)
            transform.position -= direction * _moveSpeed * Time.deltaTime;

        if (_renderer != null && direction != Vector3.zero)
            _renderer.forward = direction;
    }


    private void TryShootAtPlayer()
    {
        if (_weaponData == null)
            return;

        if (Time.time - _lastShotTime < 1f / _weaponData.FireRate)
            return;

        _lastShotTime = Time.time;

        //Vector3 spawnPos = _renderer.position + _renderer.forward * _shootOffset;
        _weaponData.Shoot(_renderer, Team.Enemy);

		//Projectile projectile = Instantiate(_weaponData.ProjectilePrefab, spawnPos, Quaternion.identity);
  //      projectile.Initialize(_renderer.forward, Team.Enemy);
    }

    private bool IsPlayerInSameRoom()
    {
        Vector3Int enemyGridPos = Vector3Int.RoundToInt(transform.position / 12f); 
        Vector3Int playerGridPos = Vector3Int.RoundToInt(_playerTransform.position / 12f);

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
