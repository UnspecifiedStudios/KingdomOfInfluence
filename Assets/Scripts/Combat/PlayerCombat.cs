using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public float lightAttackDuration = 1.5f;
    public float heavyAttackDuration = 2f;
    public float shieldDuration = 3.5f;
    public float beamAttackDuration = 3f;
    public GameObject lightAttackHitbox;
    public GameObject heavyAttackHitbox;
    public GameObject shieldHitbox;
    public GameObject beamAttackHitbox;

    private bool isLightAttacking = false;
    private bool isHeavyAttacking = false;
    private bool isBeamAttacking = false;
    private bool isShielding = false;
    private bool attackCurrentlyActive = false;
    private bool shieldCurrentlyActive = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Stub for now; may need to do something here later
    }

    /* Read user input to check if they are pressing light attack button via
     * InputAction bindings - "LightAttack"
    */
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        //if button pressed, then light attacking is true
        if (context.performed)
        {
            isLightAttacking = true;
        }

        //otherwise, light attacking is false
        else if (context.canceled)
        {
            isLightAttacking = false;
        }
    }

    /* Read user input to check if they are pressing heavy attack button via
     * InputAction bindings - "HeavyAttack"
    */
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        //if button pressed, then heavy attacking is true
        if (context.performed)
        {
            isHeavyAttacking = true;
        }

        //otherwise, heavy attacking is false
        else if (context.canceled)
        {
            isHeavyAttacking = false;
        }
    }

    /* Read user input to check if they are pressing shield button via
     * InputAction bindings - "Shield"
     */
    public void OnShield(InputAction.CallbackContext context)
    {
        //if button pressed, then shielding is true
        if (context.performed)
        {
            isShielding = true;
        }

        //otherwise, shielding is false
        else if (context.canceled)
        {
            isShielding = false;
        }
    }

    /* Read user input to check if they are pressing beam attack button via
     * InputAction bindings - "BeamAttack"
     */
    public void OnBeamAttack(InputAction.CallbackContext context)
    {
        //if button pressed, then beam attacking is true
        if (context.performed)
        {
            isBeamAttacking = true;
        }

        //otherwise, beam attacking is false
        else if (context.canceled)
        {
            isBeamAttacking = false;
        }
    }

    private void LightAttackAction()
    {
        StartCoroutine(LightAttackCoroutine());
    }

   IEnumerator LightAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true, and activate the light attack hitbox
        attackCurrentlyActive = true;
        lightAttackHitbox.SetActive(true);

        //keep light attack hitbox active while elapsed time is less than attack duration
        while (timeElapsed < lightAttackDuration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently attacking to false, and deactivate light attack hitbox
        attackCurrentlyActive = false;
        lightAttackHitbox.SetActive(false);
    }

    private void HeavyAttackAction()
    {
        StartCoroutine(HeavyAttackCoroutine());
    }

    IEnumerator HeavyAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true, and activate heavy attack hitbox
        attackCurrentlyActive = true;
        heavyAttackHitbox.SetActive(true);

        //while elapsed time is less than attack duration
        while (timeElapsed < heavyAttackDuration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently attacking to false, and deactivate heavy attack hitbox
        attackCurrentlyActive = false;
        heavyAttackHitbox.SetActive(false);
    }

    private void ShieldAction()
    {
        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently shielding to true, and activate shield hitbox
        shieldCurrentlyActive = true;
        shieldHitbox.SetActive(true);

        //while elapsed time is less than shield duration
        while (timeElapsed < shieldDuration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently shielding to false, and deactivate shield hitbox
        shieldCurrentlyActive = false;
        shieldHitbox.SetActive(false);
    }

    private void BeamAttackAction()
    {
        StartCoroutine(BeamAttackCoroutine());
    }

    IEnumerator BeamAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true, and activate beam hitbox
        attackCurrentlyActive = true;
        beamAttackHitbox.SetActive(true);

        //while elapsed time is less than beam duration
        while (timeElapsed < beamAttackDuration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently attacking to false, and deactivate beam hitbox
        attackCurrentlyActive = false;
        beamAttackHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if player inputs light attack and no attack is currently active, then perform light attack action
        if(isLightAttacking && !attackCurrentlyActive) 
        {
            LightAttackAction();
        }

        //if player inputs heavy no attack is currently active, then perform heavy attack action
        if (isHeavyAttacking && !attackCurrentlyActive)
        {
            HeavyAttackAction();
        }

        //if player inputs shield button and shield is currently not active, then perform shield action
        if (isShielding && !shieldCurrentlyActive)
        {
            ShieldAction();
        }

        //if player inputs beam button and beam is currently not active, then perform beam action
        if(isBeamAttacking && !attackCurrentlyActive)
        {
            BeamAttackAction();
        }
    }
}
