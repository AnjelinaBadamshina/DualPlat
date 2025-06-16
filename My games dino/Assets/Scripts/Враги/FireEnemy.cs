using UnityEngine;

public class FireEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // наносит 1 урона при контакте (используется твоим Damage1p, можно проигнорировать)
    }
}
