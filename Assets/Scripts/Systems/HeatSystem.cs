using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using System.Collections.Generic;

namespace DeccanHeat.Systems
{
    public class HeatSystem : MonoBehaviour
    {
        [Range(0, 5)]
        public int currentHeatLevel = 0;

        [Header("Tuning")]
        public float searchRadiusBase = 50f;
        public float cooldownTimer = 30f;

        private float currentCooldown = 0f;
        private bool isEvading = false;

        public delegate void OnHeatChanged(int newHeatLevel);
        public event OnHeatChanged HeatChangedEvent;

        void Update()
        {
            if (isEvading && currentHeatLevel > 0)
            {
                currentCooldown -= Time.deltaTime;
                if (currentCooldown <= 0)
                {
                    LoseHeat();
                }
            }
        }

        public void AddHeat(int amount)
        {
            int oldHeat = currentHeatLevel;
            currentHeatLevel = Mathf.Clamp(currentHeatLevel + amount, 0, 5);
            isEvading = false; // Reset evasion if new crime committed

            if (oldHeat != currentHeatLevel)
            {
                UpdatePoliceResponse();
            }
        }

        public void StartEvasion()
        {
            isEvading = true;
            currentCooldown = cooldownTimer * currentHeatLevel; // Harder to lose higher heat
        }

        public void LoseHeat()
        {
            currentHeatLevel = 0;
            isEvading = false;
            UpdatePoliceResponse();
        }

        private void UpdatePoliceResponse()
        {
            HeatChangedEvent?.Invoke(currentHeatLevel);

            if (currentHeatLevel == 0)
            {
                Debug.Log("Heat Lost. Police standing down.");
                // Command police AI to return to patrol
            }
            else
            {
                float radius = searchRadiusBase * currentHeatLevel;
                Debug.Log($"Heat Level {currentHeatLevel}! Police searching within {radius}m");
                // Spawn police vehicles/pedestrians, set AI target to player
            }
        }
    }
}