using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility

{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public void Activate()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire point is not assigned!");
            return;
        }
            
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        Vector3 projectileScale = projectile.transform.localScale;
        projectileScale.x = Mathf.Abs(projectileScale.x) * direction;
        projectile.transform.localScale = projectileScale;

        FireProjectile waterScript = projectile.GetComponent<FireProjectile>();
        if (waterScript != null)
        {
            waterScript.SetDirection(direction);
            Debug.Log("Direction set to: " + direction);
        }
        else
        {
            Debug.LogWarning("FireProjectile script not found on projectile");
        }
    }


}
