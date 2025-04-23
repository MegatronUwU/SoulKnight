using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        if (direction.magnitude > 0.1f)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction.normalized * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}