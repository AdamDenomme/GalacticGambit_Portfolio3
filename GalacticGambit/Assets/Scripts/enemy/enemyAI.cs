using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{

    [Header("--- Components ---")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletShootPosition;
    [SerializeField] GameObject missle;
    [SerializeField] GameObject missleShootPosition;
    [SerializeField] GameObject lootDrop;

    [Header("--- Ship Movement ---")]
    [SerializeField] float movementSpeed;
    [SerializeField] float orbitSpeed;
    [SerializeField] float orbitRange;
    [SerializeField] float maxAngle;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [Header("--- Ship Weapons ---")]
    [SerializeField] float bulletShootRate;
    [SerializeField] float bulletShootRange;
    [SerializeField] int bulletCount;
    [SerializeField] float missleShootRate;
    [SerializeField] float missleShootRange;
    [SerializeField] int missleCount;

    [Header("--- Ship Loot ---")]
    [SerializeField] List<Item> possibleLoot;
    [SerializeField] int maxLootPossible;
    List<Item> loot;

    [Header("--- Ship ---")]
    [SerializeField] int health;
    [SerializeField] bool hasShield;

    private bool isOrbitting;
    private bool isShootingBullet;
    private bool isShootingMissle;
    private Vector3 lastPosition;
    private enum AIState
    {
        Attack,
        Orbit,
        WaypointTravel
    }
    private AIState currentState;
    private Vector3 orbitStartPosition;
    private Vector3 waypoint;
    private bool waypointReached;
    private bool orbitWaytime;

    void Awake()
    {
        currentState = AIState.Attack;
        lastPosition = transform.position;
        setLoot();
        bulletCount = Random.Range(0, bulletCount);
        missleCount = Random.Range(0, missleCount);
        health = Random.Range(1, health);
        bulletShootRate = Random.Range(bulletShootRate, bulletShootRate * 3);
        missleShootRate = Random.Range(missleShootRate, missleShootRate * 3);
    }

    void setLoot()
    {
        for(int i = 0; i < maxLootPossible; i++)
        {
            loot.Add(possibleLoot[Random.Range(0, possibleLoot.Count)]);
        }
    }

    float calculateEnemyToShipAngle()
    {
        Vector3 directionToTarget = shipManager.instance.transform.position - transform.position;
        return Vector3.Angle(transform.forward, directionToTarget);
    }
    void moveTowardsShip()
    {
        Vector3 moveDirection = (shipManager.instance.transform.position - transform.position).normalized;
        transform.position += moveDirection * movementSpeed * Time.deltaTime;
        faceTravelDirection();
    }
    bool isWithinOrbitRange()
    {
        if (Vector3.Distance(transform.position, shipManager.instance.transform.position) <= orbitRange)
        {
            return true;
        }
        return false;
    }
    void orbitAroundShip()
    {
        transform.RotateAround(shipManager.instance.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
        faceTravelDirection();
    }
    bool hasReachedOrbitStartPosition()
    {
        if(Vector3.Distance(transform.position, orbitStartPosition) <= 1)
        {
            return true;
        }
        return false;
    }
    void moveToWaypoint()
    {
        Vector3 moveDirection = (waypoint - transform.position).normalized;
        transform.position += moveDirection * movementSpeed * Time.deltaTime;
        faceTravelDirection();
    }
    bool isWithinWaypointRadius()
    {
        if(Vector3.Distance(transform.position, waypoint) < 50f)
        {
            return true;
        }
        return false;
    }
    Vector3 selectWaypoint()
    {
        float randomAngle = Random.Range(-maxAngle, maxAngle);
        Debug.Log("Angle: " + randomAngle);
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward;

        float distance = Random.Range(minRange, maxRange);
        Debug.Log("Distance: " + distance);

        Vector3 randomPosition = transform.position + randomDirection * distance;
        randomPosition = new Vector3(randomPosition.x, 0, randomPosition.z);
        return randomPosition;
    }
    IEnumerator startOrbitWay()
    {
        yield return new WaitForSeconds(2);
        orbitWaytime = false;

    }
    void faceTravelDirection()
    {
        Vector3 moveDirection = (transform.position - lastPosition).normalized;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = newRotation;

        lastPosition = transform.position;
    }
    void Update()
    {
        faceTravelDirection();
        float shipAngle = calculateEnemyToShipAngle();
        switch (currentState)
        {
            case AIState.Attack:
                moveTowardsShip();
                StartCoroutine(shootBullet());
                StartCoroutine(shootMissle());

                Debug.Log("Within Orbit Range:" + isWithinOrbitRange());
                if (isWithinOrbitRange())
                {
                    orbitStartPosition = transform.position;

                    currentState = AIState.Orbit;
                    orbitWaytime = true;
                }
                break;
            case AIState.Orbit:
                Debug.Log("Has Reached Start Poisition: " + hasReachedOrbitStartPosition());
                orbitAroundShip();
                if (hasReachedOrbitStartPosition() && !orbitWaytime)
                {
                    waypoint = selectWaypoint();
                    currentState = AIState.WaypointTravel;
                }else if (orbitWaytime)
                {
                    StartCoroutine(startOrbitWay());
                }
                break;
            case AIState.WaypointTravel:
                Debug.Log("Within Waypoint Radius: " + isWithinWaypointRadius());
                moveToWaypoint();
                if(isWithinWaypointRadius())
                {
                    waypointReached = true;
                }

                if (waypointReached && shipAngle < 5)
                {
                    Debug.Log("Waypoint Reached: " + waypointReached);
                    currentState = AIState.Attack;
                    waypointReached = false;
                }
                else if(waypointReached)
                {
                    transform.RotateAround(waypoint, Vector3.up, orbitSpeed * Time.deltaTime);
                }
                break;
        }
       // if(!isOrbitting && distanceToTarget > orbitRange)
       // {
       //     Vector3 moveDirection = (shipManager.instance.transform.position - transform.position).normalized;
       //     transform.position += moveDirection * movementSpeed * Time.deltaTime;
       //     transform.LookAt(shipManager.instance.transform.position);
       // }

       // //Only orbits in 2d, additional functionilty required.
       // else
       // {
       //     isOrbitting = true;
       //     transform.RotateAround(shipManager.instance.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
       //     transform.LookAt(shipManager.instance.transform.position);
       // }

       // if (distanceToTarget < bulletShootRange && !isShootingBullet && bulletCount > 0)
       // {
       //     StartCoroutine(shootBullet());
       // }

       // if(distanceToTarget < missleShootRange && !isShootingMissle && missleCount > 0)
       // {
       //     StartCoroutine(shootMissle());
       // }
       // // Loot drop tester, kills enemy to test loot drop.
       //// StartCoroutine(damageTest());
    }

    IEnumerator shootBullet()
    {
        isShootingBullet = true;
        //Debug.Log("Spawn bullet");
        Instantiate(bullet, bulletShootPosition.transform.position, transform.rotation);
        bulletCount--;
        yield return new WaitForSeconds(bulletShootRate);
        isShootingBullet = false;
    }
    IEnumerator shootMissle()
    {
        isShootingMissle = true;
        Instantiate(missle, missleShootPosition.transform.position, transform.rotation);
        missleCount--;
        yield return new WaitForSeconds(missleShootRate);
        isShootingMissle = false;
    }

    IEnumerator explode()
    {
        //Play particle effect and sound effect

        //Game object should stop orbitting and continue in direction it was last traveling as well.
        //Game object should stop shooting.
        yield return new WaitForSeconds(3);
        Instantiate(lootDrop, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }

    public void takeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            StartCoroutine(explode());
        }

    }
    // Function for loot drop test, at the time of writing this there is no way to kill the enemy that I found.

    //IEnumerator damageTest()
    //{
    //    yield return new WaitForSeconds(5);
    //    takeDamage(1);
    //}
}
