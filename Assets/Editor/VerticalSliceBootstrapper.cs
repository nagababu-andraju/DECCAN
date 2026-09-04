using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace DeccanHeat.Editor
{
    public class VerticalSliceBootstrapper : EditorWindow
    {
        [MenuItem("Deccan Heat/Bootstrap Vertical Slice")]
        public static void BootstrapScene()
        {
            Debug.Log("Starting Deccan Heat Vertical Slice Bootstrap...");

            // 1. Create a new Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Main_VerticalSlice";

            // 2. Setup Lighting and Camera
            GameObject dirLight = new GameObject("Directional Light");
            Light lightComp = dirLight.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);

            GameObject mainCam = new GameObject("Main Camera");
            mainCam.tag = "MainCamera";
            mainCam.AddComponent<Camera>();
            mainCam.AddComponent<AudioListener>();
            var camController = mainCam.AddComponent<Player.CameraController>();

            // 3. Setup World/District Primitives
            GameObject districtRoot = new GameObject("Environment_District");

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.transform.SetParent(districtRoot.transform);
            ground.transform.localScale = new Vector3(50, 1, 50);

            // Create some primitive "buildings"
            for(int i = 0; i < 10; i++)
            {
                GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                building.transform.SetParent(districtRoot.transform);
                float height = Random.Range(10f, 30f);
                building.transform.localScale = new Vector3(Random.Range(10f, 20f), height, Random.Range(10f, 20f));
                building.transform.position = new Vector3(Random.Range(-200f, 200f), height/2f, Random.Range(-200f, 200f));
            }

            // 4. Setup Player
            GameObject playerGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerGo.name = "Player_Protagonist";
            playerGo.transform.position = new Vector3(0, 1, 0);
            var playerController = playerGo.AddComponent<Player.PlayerController>();
            playerController.cameraTransform = mainCam.transform;
            camController.SetTarget(playerGo.transform);

            // 5. Setup Vehicle (Auto-Rickshaw placeholder)
            GameObject vehicleGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vehicleGo.name = "AutoRickshaw_01";
            vehicleGo.transform.position = new Vector3(10, 1, 10);
            vehicleGo.transform.localScale = new Vector3(2, 2, 4);
            var rb = vehicleGo.AddComponent<Rigidbody>();
            rb.mass = 1500f;
            var vehicleController = vehicleGo.AddComponent<Vehicles.VehicleController>();

            // Generate dummy wheel colliders
            for(int i=0; i<4; i++) {
                GameObject wheel = new GameObject($"Wheel_{i}");
                wheel.transform.SetParent(vehicleGo.transform);
                wheel.AddComponent<WheelCollider>();
            }

            // 6. Setup Systems
            GameObject managersRoot = new GameObject("Game_Managers");
            managersRoot.AddComponent<Systems.HeatSystem>();
            var missionManager = managersRoot.AddComponent<Missions.MissionManager>();
            missionManager.player = playerController;
            missionManager.heatSystem = managersRoot.GetComponent<Systems.HeatSystem>();

            Debug.Log("Scene Bootstrapped. Please save the scene.");
        }
    }
}
