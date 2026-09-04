using UnityEngine;
using System.Collections.Generic;

namespace DeccanHeat.Missions
{
    [System.Serializable]
    public class MissionDefinition
    {
        public string missionID;
        public string missionTitle;
        public string description;
        public List<MissionObjective> objectives;
        public int rewardMoney;
    }

    [System.Serializable]
    public class MissionObjective
    {
        public enum ObjectiveType { GoTo, EnterVehicle, TalkTo, Eliminate, LoseHeat }
        public ObjectiveType objectiveType;
        public string targetID;
        public Vector3 location;
        public float completionRadius = 3f;
    }

    public class MissionManager : MonoBehaviour
    {
        public MissionDefinition currentMission;
        public Player.PlayerController player;
        public Systems.HeatSystem heatSystem;

        public TextAsset[] availableMissionFiles;

        private int currentObjectiveIndex = 0;
        private bool isMissionActive = false;

        public delegate void OnMissionStateChanged(string message);
        public event OnMissionStateChanged MissionUIUpdate;

        public void StartMissionFromJson(string missionID)
        {
            foreach (TextAsset jsonFile in availableMissionFiles)
            {
                MissionDefinition def = JsonUtility.FromJson<MissionDefinition>(jsonFile.text);
                if (def.missionID == missionID)
                {
                    StartMission(def);
                    return;
                }
            }
            Debug.LogError($"Mission {missionID} not found in available files.");
        }

        public void StartMission(MissionDefinition mission)
        {
            currentMission = mission;
            currentObjectiveIndex = 0;
            isMissionActive = true;
            UpdateObjectiveUI();
            Debug.Log($"Mission Started: {mission.missionTitle}");
        }

        void Update()
        {
            if (!isMissionActive || currentMission == null || currentObjectiveIndex >= currentMission.objectives.Count) return;

            MissionObjective currentObj = currentMission.objectives[currentObjectiveIndex];
            CheckObjectiveCompletion(currentObj);
        }

        private void CheckObjectiveCompletion(MissionObjective obj)
        {
            if (player == null) return;

            switch (obj.objectiveType)
            {
                case MissionObjective.ObjectiveType.GoTo:
                    if (Vector3.Distance(player.transform.position, obj.location) <= obj.completionRadius)
                    {
                        CompleteObjective();
                    }
                    break;

                case MissionObjective.ObjectiveType.EnterVehicle:
                    if (player.isDriving && player.currentVehicle != null) // Simplified check
                    {
                        CompleteObjective();
                    }
                    break;

                case MissionObjective.ObjectiveType.LoseHeat:
                    if (heatSystem != null && heatSystem.currentHeatLevel == 0)
                    {
                        CompleteObjective();
                    }
                    break;
            }
        }

        public void CompleteObjective()
        {
            currentObjectiveIndex++;
            if (currentObjectiveIndex >= currentMission.objectives.Count)
            {
                CompleteMission();
            }
            else
            {
                UpdateObjectiveUI();
            }
        }

        private void UpdateObjectiveUI()
        {
            if (currentObjectiveIndex < currentMission.objectives.Count)
            {
                MissionObjective obj = currentMission.objectives[currentObjectiveIndex];
                MissionUIUpdate?.Invoke($"Objective: {obj.objectiveType.ToString()} {obj.targetID}");
            }
        }

        private void CompleteMission()
        {
            isMissionActive = false;
            if (player != null)
            {
                player.money += currentMission.rewardMoney;
            }
            MissionUIUpdate?.Invoke($"Mission Complete! Reward: ${currentMission.rewardMoney}");
            Debug.Log($"Mission Complete: {currentMission.missionTitle}");
            currentMission = null;
        }

        public void FailMission()
        {
            isMissionActive = false;
            MissionUIUpdate?.Invoke("Mission Failed!");
            Debug.Log("Mission Failed.");
        }
    }
}