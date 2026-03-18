using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    PlayerAnimationHandler animHandler;
    
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    public AudioSource footsteps;
    int footstepSoundIndex = 4;

    [Header("Ground & Air Detection Stats")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceToFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    [SerializeField]
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;
    

    [Header("Movement Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float sprintSpeed = 7;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float fallingSpeed = 45;
    
    void Start()
    {
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animHandler = GetComponentInChildren<PlayerAnimationHandler>();
        cameraObject = Camera.main.transform;
        animHandler.Initialize();
        playerManager = GetComponent<PlayerManager>();
        footsteps = GetComponentsInChildren<AudioSource>()[footstepSoundIndex];

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11 );
    }
    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        if (animHandler.canRotate == false)
            return;
        // rotate the player to face the move direction
        Vector3 targetDirection = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;
        targetDirection = cameraObject.forward * inputHandler.verticalInput;
        targetDirection += cameraObject.right * inputHandler.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;
        // if there is no input, maintain the current forward direction
        if (targetDirection == Vector3.zero)
        {
            targetDirection = myTransform.forward;
        }
        // rotate the player to face the move direction
        float rs = rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
        myTransform.rotation = targetRotation;
    }
    
    public void HandleMovement(float delta)
    {
        if(inputHandler.rollflag)
            return;
        if (playerManager.isInAir && inAirTimer > 0.5f)          // don't respond to movement while falling
            return;
        // if(playerManager.IsInteracting)
        //     return;

        moveDirection = cameraObject.forward * inputHandler.verticalInput;
        moveDirection += cameraObject.right * inputHandler.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        float speed = movementSpeed;

        if(inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if(inputHandler.moveAmount < 0.5)
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }

           moveDirection *= speed;
           playerManager.isSprinting = false;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane (moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if(inputHandler.moveAmount > 0)
        {
            if(!footsteps.isPlaying && playerManager.isSprinting == false)
            {
                footsteps.Play();
                footsteps.loop = true;
            }else if(!footsteps.isPlaying && playerManager.isSprinting == true)
            {
                footsteps.Play();
                footsteps.loop = true;
            }
        }
        else
        {
            footsteps.Stop();
            footsteps.loop = false;
        }
        

        if (animHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }
    public void HandleRollingAndSprinting(float delta)
    {
        if(animHandler.anim.GetBool("IsInteracting"))
        {
            return;
        }

        if (inputHandler.rollflag)
        {
            moveDirection = cameraObject.forward * inputHandler.verticalInput;
            moveDirection += cameraObject.right * inputHandler.horizontalInput;

            if (inputHandler.moveAmount > 0)
            {
                animHandler.PlayTargetAnimation("RollForward", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin += dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceToFall, Color.red);
        Debug.DrawRay(origin, myTransform.forward * 0.4f, Color.blue);
        //Ground Check
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;

            targetPosition.y = tp.y;

            //Landed
            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.3f)
                {
                    if(!playerManager.IsInteracting && !playerManager.isGrounded){
                    animHandler.PlayTargetAnimation("Landing", true);
                    inAirTimer = 0;
                    playerManager.isGrounded = true;
                    }
                    

                }
                else
                {
                    animHandler.PlayTargetAnimation("Empty", false);
                   playerManager.isGrounded = false;
                   playerManager.isInAir = false;
                }
                
            }
        }
        else
        {
            //Start Falling
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            //In Air for a long time
            if(!playerManager.isInAir )
            {
                if(playerManager.IsInteracting && inAirTimer > 0.2f)
                {
                    animHandler.PlayTargetAnimation("FallingLoop", true);
                }
            
            Vector3 vel = rigidbody.velocity;
            vel.Normalize();
            rigidbody.velocity = vel * (movementSpeed / 2);
            playerManager.isInAir = true;
            }
        }

        //Move the player to the target position if grounded or in the air for a short time to prevent snapping to the ground
        if(playerManager.isGrounded)
        {
           if(playerManager.IsInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Slerp(myTransform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }

        
        
    }

    #endregion
    
    }