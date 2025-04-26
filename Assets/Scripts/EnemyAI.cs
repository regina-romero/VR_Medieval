using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private float walkPointRange = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;

    private int _currentHealth;
    private int _maxHealth = 100;
    private LayerMask whatIsGround;
    bool isSearching = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        whatIsGround = LayerMask.GetMask("Ground");

        _currentHealth = _maxHealth;

        player = GameObject.FindGameObjectWithTag("Origin").transform;
        SetRandomWalkPoint();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange && !walkPointSet)
        {
            ChasePlayer();
            isSearching = false;
            animator.SetBool("isSearching", isSearching);
        }
        else if (!walkPointSet)
        {
            Wander();
        }

        if (Vector3.Distance(transform.position, player.position) < attackRange && !alreadyAttacked)
        {
            alreadyAttacked = true;
            FireBullet();
            Invoke(nameof(ResetAttack), 1f);
        }
    }

    void Wander()
    {
        if (Vector3.Distance(transform.position, walkPoint) < 1f)
        {
            SetRandomWalkPoint();
        }

        agent.SetDestination(walkPoint);
    }

    void SetRandomWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

        isSearching = true;
        animator.SetBool("isSearching", isSearching);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        isSearching = false;
        animator.SetBool("isSearching", isSearching);
    }

    void FireBullet()
    {
        GameObject spawnedBullet = Instantiate(projectile);
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.position = spawnPoint.position;
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        Destroy(spawnedBullet, 5);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            TakeDamage(5);
        }

        if (collision.gameObject.CompareTag("ray"))
        {
            TakeDamage(1);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) DestroyEnemy();
    }

    void DestroyEnemy()
    {
        gameObject.SetActive(true);
        StartCoroutine(WaitBeforeDestroy());
    }

    private IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
