using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIOnBoard : MonoBehaviour, IDamage
{
    [Header("--- Components ---")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    //[SerializeField] Animator animator;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject bullet;
    //[SerializeField] GameObject loot;
    [SerializeField] Collider collide;
    //[SerializeField] AudioSource audio;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;

    [Header("--- Enemy Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int targetFaceSpeed;
    //[SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] float shootRate;
    //[SerializeField] int experienceMin;
    //[SerializeField] int experienceMax;

    //Animations
    [SerializeField] float animChangeSpeed;

    //Variable Definitions:
    private Vector3 playerDirection;
    private bool playerInRange = true;
    private bool isShooting;
    private float stoppingDistOrig;
    private float angleToPlayer;
    private bool isDead;
    private bool destinationChosen;
    private GameObject targetLock;
    private bool isRoaming;
    //private Vector3 startingPosition;

    void Start()
    {
        stoppingDistOrig = agent.stoppingDistance;
        isDead = false;
    }
    void Update()
    {
        if (playerInRange)
        {

        }
        float agentVelocity = agent.velocity.normalized.magnitude;

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    bool canSeePlayer()
    {
        if (!isDead)
        {
            if(gamemanager.instance.player != null)
            {
                playerDirection = gamemanager.instance.player.transform.position - (transform.position - Vector3.down);
                angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
            }
            RaycastHit hit;
            if (Physics.Raycast(headPos.position, playerDirection, out hit))
            {
                if (hit.collider.CompareTag("Crew Mate"))
                {
                    agent.stoppingDistance = stoppingDistOrig;
                    agent.SetDestination(gamemanager.instance.player.transform.position);

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        faceTarget();

                        if (!isShooting && angleToPlayer <= shootAngle)
                        {
                            StartCoroutine(shoot());
                            //StartCoroutine(playAudioClip(audio));
                        }

                    }
                    return true;
                }
            }
        }
        return false;

    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //IEnumerator playAudioClip(AudioSource clip)
    //{
    //    if (clip != null)
    //    {
    //        clip.Play();
    //        yield return new WaitForSeconds(clip.clip.length);
    //    }
    //}

    public void takeDamage(int amount)
    {
        healthPoints -= amount;

        if (healthPoints <= 0)
        {
            agent.enabled = false;
            //animator.SetBool("Dead", true);
            isDead = true;
            //gamemanager.instance.updateGameGoal(-1);
            //gamemanager.instance.addExperience(UnityEngine.Random.Range(experienceMin, experienceMax));

            collide.enabled = false;

            StartCoroutine(DeathCleanup());
        }
        else
        {
            StartCoroutine(flashDamage());
            agent.SetDestination(gamemanager.instance.player.transform.position);
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetFaceSpeed);
    }
    //public void physics(Vector3 push)
    //{
    //    agent.velocity += push / 2;
    //}

    IEnumerator DeathCleanup()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    
    void OnTriggerStay(Collider other)
    {
        if(targetLock == null && other.CompareTag("Interactable"))
        {
            targetLock = other.gameObject;
        }
        
        
            
            if (other.CompareTag("Crew Mate") && canSeePlayer())
            {
            Debug.Log(1);
                isRoaming = false;
                targetLock = null;
                gamemanager.instance.player = other.gameObject;
            }
            else if (other.CompareTag("Interactable") && !canSeePlayer() && targetLock != null && agent.remainingDistance <= agent.stoppingDistance && !targetLock.GetComponent<IInteractable>().amIEnabled())
            {
            Debug.Log(2);
            isRoaming = false;
            agent.SetDestination(targetLock.transform.position);
                agent.stoppingDistance = stoppingDistOrig;

                playerDirection = targetLock.transform.position - (transform.position - Vector3.down);
                angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

                faceTarget();

                if (!isShooting && angleToPlayer <= shootAngle)
                {
                Debug.Log(3);
                StartCoroutine(shoot());
                    //StartCoroutine(playAudioClip(audio));
                }

            }
            else if (targetLock == null && !isRoaming)
            {

            Debug.Log(4);
            isRoaming = true;
            agent.stoppingDistance = 0;
                StartCoroutine(roam());
            }
            
        if (targetLock != null && targetLock.GetComponent<IInteractable>().amIEnabled())
        {
            Debug.Log(5);
            targetLock = null;
        }

    }
    IEnumerator roam()
    {

        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);
            //isRoaming = false;

            Vector3 randomPosition = Random.insideUnitSphere * roamDistance;
            //randomPosition += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;

        }
    }

}
