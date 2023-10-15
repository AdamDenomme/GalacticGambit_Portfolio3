using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] float destoryTime;
    [SerializeField] ParticleSystem paricle;

    void Start()
    {
        rigidBody.velocity = transform.forward * speed;
        Destroy(gameObject, destoryTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            return;
        }
        IDamage damageable = other.GetComponent<IDamage>();
        if(damageable != null)
        {
            damageable.takeDamage(damage);
        }
        ParticleSystem spawnedParticleSystem = Instantiate(paricle, transform.position, Quaternion.Inverse(transform.rotation));
        Destroy(spawnedParticleSystem, 2);
        Destroy(gameObject);
    }
}
