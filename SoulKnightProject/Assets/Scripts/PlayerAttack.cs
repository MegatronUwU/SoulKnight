using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _projectileLifetime = 2f;
    [SerializeField] private float _fireCooldown = 0.5f;
    [SerializeField] private float _projectileSpawnOffset = 1f;
    [SerializeField] private Transform _renderer = null;

    private float _lastFireTime;

    public void TriggerAttack()
    {
        if (Time.time - _lastFireTime < _fireCooldown)
            return;

        _lastFireTime = Time.time;

        Vector3 spawnPos = _renderer.position + _renderer.forward * _projectileSpawnOffset;
        Projectile projectile = Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);

        projectile.Initialize(_renderer.forward);
    }
}
