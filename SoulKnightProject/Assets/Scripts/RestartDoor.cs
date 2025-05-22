using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartDoor : MonoBehaviour
{
    private bool _isActive = false;

    public void ActivateDoor()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive) return;
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
