using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _projectileLifetime = 2f;
    [SerializeField] private float _fireCooldown = 0.5f;

    private float _lastFireTime;

    public void TriggerAttack()
    {
        if (Time.time - _lastFireTime < _fireCooldown)
            return;

        _lastFireTime = Time.time;

        Vector3 spawnPos = transform.position + transform.forward * 1f;
        GameObject projectile = Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);

        if (projectile.TryGetComponent(out Rigidbody rb))
        {
            // Dois sûrement faire tourner mon player, besoin de fix ça
            rb.linearVelocity = transform.forward * _projectileSpeed;
        }

        Destroy(projectile, _projectileLifetime);
    }
}
