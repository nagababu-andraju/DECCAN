using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;

namespace DeccanHeat.Systems
{
    [System.Serializable]
    public class SaveData
    {
        public Vector3 playerPosition;
        public int money;
        public int heatLevel;
        public string lastCompletedMission;
    }

    public class SaveLoadManager : MonoBehaviour
    {
        private const string SaveKey = "DeccanHeatSave";

        public Player.PlayerController player;
        public HeatSystem heatSystem;

        public void SaveGame()
        {
            SaveData data = new SaveData
            {
                playerPosition = player.transform.position,
                money = player.money,
                heatLevel = heatSystem.currentHeatLevel,
                lastCompletedMission = "" // Would hook into MissionManager
            };

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
            Debug.Log("Game Saved.");
        }

        public void LoadGame()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                player.transform.position = data.playerPosition;
                player.money = data.money;
                heatSystem.currentHeatLevel = data.heatLevel;
                Debug.Log("Game Loaded.");
            }
            else
            {
                Debug.LogWarning("No save file found.");
            }
        }
    }
}