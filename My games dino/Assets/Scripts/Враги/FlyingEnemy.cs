using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // ��������� ���� ������� 1 �����
    }
}
