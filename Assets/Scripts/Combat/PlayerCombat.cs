using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GridBrushBase;

[Serializable]
public class AttackStaminaCosts
{
    public float lightAttackCost;
    public float heavyAttackCost;
    public float shieldCost;
    public float beamAttackCost;
    public float beamAttackCostOT;
}

[Serializable]
public class AttackDamageVals
{
    public float lightAttackDmg;
    public float heavyAttackDmg;

    public float beamAttackDmg;
    public float beamAttackDmgOT;
}

[Serializable]
public class PlayerObjRefs
{
    public GameObject cameraReference;
    public GameObject statsObject;
    public GameObject lockOnCamPosObject;
}

public class PlayerCombat : MonoBehaviour
{
    public float lightAttackDuration = 1.5f;
    public float heavyAttackDuration = 2f;
    public float shieldDuration = 3.5f;
    public float beamAttackDuration = 3f;
    public float comboResetTime = 1.5f;
    public bool attackCurrentlyActive = false;
    public List<MeleeWeaponAttackScriptableObject> LightAttackStrings;
    public List<MeleeWeaponAttackScriptableObject> HeavyAttackStrings;
    /* TODO - Major Notes:
     * - Planning on reworking the current attack/combat system
     * - Want to move away from simple light/heavy attack values ON the player itself
     * - Instead, have every weapon carry its own damage, duration, animation values and unique list of combo strings
     * - For all of this, going to need a weapon class with all of the aforementioned information above
     * - Player prefab must therefore have a current weapon game object attached
     */
    public GameObject lightAttackHitbox;
    public GameObject heavyAttackHitbox;
    public GameObject shieldHitbox;
    public GameObject beamAttackHitbox;
    [SerializeField] public PlayerObjRefs playerObjRefs;
    [SerializeField] public AttackStaminaCosts atkStaminaCosts;
    [SerializeField] public AttackDamageVals atkDmgVals;

    private PlayerMovement playerMovementComponent;
    private int comboCounter;
    private float lastAttackEnd;
    private bool isLightAttacking = false;
    private bool isHeavyAttacking = false;
    private bool isBeamAttacking = false;
    private bool isShielding = false;
    private bool shieldCurrentlyActive = false;

    private OrbitCamera orbCamBehavior;
    private PlayerStats playerStats;
    private TargetingManager targetingMngr;
    private bool isLockedOn = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Get the PlayerMovement component/script that is also attached to this PlayerCapsule gameObject
        // Get references/components to every object used in script
        playerMovementComponent = gameObject.GetComponentInParent<PlayerMovement>();
        orbCamBehavior = playerObjRefs.cameraReference.GetComponent<OrbitCamera>();
        playerStats = playerObjRefs.statsObject.GetComponent<PlayerStats>();
        targetingMngr = playerObjRefs.lockOnCamPosObject.GetComponent<TargetingManager>();

        comboCounter = 0;
    }

    /* Read user input to check if they pressed the camera toggle input via
     * InputAction bindings - "CameraToggle"
    */
    public void OnCameraToggle(InputAction.CallbackContext context)
    {
        //Check if the button was pressed
        if(context.performed)
        {   
            // toggle bool
            isLockedOn = !isLockedOn;
            // set bool in orbit camera
            orbCamBehavior.currentlyLockingOn = isLockedOn;

            if (isLockedOn)
            {
                // tell lockon manager to re-calculate best target
                targetingMngr.EnableLockOn();
            }
        }
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
        //check for time elapsed greater than comvo reset time OR we have reached max combo string count
        //  TODO: Still need to decide whether light attack string will have a length separate from heavy attack string
        //  OR have a global combo string size - for now, using size of the respective attack string list
        if(Time.time - lastAttackEnd > comboResetTime || comboCounter >= LightAttackStrings.Count)
        {
            //reset combo counter
            comboCounter = 0;
        }

        StartCoroutine(LightAttackCoroutine());
    }

   IEnumerator LightAttackCoroutine()
    {

        //initialize variables
        float timeElapsed = 0f;
        MeleeWeaponAttackScriptableObject currentLightAttack = LightAttackStrings[comboCounter];

        //set currently attacking to true, and activate the light attack hitbox
        attackCurrentlyActive = true;
        lightAttackHitbox.SetActive(true);

        //rotate the player in the direction of the camera
        RotatePlayerToCameraDirection();

        //TODO: play animation at the current combo attack at the current index in the light attack list
        Debug.Log(currentLightAttack.debugMsg);

        //keep light attack hitbox active while elapsed time is less than attack duration
        // (going to need anim duration for this)
        while (timeElapsed < currentLightAttack.duration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        //increment combo counter and last attack end
        comboCounter++;
        lastAttackEnd = Time.time;

        //set currently attacking to false, and deactivate light attack hitbox
        attackCurrentlyActive = false;
        lightAttackHitbox.SetActive(false);
    }

    private void HeavyAttackAction()
    {
        //check for time elapsed greater than combo reset time OR we have reached max combo string count
        //  TODO: Still need to decide whether light attack string will have a length separate from heavy attack string
        //  OR have a global combo string size - for now, using size of the respective attack string list
        if (Time.time - lastAttackEnd > comboResetTime || comboCounter >= HeavyAttackStrings.Count)
        {
            //reset combo counter to zero
            comboCounter = 0;
        }

        StartCoroutine(HeavyAttackCoroutine());
    }

    IEnumerator HeavyAttackCoroutine()
    {
        //initialize variables
        float timeElapsed = 0f;
        MeleeWeaponAttackScriptableObject currentHeavyAttack = HeavyAttackStrings[comboCounter];

        //set currently attacking to true, and activate heavy attack hitbox
        attackCurrentlyActive = true;
        heavyAttackHitbox.SetActive(true);

        //rotate the player in the direction of the camera
        RotatePlayerToCameraDirection();

        //TODO: play animation at the current combo attack at the current index in the heavy attack list
        Debug.Log(currentHeavyAttack.debugMsg);

        //while elapsed time is less than attack duration
        while (timeElapsed < currentHeavyAttack.duration)
        {
            //update elapsed time
            timeElapsed += Time.deltaTime;

            //yield return to resume in the next frame
            yield return null;
        }

        ///increment combo counter and last attack end
        comboCounter++;
        lastAttackEnd = Time.time;

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

        //rotate the player in the direction of the camera
        RotatePlayerToCameraDirection();

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

    /* TOMAYBE: May want to change this to a Quaternion return instead; and make this function simply only
     *          get the current direction of the camera on the (x,z) plane. This then could be used
     *          anywhere else down the line where we need the camera's direction on the (x,z) plane.
     */
    private void RotatePlayerToCameraDirection()
    {
        //initialize variables
        Vector3 cameraPlaneDirection = new Vector3(playerObjRefs.cameraReference.transform.forward.x, 0,
                                              playerObjRefs.cameraReference.transform.forward.z);

        //calculate rotation direction from camera plane direction, then apply rotation to player transform
        Quaternion rotationDirection = Quaternion.LookRotation(cameraPlaneDirection, Vector3.up);
        transform.rotation = rotationDirection;
    }

    // Update is called once per frame
    void Update()
    {
        //if player inputs light attack and no attack is currently active, then perform light attack action
        if(isLightAttacking && !attackCurrentlyActive) 
        {
            if (playerStats.Stamina.TryConsume(atkStaminaCosts.lightAttackCost))
            {
                LightAttackAction();
            }
        }

        //if player inputs heavy no attack is currently active, then perform heavy attack action
        if (isHeavyAttacking && !attackCurrentlyActive)
        {
            if (playerStats.Stamina.TryConsume(atkStaminaCosts.heavyAttackCost))
            {
                HeavyAttackAction();
            }
        }

        //if player inputs shield button and shield is currently not active, then perform shield action
        if (isShielding && !shieldCurrentlyActive)
        {
            if (playerStats.Stamina.TryConsume(atkStaminaCosts.shieldCost))
            {
                ShieldAction();
            }
        }

        //if player inputs beam button and beam is currently not active, then perform beam action
        if(isBeamAttacking && !attackCurrentlyActive)
        {
            if (playerStats.Stamina.TryConsume(atkStaminaCosts.beamAttackCost))
            {
                BeamAttackAction();
            }
        }
    }
}
