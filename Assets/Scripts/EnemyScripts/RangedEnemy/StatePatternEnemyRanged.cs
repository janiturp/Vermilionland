using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemyRanged : MonoBehaviour
{
    #region Enemy attributes.
    [SerializeField] GameObject enemy;
    public float health;
    [SerializeField] private float maxHealth;
    public float moveSpeed;
    public float sightRange;
    public Transform eye;
    public float chaseSpeed;
    public float attackRange;
    public GameObject arm;
    public GameObject armRotator;
    public Animator animator;
    public GameObject ammoSpawn;
    public GameObject enemyBullet;
    public float bulletSpeed;

    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject shootEffect;
 
    [HideInInspector] public Rigidbody2D rb;

    // Chasetarget. Player that enters enemy's hearing range or is seen by the enemy.
    [HideInInspector] public Transform chaseTarget;
    #endregion

    #region Bullet types
    public GameObject pistolBullet;
    public GameObject shotgunBullet;
    #endregion

    #region Randomizer for patrol.
    public Transform moveSpot;

    // How long the enemy waits when it has reached a patrol spot.
    public float waitTime;

    // Randomized coordinates for moveSpot.
    [HideInInspector]
    public float minX = -5;
    [HideInInspector]
    public float maxX = 5;
    [HideInInspector]
    public float minY = -5;
    [HideInInspector]
    public float maxY = 5;
    #endregion


    #region Statemachine states.
    // Statemachine states.
    [HideInInspector] public IEnemyStateRanged currentState;
    [HideInInspector] public PatrolStateRanged patrolState;
    [HideInInspector] public ChaseStateRanged chaseState;
    [HideInInspector] public FleeStateRanged fleeState;
    [HideInInspector] public AttackStateRanged attackState;
    [HideInInspector] public AlertStateRanged alertState;
    [HideInInspector] public DieStateRanged dieState;
    #endregion

    #region Sounds
    public AudioClip enemyPainSound;
    public AudioClip enemyDamageSound;
    public AudioClip enemyDeathSound;
    public AudioClip enemyFireSound;
    public AudioClip enemyRoar;
    #endregion

    public AudioSource audioSource;

    // Awake is called as soon as Enemy object awakes. Constructs states.
    private void Awake()
    {
        patrolState = new PatrolStateRanged(this);
        chaseState = new ChaseStateRanged(this);
        fleeState = new FleeStateRanged(this);
        attackState = new AttackStateRanged(this);
        alertState = new AlertStateRanged(this);
        dieState = new DieStateRanged(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // Start making randomized spots to patrol to.
        waitTime = Random.Range(0f, 5f);
        // Start patrolling.
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.manager.soundEffectsVolume;
        currentState.UpdateState();
        // Setting so that the moveSpot is no longer child of the Enemy.
        // This is because moveSpot is used by the PatrolState to make movement targets for the enemy.
        // At the beginning the moveSpot is enemy's child so making a prefab is easier.
        // Might change it later so that the script instantiates moveSpot prefab instead.
        moveSpot.transform.parent = transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Take damage from pistol bullet.
        if (collision.collider.CompareTag("PistolBullet"))
        {
            TakeDamage(pistolBullet.GetComponent<PistolBullet>().damage);
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }

        // Take damage from shotgun bullet.
        if (collision.collider.CompareTag("ShotgunBullet"))
        {
            TakeDamage(shotgunBullet.GetComponent<ShotgunBullet>().damage);
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }

    }

    // Enemy takes damage. Flees from player when health get's low enough.
    void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            enemy.GetComponent<BoxCollider2D>().enabled = false;
            audioSource.PlayOneShot(enemyDeathSound);
            currentState = dieState;
            GameManager.manager.playerEXP += 35;
        }
        else if(health <= maxHealth * 0.25)
        {
            chaseTarget = GameObject.FindGameObjectWithTag("Player").transform;
            //currentState = fleeState;
        }
        audioSource.PlayOneShot(enemyDamageSound);
        audioSource.PlayOneShot(enemyPainSound);
    }

    public void Shoot()
    {
        audioSource.PlayOneShot(enemyFireSound);
        GameObject bulletInstance = Instantiate(enemyBullet, ammoSpawn.transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = ammoSpawn.transform.right * bulletSpeed * transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(collision);
    }
}
