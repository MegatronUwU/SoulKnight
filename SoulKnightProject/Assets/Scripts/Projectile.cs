using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 2f;

    [SerializeField] LayerMask PlayerLayer;
	[SerializeField] LayerMask EnemyLayer;

	private Vector3 _movementDirection = Vector3.zero;

    public void Initialize(Vector3 movementDirection, Team team)
    {
        if(team == Team.Player)
            gameObject.layer = LayerMask.NameToLayer("Player");
        else
			gameObject.layer = LayerMask.NameToLayer("Enemy");

		_movementDirection = movementDirection;

        Destroy(gameObject, _lifetime);
    }

	private void Update()
	{
		transform.Translate(_speed * Time.deltaTime * _movementDirection);
	}

	private void OnTriggerEnter(Collider other)
	{
        if(other.TryGetComponent(out Health health))
        {
            health.TakeDamage(10);
        }

		Destroy(gameObject);
	}
}