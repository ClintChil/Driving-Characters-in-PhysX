using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public float force = 25;
    public float facingForce = 50;
    protected CharacterInput input;
    protected Vector3 currentFacing = Vector3.zero;
    //
    public Rigidbody chestBody;
    protected Vector3 inputDirection = Vector3.zero;
    public CharacterLegs legs;
    public CharacterUpright chestUpright;
    public CharacterMaintainHeight maintainHeight;
    public CharacterMaintainHeight[] otherMaintainHeight;
    public CharacterUpright[] otherUprights;
    public CharacterFaceDirection faceDirection;
    public Rigidbody[] feetBodies;
    public float maintainHeightStanding = 1;
    public float maintainHeightCrouching = 0.6f;
    //
    //
    protected float jumpCounter = 0;
    public float jumpDelay = 0.4f;
    public float airTimeDelay = 2;
    protected bool jumpAnticipation = false;
    protected bool inAir = false;
    public float jumpForce = 100;
    public float jumpForwardForce = 50;
    public float facePlantForce = 250;
    protected float facePlantM = 1;
    protected float getUpCounter = 0;
    //
    void Start()
    {
        input = GetComponent<CharacterInput>();
    }
    //
    void Update()
    {
        //
        if (input.PressFire())
        {
            StartJumpAnticipation();
        }
        if (getUpCounter > 0)
        {
            getUpCounter -= Time.deltaTime;
            //
            // *****************  LIFT ARMS OFF OF THE GROUND SLOWLY WHEN GETTING UP ************
            //
            foreach (CharacterMaintainHeight h in otherMaintainHeight)
            {
                h.desiredHeight = Mathf.Lerp(h.desiredHeight, 0.2f, Time.deltaTime * 3);
            }
        }
        if (jumpAnticipation)
        {
            //***********************************  CROUCHING BEFORE JUMP **********************
            //
            jumpCounter += Time.deltaTime;
            if (jumpCounter >= jumpDelay)
            {
                Jump();
            }
        }
        else if (inAir)
        {
            //***********************************  AIR BORNE **********************
            //
            jumpCounter += Time.deltaTime;
            if (jumpCounter >= airTimeDelay)
            {
                GetUpFromJump();
            }
            //
        }
        else
        {
            //***********************************  STANDING ON GROUND **********************
            //           
            inputDirection = Vector3.zero;
            if (input.HoldRight())
            {
                inputDirection += Vector3.right;
            }
            if (input.HoldLeft())
            {
                inputDirection += Vector3.left;
            }
            if (input.HoldUp())
            {
                inputDirection += Vector3.forward;
            }
            if (input.HoldDown())
            {
                inputDirection += Vector3.back;
            }
            if (inputDirection != Vector3.zero)
            {
                // *** MOVE BASED ON INPUT DIRECTION ****
                //
                inputDirection.Normalize();
                //               
                currentFacing = chestBody.transform.forward;
                currentFacing.y = 0;
                currentFacing.Normalize();
                //
                if (!legs.walking)
                {
                    legs.StartWalking();
                }
                //
                faceDirection.facingDirection = inputDirection;
                //
            }
            else
            {
                // *** STAND STILL WHEN ZERO INPUT ****
                //
                faceDirection.facingDirection = currentFacing;
                //
                if (legs.walking)
                {
                    legs.StopWalking();
                }
            }
        }
    }

    private void GetUpFromJump()
    {
        // ***********************  STAND UP AFTER BEING A RAGDOLL *******
        //
        foreach (CharacterMaintainHeight h in otherMaintainHeight)
        {
            h.enabled = true;
            h.desiredHeight = -3; // **** START ARMS ON GROUND AND THEN LERP THIS VALUE TO NORMAL HEIGHT ****
        }
        foreach (CharacterUpright h in otherUprights)
        {
            h.enabled = true;
        }
        //
        getUpCounter = 3; // ** JUST USED TO SETTLE THE ARMS ***
        //
        //
        // **** NEXT: REACTIVATE ALL THE OTHER COMPONENTS THAT MOVE THE LIMBS AND TORSO ****
        //
        inAir = false;
        maintainHeight.enabled = true;
        maintainHeight.desiredHeight = maintainHeightStanding;
        faceDirection.enabled = true;
        legs.enabled = true;
        chestUpright.enabled = true;
        //
        // *** DO A SMALL HOP UPWARD TO START GETTING UP ***
        //
        chestBody.AddForceAtPosition((chestBody.transform.forward * -1 + Vector3.up) * 20, chestBody.transform.TransformPoint(Vector3.up * 0.2f), ForceMode.Impulse);
    }
    private void Jump()
    {
        // ***********************  ACTUALLY JUMP - Launch into the air *******
        //
        //
        // **** DISABLE SOME CONTROLLING COMPONENTS (the height maintaining script on the torso and upright forces on feet) ****
        //
        foreach (CharacterMaintainHeight h in otherMaintainHeight)
        {
            h.enabled = false;
        }
        foreach (CharacterUpright h in otherUprights)
        {
            h.enabled = false;
        }
        // **** LAUNCH INTO AIR HERE :
        //
        chestBody.AddForce(Vector3.up * jumpForce + chestBody.transform.forward * jumpForwardForce, ForceMode.Impulse);
        //
        // **** NEXT: DISABLE ALL THE OTHER CONTROLLING COMPONENTS AND ESSENTIALLY BECOME A RAGDOLL ****
        //
        maintainHeight.enabled = false;
        jumpCounter = 0;
        jumpAnticipation = false;
        inAir = true;
        legs.enabled = false;
        chestUpright.enabled = false;
        faceDirection.enabled = false;
        //
        // ****  SOMETIMES THE FACEPLANT IS GOING TO HAVE MORE FORCE ON IT, BECAUSE RANDOM STRENGTH FACEPLANTS ARE COOL ***
        //
        facePlantM = 0.9f + Random.value * 0.4f;
    }
    //
    private void StartJumpAnticipation()
    {
        // ***********************  CROUCH A BIT UNTIL THE ACTUAL JUMP *******
        legs.StopWalking();
        jumpAnticipation = true;
        maintainHeight.desiredHeight = maintainHeightCrouching;
        jumpCounter = 0;
    }
    //
    void FixedUpdate()
    {
        // *************  I FIND APPLYING FORCES IN FIXED UPDATE TO BE MORE RELIABLE THAN IN UPDATE ****
        //
        if (!inAir)
        {
            // ****  APPLY DRAGS ****
            //
            ApplyStandingAndWalkingDrag();
            //
            if (!jumpAnticipation)
            {
                if (inputDirection != Vector3.zero)
                {
                    // *********************  MOVE CHEST IN THE INPUT DIRECTION *******
                    //
                    // *** (THIS IS ZERO IN THE PROJECT BY DEFAULT, I PREFER HAVING THE LEGS PULL THE BODY FORWARD ***
                    //
                    chestBody.AddForceAtPosition(force * inputDirection * Time.deltaTime, chestBody.transform.TransformDirection(Vector3.forward * 2), ForceMode.Impulse);
                    //                   
                    //                    
                }
            }

        }
        else if (inAir)
        {
            //
            // *******************************************  TOWARDS END OF JUMP, FORCE A FACEPLANT *****
            //
            if (jumpCounter > airTimeDelay * 0.15f && jumpCounter < airTimeDelay * 0.4f)
            {
                chestBody.AddForceAtPosition((chestBody.transform.forward + Vector3.down) * facePlantForce * facePlantM * Time.deltaTime, chestBody.transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);
                //
                foreach (Rigidbody f in feetBodies)
                {
                    f.AddForce(Vector3.up * 5 * Time.deltaTime, ForceMode.Impulse);
                }
            }
        }
    }

    private void ApplyStandingAndWalkingDrag()
    {
        // ***********  APPLY DRAGS! **
        //
        // THIS, along with the powerful facing direction forces, ACTUALLY MAKES THE CHARACTERS LESS INTERACTIBLE, BECAUSE THEY CAN'T PUSH EACH OTHER MUCH *****
        // SOFTER FORCES CAN BE BETTER, BUT THOSE NEED MORE TWEEKING, IDEALLY JUST ENOUGH FORCE TO ACHIEVE THE EFFECT WITHOUT BECOMING LOCKED INTO THAT POSITION OR DIRECTION ***
        //
        if (inputDirection == Vector3.zero)
        {
            // ***** WHEN STANDING STILL, APPLY A DRAG BASED ON HOW FAST THE TORSO IS TRAVELLING ***
            //
            Vector3 horizontalVelocity = chestBody.velocity;
            horizontalVelocity.y = 0;
            //
            float speed = horizontalVelocity.magnitude;
            //
            chestBody.velocity *= (1 - Mathf.Clamp(speed * 20f + 10, 0, 50) * Time.fixedDeltaTime);
        }
        else
        {
            // ***** APPLY A POWERFUL DRAG FORCE IF THE TORSO ISN'T TRAVELLING IN THE INPUT DIRECITON, ALLOWS FOR TIGHT TURNS ***
            //
            Vector3 horizontalVelocity = chestBody.velocity;
            horizontalVelocity.y = 0;
            //
            float m = 1 - (1 + Vector3.Dot(horizontalVelocity.normalized, inputDirection)) / 2f;
            chestBody.velocity *= (1 - (m * 30) * Time.fixedDeltaTime);
        }
        //
    }
}
