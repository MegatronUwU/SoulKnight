using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _renderer = null;

    private void Update()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);

        if (direction.magnitude > 0.1f)
        {
			_renderer.forward = direction.normalized;
            transform.Translate(_moveSpeed * Time.deltaTime * direction);
            //GetComponent<Rigidbody>().linearVelocity = _moveSpeed * Time.deltaTime * direction;
        }
    }
}