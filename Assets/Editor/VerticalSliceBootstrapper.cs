
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
            var camController = mainCam.AddComponent<CameraSystem.CameraController>();

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

            // Generate dummy wheel colliders and assign to VehicleController
            Transform[] wheelTransforms = new Transform[4];
            WheelCollider[] wheelColliders = new WheelCollider[4];

            string[] wheelNames = { "FL", "FR", "RL", "RR" };
            Vector3[] wheelPos = {
                new Vector3(-1f, -0.5f, 1.5f), new Vector3(1f, -0.5f, 1.5f),
                new Vector3(-1f, -0.5f, -1.5f), new Vector3(1f, -0.5f, -1.5f)
            };

            for(int i=0; i<4; i++) {
                GameObject wheel = new GameObject($"Wheel_{wheelNames[i]}");
                wheel.transform.SetParent(vehicleGo.transform);
                wheel.transform.localPosition = wheelPos[i];
                wheelTransforms[i] = wheel.transform;
                wheelColliders[i] = wheel.AddComponent<WheelCollider>();
            }

            vehicleController.frontLeftWheel = wheelColliders[0];
            vehicleController.frontRightWheel = wheelColliders[1];
            vehicleController.rearLeftWheel = wheelColliders[2];
            vehicleController.rearRightWheel = wheelColliders[3];

            vehicleController.frontLeftTransform = wheelTransforms[0];
            vehicleController.frontRightTransform = wheelTransforms[1];
            vehicleController.rearLeftTransform = wheelTransforms[2];
            vehicleController.rearRightTransform = wheelTransforms[3];

            // 6. Setup Systems
            GameObject managersRoot = new GameObject("Game_Managers");
            managersRoot.AddComponent<Systems.HeatSystem>();
            managersRoot.AddComponent<Systems.DayNightCycle>().directionalLight = lightComp;
            managersRoot.AddComponent<Systems.SaveLoadManager>();
            var missionManager = managersRoot.AddComponent<Missions.MissionManager>();
            missionManager.player = playerController;
            missionManager.heatSystem = managersRoot.GetComponent<Systems.HeatSystem>();

            // Load json assets into mission manager array for the bootstrapper
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] {"Assets/Data/Missions"});
            missionManager.availableMissionFiles = new TextAsset[guids.Length];
            for(int i=0; i<guids.Length; i++) {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                missionManager.availableMissionFiles[i] = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            }

            // 7. Setup Mission Targets (Empty GameObjects for JSON targets)
            GameObject targetsRoot = new GameObject("Mission_Targets");
            string[] targetNames = { "Safehouse_Marker", "Bazaar_Market", "ChopShop_Garage", "Police_Station_Exterior", "Dark_Alley", "Drop_Location", "Informant_NPC" };
            Vector3[] targetPositions = {
                new Vector3(10, 0, 15), new Vector3(150, 0, -40), new Vector3(-200, 0, 80),
                new Vector3(50, 0, 200), new Vector3(-80, 0, -120), new Vector3(300, 0, 300), new Vector3(20, 0, 45)
            };
            for (int i=0; i<targetNames.Length; i++) {
                GameObject t = new GameObject(targetNames[i]);
                t.transform.SetParent(targetsRoot.transform);
                t.transform.position = targetPositions[i];
            }

            // 8. Setup NPCs and Traffic
            GameObject aiRoot = new GameObject("AI_Actors");
            for (int i=0; i<5; i++) {
                GameObject ped = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                ped.name = $"Pedestrian_{i}";
                ped.transform.SetParent(aiRoot.transform);
                ped.transform.position = new Vector3(Random.Range(-50, 50), 1, Random.Range(-50, 50));
                ped.AddComponent<UnityEngine.AI.NavMeshAgent>();
                ped.AddComponent<AI.PedestrianAI>();
            }

            // 9. Setup UI & Programmatic Input Manager
            GameObject uiRoot = new GameObject("UI_Root");
            var canvas = uiRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject hudPanel = new GameObject("HUD_Panel");
            hudPanel.transform.SetParent(uiRoot.transform);
            GameObject pausePanel = new GameObject("Pause_Panel");
            pausePanel.transform.SetParent(uiRoot.transform);
            pausePanel.SetActive(false); // Hide by default

            var uiManager = uiRoot.AddComponent<UI.UIManager>();
            uiManager.hudPanel = hudPanel;
            uiManager.pauseMenuPanel = pausePanel;

            // Attach programmatic InputHandler instead of PlayerInput component
            var inputHandler = managersRoot.AddComponent<Player.InputHandler>();
            inputHandler.playerController = playerController;
            inputHandler.cameraController = camController;
            inputHandler.uiManager = uiManager;

            var combatSystem = playerGo.AddComponent<Combat.CombatSystem>();
            combatSystem.firePoint = playerGo.transform; // dummy transform
            inputHandler.combatSystem = combatSystem;

            // 10. Add scene to Build Settings
            var originalScenes = EditorBuildSettings.scenes;
            var newScenes = new EditorBuildSettingsScene[originalScenes.Length + 1];
            System.Array.Copy(originalScenes, newScenes, originalScenes.Length);
            newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene("Assets/Scenes/Main_VerticalSlice.unity", true);
            EditorBuildSettings.scenes = newScenes;

            // 9. Save Scene
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/Main_VerticalSlice.unity");
            Debug.Log("Scene Bootstrapped and Saved to Assets/Scenes/Main_VerticalSlice.unity.");
        }
    }
}
