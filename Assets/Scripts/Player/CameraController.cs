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

        public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        void LateUpdate()
        {
            if (target == null) return;

            // Simple orbital camera logic
            transform.RotateAround(target.position, Vector3.up, lookInput.x * rotationSpeed);

            // Re-calculate offset based on current rotation
            Vector3 desiredPosition = target.position + (transform.rotation * offset);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target.position + Vector3.up * 1.5f); // Look slightly above the target's origin
        }
    }
}
