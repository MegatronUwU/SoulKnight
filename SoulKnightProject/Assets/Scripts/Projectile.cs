using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 2f;

    private Vector3 _movementDirection = Vector3.zero;

    public void Initialize(Vector3 movementDirection)
    {
        _movementDirection = movementDirection;

        Destroy(gameObject, _lifetime);
    }

	private void Update()
	{
		transform.Translate(_movementDirection * _speed * Time.deltaTime);
	}

}