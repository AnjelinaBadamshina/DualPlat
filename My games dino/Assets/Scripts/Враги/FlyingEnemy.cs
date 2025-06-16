using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // Воздушный враг наносит 1 урона
    }
}
