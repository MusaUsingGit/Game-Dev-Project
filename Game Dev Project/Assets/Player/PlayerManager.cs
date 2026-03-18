using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputHandler inputHandler;
    Animator anim;
    public bool IsInteracting;
    
    CameraHandler cameraHandler;

    PlayerLocomotion playerLocomotion;
    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    public bool isInMenu;

      public void Awake()
    {
       
    }
    void Start()
    {
        cameraHandler = CameraHandler.singleton;
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

       private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }
    public void Update()
    {
        float delta = Time.deltaTime;

        isSprinting = inputHandler.rollSprintInput;
        // keep manager flag in sync with the animator so it doesn't get stuck
        if (anim != null)
        {
            IsInteracting = anim.GetBool("IsInteracting");
        }

        inputHandler.tickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
       
    }

    public void LateUpdate()
    {
        float delta = Time.deltaTime;
        inputHandler.rollflag = false;
        inputHandler.sprintFlag = false;
        inputHandler.lightAttackInput = false;
        inputHandler.heavyAttackInput = false;
        inputHandler.dpadInputDown = false;
        inputHandler.dpadInputUp = false;
        inputHandler.dpadInputLeft = false;
        inputHandler.dpadInputRight = false;

        if (isInAir){
            playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }
}
