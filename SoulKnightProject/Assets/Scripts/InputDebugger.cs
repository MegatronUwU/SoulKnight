using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputDebugger : MonoBehaviour
{
	private InputSystem_Actions _inputActions = null;

	private void Start()
	{
		_inputActions = new();
		_inputActions.Enable();

		_inputActions.Player.Jump.performed += OnJumpPerformed;
	}

	private void OnDestroy()
	{
		_inputActions.Player.Jump.performed -= OnJumpPerformed;
	}

	private void OnJumpPerformed(InputAction.CallbackContext context)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
