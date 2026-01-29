using System.Collections;
using UnityEngine;

public class EnemyAttackShootOne : EnemyAttackBase
{
    public override IEnumerator AttackFunction()
    {
        Debug.Log("The enemy used attack 'Shoot One'");
        yield return new WaitForSeconds(0f);
    }
}
