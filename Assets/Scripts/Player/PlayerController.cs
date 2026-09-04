using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;

namespace DeccanHeat.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 3f;
        public float runSpeed = 6f;
        public float gravity = -9.81f;
        public float jumpHeight = 1.5f;

        [Header("Stats")]
        public int health = 100;
        public int money = 0;

        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private Vector2 movementInput;

        [Header("Vehicle State")]
        public bool isDriving = false;
        public Vehicles.VehicleController currentVehicle;

        [Header("Camera")]
        public Transform cameraTransform;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        // Raw Input Methods for InputHandler routing
        public void SetMovementInput(Vector2 input)
        {
            movementInput = input;
        }

        public void DoJump()
        {
            if (isGrounded && !isDriving)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        public void DoInteract()
        {
            if (isDriving)
            {
                ExitVehicle();
            }
            else
            {
                TryEnterVehicle();
            }
        }

        void Update()
        {
            if (isDriving) return;

            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Keep grounded
            }

            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);

            // Adjust movement relative to camera orientation
            if (cameraTransform != null)
            {
                Vector3 camForward = cameraTransform.forward;
                camForward.y = 0;
                camForward.Normalize();

                Vector3 camRight = cameraTransform.right;
                camRight.y = 0;
                camRight.Normalize();

                move = (camForward * move.z) + (camRight * move.x);
            }

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            controller.Move(move * walkSpeed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void TryEnterVehicle()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
            foreach (var hitCollider in hitColliders)
            {
                Vehicles.VehicleController vehicle = hitCollider.GetComponentInParent<Vehicles.VehicleController>();
                if (vehicle != null && !vehicle.isOccupied)
                {
                    currentVehicle = vehicle;
                    isDriving = true;
                    vehicle.EnterVehicle(this);

                    // Disable player physical presence
                    controller.enabled = false;
                    transform.SetParent(vehicle.transform);
                    transform.localPosition = Vector3.zero;

                    // Typically mesh renderers are disabled here too
                    return;
                }
            }
        }

        private void ExitVehicle()
        {
            if (currentVehicle != null)
            {
                transform.SetParent(null);
                transform.position = currentVehicle.exitPoint != null ? currentVehicle.exitPoint.position : currentVehicle.transform.position + Vector3.right * 2f;
                controller.enabled = true;

                currentVehicle.ExitVehicle();
                currentVehicle = null;
                isDriving = false;
            }
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player Died. Implement Restart/Checkpoint logic here.");
            // Notify game manager to reload checkpoint
        }
    }
}
