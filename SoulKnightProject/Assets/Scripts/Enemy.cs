using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Health _health = null;
	public Health Health => _health;
}
