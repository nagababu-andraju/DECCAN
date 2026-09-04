using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;

namespace DeccanHeat.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        public bool isOccupied = false;
        public float currentHealth = 100f;
        public Transform exitPoint;

        [Header("Physics & Handling")]
        public float motorForce = 1500f;
        public float breakForce = 3000f;
        public float maxSteerAngle = 30f;

        [Header("Wheel Colliders")]
        public WheelCollider frontLeftWheel;
        public WheelCollider frontRightWheel;
        public WheelCollider rearLeftWheel;
        public WheelCollider rearRightWheel;

        [Header("Wheel Transforms")]
        public Transform frontLeftTransform;
        public Transform frontRightTransform;
        public Transform rearLeftTransform;
        public Transform rearRightTransform;

        private Rigidbody rb;
        private float horizontalInput;
        private float verticalInput;
        private bool isBreaking;

        private Player.PlayerController driver;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0, -0.5f, 0); // Lower center of mass for stability
        }

        public void EnterVehicle(Player.PlayerController player)
        {
            isOccupied = true;
            driver = player;
            rb.isKinematic = false; // Ensure physics is active
        }

        public void ExitVehicle()
        {
            isOccupied = false;
            driver = null;
            horizontalInput = 0;
            verticalInput = 0;
            isBreaking = true;
            ApplyBrakes();
        }

        public void SetInput(Vector2 move, bool brake)
        {
            if (!isOccupied) return;
            horizontalInput = move.x;
            verticalInput = move.y;
            isBreaking = brake;
        }

        private void FixedUpdate()
        {
            if (!isOccupied && !isBreaking) return; // Allow settling

            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        private void HandleMotor()
        {
            if (isBreaking)
            {
                ApplyBrakes();
                return;
            }

            frontLeftWheel.brakeTorque = 0;
            frontRightWheel.brakeTorque = 0;
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;

            float torque = verticalInput * motorForce;
            rearLeftWheel.motorTorque = torque;
            rearRightWheel.motorTorque = torque;
        }

        private void ApplyBrakes()
        {
            frontLeftWheel.brakeTorque = breakForce;
            frontRightWheel.brakeTorque = breakForce;
            rearLeftWheel.brakeTorque = breakForce;
            rearRightWheel.brakeTorque = breakForce;

            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
        }

        private void HandleSteering()
        {
            float steerAngle = maxSteerAngle * horizontalInput;
            frontLeftWheel.steerAngle = steerAngle;
            frontRightWheel.steerAngle = steerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheel, frontLeftTransform);
            UpdateSingleWheel(frontRightWheel, frontRightTransform);
            UpdateSingleWheel(rearLeftWheel, rearLeftTransform);
            UpdateSingleWheel(rearRightWheel, rearRightTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            if (wheelTransform == null) return;
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.position = pos;
            wheelTransform.rotation = rot;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Debug.Log("Vehicle Exploded!");
                if (isOccupied && driver != null)
                {
                    driver.TakeDamage(100); // Kill player on explosion
                }
                gameObject.SetActive(false); // Destroy/Pool
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Simple crash damage
            if (collision.relativeVelocity.magnitude > 10f)
            {
                TakeDamage(collision.relativeVelocity.magnitude);
            }
        }
    }
}
