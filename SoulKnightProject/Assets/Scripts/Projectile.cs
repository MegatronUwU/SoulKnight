using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private Rigidbody _rigidbody = null; 

    private void Start()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
        Destroy(gameObject, _lifetime);
    }
}