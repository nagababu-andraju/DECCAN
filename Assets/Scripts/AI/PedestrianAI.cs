using UnityEngine;
using UnityEngine.AI;

namespace DeccanHeat.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PedestrianAI : MonoBehaviour
    {
        public int health = 100;
        private NavMeshAgent agent;
        public float wanderRadius = 30f;

        private enum State { Idle, Wander, Flee, Dead }
        private State currentState;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            currentState = State.Idle;
            InvokeRepeating("EvaluateState", 1f, 3f);
        }

        void EvaluateState()
        {
            if (currentState == State.Dead || currentState == State.Flee) return;

            if (Random.value > 0.5f)
            {
                currentState = State.Wander;
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
            }
            else
            {
                currentState = State.Idle;
                agent.ResetPath();
            }
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                Flee();
            }
        }

        private void Flee()
        {
            currentState = State.Flee;
            agent.speed *= 2f;
            Vector3 fleeDirection = RandomNavSphere(transform.position, wanderRadius * 2, -1);
            agent.SetDestination(fleeDirection);
        }

        private void Die()
        {
            currentState = State.Dead;
            agent.enabled = false;
            // Trigger ragdoll or death animation
            Debug.Log("Pedestrian Died.");
            Destroy(gameObject, 5f); // Cleanup
        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
            return navHit.position;
        }
    }
}