using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Enemy
{
    public class EnemyKnight : MonoBehaviour
    {
        public GameObject[] controlPoints;
        private NavMeshAgent agent;
        private Animator enemyAnimator;
        private GameObject player;
        int destinyID = 0;
        bool isMoving = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            GoToPoint();
        }
        private void FixedUpdate()
        {
            enemyAnimator.SetFloat("speed", agent.velocity.sqrMagnitude);
            if(isMoving)
            {
                if ((transform.position - controlPoints[destinyID].transform.position).magnitude < 0.3f)
                {
                    StartCoroutine(StayAtPoint());
                }
            }
        }
        private void GoToPoint()
        {
            isMoving = true;
            agent.SetDestination(controlPoints[destinyID].transform.position);
        }
        IEnumerator StayAtPoint()
        {
            destinyID++;
            isMoving = false;
            yield return new WaitForSeconds(5f);
            if (destinyID > controlPoints.Length - 1)
            {
                destinyID = 0;
            }
            GoToPoint();
        }
    }
}
