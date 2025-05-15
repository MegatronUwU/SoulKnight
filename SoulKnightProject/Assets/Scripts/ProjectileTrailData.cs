using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileTrail", menuName = "Scriptable Objects/Projectiles/Trail Settings")]
public class ProjectileTrailData : ScriptableObject
{
    public float TrailTime = 0.5f;
    public float StartWidth = 0.2f;
    public float EndWidth = 0f;
    public Gradient ColorOverTime;
    public Material TrailMaterial;
}