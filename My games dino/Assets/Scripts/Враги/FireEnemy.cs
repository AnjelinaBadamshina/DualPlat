using UnityEngine;

public class FireEnemy : EnemyBase
{
    protected override int GetContactDamage()
    {
        return 1; // ������� 1 ����� ��� �������� (������������ ����� Damage1p, ����� ���������������)
    }
}
