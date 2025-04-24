using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private Vector3 _offset = new Vector3(0f, 15f, -10f); 
    [SerializeField] private float _followSpeed = 5f; 

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 targetPosition = _target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
    }
}