using UnityEngine;

public class WaterEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // ������� 1 ����� ��� ��������
    }
}
