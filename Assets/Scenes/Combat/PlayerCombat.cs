using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public float lightAttackDuration = 1.5f;
    public float heavyAttackDuration = 2f;
    public GameObject lightAttackHitbox;
    public GameObject heavyAttackHitbox;

    public bool isLightAttacking = false;
    public bool isHeavyAttacking = false;
    public bool isCurrentlyAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    private void LightAttackAction()
    {
        StartCoroutine(LightAttackCoroutine());
        /*
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true
        isCurrentlyAttacking = true;
        print(isLightAttacking);

        //while elapsed time is less than attack duration
        while (timeElapsed < lightAttackDuration)
        {
            //spawn a hitbox that moves forward and last for a brief amount of time
            lightAttackHitbox.SetActive(true);

            //update elapsed time
            timeElapsed += Time.deltaTime;
            print(timeElapsed);
        }

        //set currently attacking to false
        isCurrentlyAttacking = false;

        lightAttackHitbox.SetActive(false);
        */
    }

   IEnumerator LightAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true
        isCurrentlyAttacking = true;

        //while elapsed time is less than attack duration
        while (timeElapsed < lightAttackDuration)
        {
            //spawn a hitbox that moves forward and last for a brief amount of time
            lightAttackHitbox.SetActive(true);

            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently attacking to false, and deactivate light attack hitbox
        isCurrentlyAttacking = false;
        lightAttackHitbox.SetActive(false);
    }

    private void HeavyAttackAction()
    {
        StartCoroutine(HeavyAttackCoroutine());
        /*
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true
        isCurrentlyAttacking = true;

        //while elapsed time is less than attack duration
        while (timeElapsed < heavyAttackDuration)
        {
            //spawn a hitbox that moves forward and last for a brief amount of time
            heavyAttackHitbox.SetActive(true);

            //update elapsed time
            timeElapsed += Time.deltaTime;
        }

        //set currently attacking to false
        isCurrentlyAttacking = false;

        heavyAttackHitbox.SetActive(false);
        */
    }

    IEnumerator HeavyAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;

        //set currently attacking to true
        isCurrentlyAttacking = true;

        //while elapsed time is less than attack duration
        while (timeElapsed < heavyAttackDuration)
        {
            //spawn a hitbox that moves forward and last for a brief amount of time
            heavyAttackHitbox.SetActive(true);

            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //set currently attacking to false, and deactivate heavy attack hitbox
        isCurrentlyAttacking = false;
        heavyAttackHitbox.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //if player inputs light attack and no attack is currently active, then perform light attack action
        if(isLightAttacking && !isCurrentlyAttacking) 
        {
            LightAttackAction();
        }

        //if player inputs heavy no attack is currently active, then perform heavy attack action
        if (isHeavyAttacking && !isCurrentlyAttacking)
        {
            HeavyAttackAction();
        }

        //if player inputs shield button,then perform shield action

        //if player inputs beam button, then perform beam action
    }
}
