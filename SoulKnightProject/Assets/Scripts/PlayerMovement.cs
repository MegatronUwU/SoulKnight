using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _renderer = null;

    private Animator _animator;
    private bool _isRunningSoundPlaying = false;


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        float speed = direction.magnitude;


        if (speed > 0.1f)
        {
			_renderer.forward = direction.normalized;
            transform.Translate(_moveSpeed * Time.deltaTime * direction);
            //GetComponent<Rigidbody>().linearVelocity = _moveSpeed * Time.deltaTime * direction;
            
            if (!_isRunningSoundPlaying)
            {
                SoundManager.Instance.Play("Run");
                _isRunningSoundPlaying = true;
            }
        }
        else
        {
            _isRunningSoundPlaying = false;
        }
       
        _animator.SetFloat("Speed", speed);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out Door door))
            door.EnterRoom();
	}
}