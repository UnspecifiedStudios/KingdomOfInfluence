using System.Collections;
using UnityEngine;

public class EnemyAttackSwingOne : EnemyAttackBase
{
    public override IEnumerator AttackFunction()
    {
        Debug.Log("The enemy used attack 'Swing One'");
        yield return new WaitForSeconds(0f);
    }
}
