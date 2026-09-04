using UnityEngine;
using UnityEngine.InputSystem;

namespace DeccanHeat.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public int meleeDamage = 20;
        public int rangedDamage = 40;
        public float meleeRange = 2f;
        public float rangedRange = 50f;

        public Transform firePoint;
        public ParticleSystem muzzleFlash;

        // Assumes this is on the Player
        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Simple Raycast Ranged Combat
                FireWeapon();
            }
        }

        public void OnMelee(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformMelee();
            }
        }

        private void FireWeapon()
        {
            if (muzzleFlash != null) muzzleFlash.Play();

            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, rangedRange))
            {
                // Attempt to apply damage
                var target = hit.collider.GetComponent<AI.PedestrianAI>();
                if (target != null)
                {
                    target.TakeDamage(rangedDamage);
                    NotifyHeatSystem();
                }
            }
        }

        private void PerformMelee()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, meleeRange);
            foreach(var hit in hits)
            {
                var target = hit.GetComponent<AI.PedestrianAI>();
                if (target != null && target.gameObject != this.gameObject)
                {
                    target.TakeDamage(meleeDamage);
                    NotifyHeatSystem();
                }
            }
        }

        private void NotifyHeatSystem()
        {
            var heat = FindObjectOfType<Systems.HeatSystem>();
            if (heat != null)
            {
                heat.AddHeat(1); // Escalate heat upon violence
            }
        }
    }
}