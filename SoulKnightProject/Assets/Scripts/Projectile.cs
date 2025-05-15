using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 2f;

    [SerializeField] LayerMask PlayerLayer;
	[SerializeField] LayerMask EnemyLayer;

    [SerializeField] private ProjectileTrailData _trailData;

    private Vector3 _movementDirection = Vector3.zero;

    private void Start()
    {
        var trail = GetComponent<TrailRenderer>();
        if (trail != null && _trailData != null)
        {
            trail.time = _trailData.TrailTime;
            trail.startWidth = _trailData.StartWidth;
            trail.endWidth = _trailData.EndWidth;
            trail.colorGradient = _trailData.ColorOverTime;

            if (_trailData.TrailMaterial != null)
                trail.material = _trailData.TrailMaterial;
        }
    }

    public void Initialize(Vector3 movementDirection, Team team)
    {
        if(team == Team.Player)
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        else
			gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");

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