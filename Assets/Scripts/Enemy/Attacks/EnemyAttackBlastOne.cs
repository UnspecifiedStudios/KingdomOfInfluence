using System.Collections;
using UnityEngine;

public class EnemyAttackBlastOne : EnemyAttackBase
{
    public override IEnumerator AttackFunction()
    {
        Debug.Log("The enemy used attack 'Blast One'");
        yield return new WaitForSeconds(0f);
    }
}
