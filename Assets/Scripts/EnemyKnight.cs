using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Enemy
{
    public class EnemyKnight : MonoBehaviour
    {
        public GameObject[] controlPoints;
        public float chaseDistance;
        public float attackDistance;

        private float currentDistance;

        private NavMeshAgent agent;
        private Animator enemyAnimator;
        private GameObject player;
        private EnemyState currentState;
        bool canAttack = true;

        void Start()
        {
            controlPoints = GameObject.FindGameObjectsWithTag("controlPoint");
            canAttack = true;
            agent = GetComponent<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            currentState = EnemyState.PATROL;
            ExecutePatrol();
        }
        private void Update()
        {
            currentDistance = (player.transform.position - transform.position).magnitude;
            switch (currentState)
            {
                case EnemyState.PATROL:
                    if (currentDistance <= chaseDistance)
                    {
                        currentState = EnemyState.CHASE;
                        ExecuteChase();
                    }
                    enemyAnimator.SetFloat("speed", agent.velocity.sqrMagnitude);
                    break;
                case EnemyState.CHASE:
                    enemyAnimator.SetFloat("speed", agent.velocity.sqrMagnitude);
                    agent.SetDestination(player.transform.position);
                    if (agent.remainingDistance <= attackDistance)
                    {
                        currentState = EnemyState.ATTACK;
                    }
                    else if (agent.remainingDistance > chaseDistance)
                    {
                        currentState = EnemyState.PATROL;
                        ExecutePatrol();
                    }
                    break;
                case EnemyState.ATTACK:
                    if (canAttack)
                    {
                        DoAttack();
                    }
                    if (currentDistance > attackDistance)
                    {
                        currentState = EnemyState.CHASE;
                        ExecuteChase();
                    }
                    break;
                case EnemyState.DEATH:
                    enemyAnimator.SetFloat("speed", 0);
                    enemyAnimator.SetBool("Death", true);
                    agent.enabled = false;
                    break;
            }
        }

        public void ApplyDestination()
        {
            agent.SetDestination(controlPoints[Random.Range(0, controlPoints.Length)].transform.position);
        }
        private void ExecutePatrol()
        {
            InvokeRepeating("ApplyDestination", 0f, 4f);
        }
        private void ExecuteChase()
        {
            CancelInvoke();
            agent.SetDestination(player.transform.position);
        }
        void DoAttack()
        {
            enemyAnimator.SetTrigger("PlayerAttack");
            canAttack = false;
            player.SendMessage("ReciveDamage");
            StartCoroutine(Cooldown());
        }
        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(3f);
            canAttack = true;
        }
        void ReciveDamage()
        {
            currentState = EnemyState.DEATH;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(Despawn());
        }
        IEnumerator Despawn()
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }
    }
    public enum EnemyState { PATROL, CHASE, ATTACK, DEATH };
}
