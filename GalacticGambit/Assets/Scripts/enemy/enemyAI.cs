using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] shipManager ship;
    [SerializeField] float movementSpeed;
    [SerializeField] float orbitSpeed;
    [SerializeField] float orbitRange;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletShootPosition;
    [SerializeField] float bulletShootRate;
    [SerializeField] float bulletShootRange;
    [SerializeField] int bulletCount;
    [SerializeField] GameObject missle;
    [SerializeField] GameObject missleShootPosition;
    [SerializeField] GameObject lootDrop;
    [SerializeField] float missleShootRate;
    [SerializeField] float missleShootRange;
    [SerializeField] int missleCount;
    [SerializeField] int health;

    private bool isOrbitting;
    private bool isShootingBullet;
    private bool isShootingMissle;

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, ship.transform.position);

        if(!isOrbitting && distanceToTarget > orbitRange)
        {
            Vector3 moveDirection = (ship.transform.position - transform.position).normalized;
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
            transform.LookAt(ship.transform.position);
        }

        //Only orbits in 2d, additional functionilty required.
        else
        {
            isOrbitting = true;
            transform.RotateAround(ship.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            transform.LookAt(ship.transform.position);
        }

        if (distanceToTarget < bulletShootRange && !isShootingBullet && bulletCount > 0)
        {
            StartCoroutine(shootBullet());
        }

        if(distanceToTarget < missleShootRange && !isShootingMissle && missleCount > 0)
        {
            StartCoroutine(shootMissle());
        }
        // Loot drop tester, kills enemy to test loot drop.
       // StartCoroutine(damageTest());
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
