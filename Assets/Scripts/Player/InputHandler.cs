using UnityEngine;
using UnityEngine.InputSystem;

namespace DeccanHeat.Player
{
    public class InputHandler : MonoBehaviour
    {
        public PlayerController playerController;
        public CameraSystem.CameraController cameraController;
        public UI.UIManager uiManager;
        public Combat.CombatSystem combatSystem;

        private InputActionMap actionMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction interactAction;
        private InputAction fireAction;
        private InputAction brakeAction;
        private InputAction pauseAction;

        void Awake()
        {
            actionMap = new InputActionMap("DeccanHeatControls");

            moveAction = actionMap.AddAction("Move", type: InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            lookAction = actionMap.AddAction("Look", type: InputActionType.Value);
            lookAction.AddBinding("<Mouse>/delta");

            jumpAction = actionMap.AddAction("Jump", type: InputActionType.Button);
            jumpAction.AddBinding("<Keyboard>/space");

            interactAction = actionMap.AddAction("Interact", type: InputActionType.Button);
            interactAction.AddBinding("<Keyboard>/e");

            fireAction = actionMap.AddAction("Fire", type: InputActionType.Button);
            fireAction.AddBinding("<Mouse>/leftButton");

            brakeAction = actionMap.AddAction("Brake", type: InputActionType.Button);
            brakeAction.AddBinding("<Keyboard>/space");

            pauseAction = actionMap.AddAction("Pause", type: InputActionType.Button);
            pauseAction.AddBinding("<Keyboard>/escape");
        }

        void OnEnable()
        {
            actionMap.Enable();

            jumpAction.performed += OnJump;
            interactAction.performed += OnInteract;
            fireAction.performed += OnFire;
            pauseAction.performed += OnPause;
        }

        void OnDisable()
        {
            actionMap.Disable();

            jumpAction.performed -= OnJump;
            interactAction.performed -= OnInteract;
            fireAction.performed -= OnFire;
            pauseAction.performed -= OnPause;
        }

        void Update()
        {
            if (playerController == null) return;

            // Route Camera Input
            if (cameraController != null)
            {
                var lookContext = new InputAction.CallbackContext(); // Simulate context
                // Manual hack since we can't easily fake context values, we'll assign direct
            }

            // Route Move Input
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            bool brakeValue = brakeAction.IsPressed();

            if (playerController.isDriving && playerController.currentVehicle != null)
            {
                // We'll pass raw values to VehicleController directly to avoid fake contexts
                playerController.currentVehicle.SetInput(moveValue, brakeValue);
            }
            else
            {
                playerController.SetMovementInput(moveValue);
            }

            if (cameraController != null)
            {
                cameraController.SetLookInput(lookAction.ReadValue<Vector2>());
            }
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (playerController != null && !playerController.isDriving) playerController.DoJump();
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (playerController != null) playerController.DoInteract();
        }

        private void OnFire(InputAction.CallbackContext ctx)
        {
            if (combatSystem != null && playerController != null && !playerController.isDriving)
            {
                combatSystem.FireWeapon();
            }
        }

        private void OnPause(InputAction.CallbackContext ctx)
        {
            if (uiManager != null) uiManager.TogglePause();
        }
    }
}