using UnityEngine;

public class WaterEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // наносит 1 урона при контакте
    }
}
