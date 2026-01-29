using System.Collections;
using UnityEngine;

public class EnemyAttackBase : MonoBehaviour
{
    public virtual IEnumerator AttackFunction()
    {
        Debug.Log("AttackFunction() is not overridden. Please code in an override.");
        yield return new WaitForSeconds(0f);
    }
}
