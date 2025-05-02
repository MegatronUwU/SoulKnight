using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform _renderer; 
    [SerializeField] private WeaponData _currentWeapon;

    public void TriggerAttack()
    {
        //if (_currentWeapon != null && _renderer != null && _currentWeapon.CanShoot())
        //{
        //    _currentWeapon.Shoot(_renderer, Team.Player);
        //}
        ShootAuto();
    }

    public void SetWeapon(WeaponData newWeapon)
    {
        _currentWeapon = newWeapon;
    }

    public void ShootAuto()
    {
        if (_currentWeapon == null || !_currentWeapon.CanShoot())
            return;

        if(TryFindClosestEnemy(out Transform target))
        {
			Vector3 direction = (target.position - _renderer.position).normalized;
			_renderer.forward = direction;
		}

        _currentWeapon.Shoot(_renderer, Team.Player);
    }

    private bool TryFindClosestEnemy(out Transform closestEnemy)
    {
        //TODO: Replace with enemy list in room
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
				closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
            return true;

        return false;
    }
}
