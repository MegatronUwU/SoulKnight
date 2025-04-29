using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _renderer;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _shootOffset = 1f;

	[SerializeField]
	private PlayerReferenceData _playerReferenceData = null;

	private Transform _playerTransform;
    private float _lastShotTime;

    private void Start()
    {
        _playerTransform = _playerReferenceData.Player.transform;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        MoveTowardsPlayer();
        TryShootAtPlayer();
    }

    private void MoveTowardsPlayer()
	{
		Vector3 direction = (_playerTransform.position - transform.position).normalized;
		transform.position += _moveSpeed * Time.deltaTime * direction;

		if (_renderer == null || direction == Vector3.zero)
			return;

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
}
