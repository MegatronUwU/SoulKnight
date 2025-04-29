using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _renderer;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private float _moveSpeed = 3f;

    private Transform _player;
    private float _lastShotTime;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        MoveTowardsPlayer();
        TryShootAtPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;

        if (_renderer != null)
        {
            _renderer.forward = direction;
        }
    }

    private void TryShootAtPlayer()
    {
        if (_weaponData == null)
            return;

        if (Time.time - _lastShotTime < 1f / _weaponData.FireRate)
            return;

        _lastShotTime = Time.time;

        Vector3 spawnPos = _renderer.position + _renderer.forward * 1f;
        Projectile projectile = Instantiate(_weaponData.ProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.Initialize(_renderer.forward);
    }
}
