using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerReferenceData", menuName = "Scriptable Objects/PlayerReferenceData")]
public class PlayerReferenceData : ScriptableObject
{
	private Player _player;
	public Player Player => _player;

	public void SetPlayerReference(Player player) => _player = player;
}
