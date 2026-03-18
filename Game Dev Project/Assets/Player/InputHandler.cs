using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed;
    public float mouseX;
    public float mouseY;
    public float moveAmount;

    public bool dpadInputUp;
    public bool dpadInputDown;
    public bool dpadInputLeft;
    public bool dpadInputRight;

    public bool rollSprintInput;
    public bool lightAttackInput;
    public bool heavyAttackInput;
    public bool lockonInput;
    public bool inventoryInput;
    public bool menuInput;
    public bool rollflag;
    public bool sprintFlag;
    
    public bool LockonFlag;
    public float rollInputTimer;
    

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    CameraHandler cameraHandler;
    PlayerInventory playerInventory;

    private Vector2 movementInput;
    private Vector2 cameraInput;    

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponentInChildren<PlayerInventory>();
            cameraHandler = CameraHandler.singleton;
        }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
            inputActions.PlayerActions.LockOn.performed += ctx => lockonInput = true;
            inputActions.PlayerActions.Menu.performed += i => menuInput = true;
            inputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;
        }
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void tickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleLockOnInput(delta);
        HandleQuickSlotInput();
    }

    private void MoveInput(float delta)
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
        
    }
    
    private void HandleRollInput(float delta)
    {
        rollSprintInput = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
       if (rollSprintInput)
        {
          rollInputTimer += delta;
          sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollflag = true;
                
            }
            rollInputTimer = 0;
        }
    }
    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => lightAttackInput = true;
        inputActions.PlayerActions.RT.performed += i => heavyAttackInput = true;

        if (lightAttackInput)
        {
            playerAttacker.handleLightAttack(playerInventory.rightHandWeapon);
            lightAttackInput = false;
        }
        if (heavyAttackInput)
        {
            playerAttacker.handleHeavyAttack(playerInventory.rightHandWeapon);
            heavyAttackInput = false;
        }
    }

    private void HandleLockOnInput(float delta)
    {
        if (lockonInput && !LockonFlag)
        {
           lockonInput = false;
            LockonFlag = true;
            cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
            cameraHandler.handleLockOn();
            if(cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
            }
        }else if (lockonInput && LockonFlag)
        {
            lockonInput = false;
            LockonFlag = false;
            cameraHandler.clearLockOnTargets();
        }
    }

    public void HandleInventoryInput(float delta)
    {
        if (inventoryInput)
        {
            inventoryInput = false;
            // toggle inventory UI here
        }
    }
   private void HandleQuickSlotInput()
    {
        inputActions.PlayerActions.DpadRight.performed += i => dpadInputRight = true;
        inputActions.PlayerActions.DpadLeft.performed += i => dpadInputLeft = true;

        if(dpadInputRight)
        {
            playerInventory.ChangeRightWeapon();
        }

        else if(dpadInputLeft)
        {
            playerInventory.ChangeLeftWeapon();
        }

    }

   
}

    

