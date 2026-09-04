using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.AI;

namespace DeccanHeat.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TrafficAI : MonoBehaviour
    {
        private NavMeshAgent agent;
        public float driveSpeed = 10f;
        public float detectionDistance = 15f;

        public Transform[] waypoints;
        private int currentWaypoint = 0;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = driveSpeed;
        }

        void Update()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            // Simple Obstacle Detection
            if (Physics.Raycast(transform.position, transform.forward, detectionDistance))
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                if (!agent.pathPending && agent.remainingDistance < 2f)
                {
                    GoToNextWaypoint();
                }
            }
        }

        void GoToNextWaypoint()
        {
            agent.destination = waypoints[currentWaypoint].position;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}