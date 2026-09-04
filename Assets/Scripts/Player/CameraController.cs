using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;

namespace DeccanHeat.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // Can be Player or Vehicle
        public Vector3 offset = new Vector3(0, 3f, -5f);
        public float smoothSpeed = 10f;
        public float rotationSpeed = 5f;

        private Vector2 lookInput;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetLookInput(Vector2 input)
        {
            lookInput = input;
        }

        private float currentYRotation = 0f;
        private float currentXRotation = 15f;

        void LateUpdate()
        {
            if (target == null) return;

            currentYRotation += lookInput.x * rotationSpeed;
            currentXRotation -= lookInput.y * rotationSpeed;
            currentXRotation = Mathf.Clamp(currentXRotation, -10f, 60f); // Prevent flipping

            Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
            Vector3 desiredPosition = target.position + rotation * offset;

            // Avoid going under the ground roughly
            if (desiredPosition.y < target.position.y)
            {
                desiredPosition.y = target.position.y + 0.5f;
            }

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target.position + Vector3.up * 1.5f); // Look slightly above the target's origin
        }
    }
}
