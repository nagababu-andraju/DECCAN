using UnityEngine;

namespace DeccanHeat.Systems
{
    public class DayNightCycle : MonoBehaviour
    {
        public Light directionalLight;
        public float dayDurationInSeconds = 120f;

        [Range(0, 1)]
        public float currentTimeOfDay = 0.5f;

        void Update()
        {
            currentTimeOfDay += (Time.deltaTime / dayDurationInSeconds);
            if (currentTimeOfDay >= 1)
            {
                currentTimeOfDay = 0;
            }

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            float sunRotation = (currentTimeOfDay * 360f) - 90f;
            directionalLight.transform.localRotation = Quaternion.Euler(sunRotation, 170f, 0f);

            // Nighttime detection
            if (currentTimeOfDay <= 0.25f || currentTimeOfDay >= 0.75f)
            {
                directionalLight.intensity = 0.1f;
            }
            else
            {
                directionalLight.intensity = 1.0f;
            }
        }
    }
}