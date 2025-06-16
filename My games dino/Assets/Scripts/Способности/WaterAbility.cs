using UnityEngine;

public class WaterAbility : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;
    public Transform WaterPoint;

    public void Activate()
    {
        GameObject projectile = Instantiate(projectilePrefab, WaterPoint.position, WaterPoint.rotation);

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        Vector3 projectileScale = projectile.transform.localScale;
        projectileScale.x = Mathf.Abs(projectileScale.x) * direction;
        projectile.transform.localScale = projectileScale;

        WaterProjectile waterScript = projectile.GetComponent<WaterProjectile>();
        if (waterScript != null)
        {
            waterScript.SetDirection(direction);
        }
    }
}
