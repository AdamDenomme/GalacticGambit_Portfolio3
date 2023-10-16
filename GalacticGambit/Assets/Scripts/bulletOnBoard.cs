using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletOnBoard : MonoBehaviour
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
        if (other.isTrigger)
        {
            return;
        }
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.takeDamage(damage);
        }
        ParticleSystem spawnedParticleSystem = Instantiate(paricle, transform.position, Quaternion.Inverse(transform.rotation));
        spawnedParticleSystem.transform.parent = gameObject.transform;
        StartCoroutine(destroyParticle(spawnedParticleSystem));
        rigidBody.velocity = Vector3.zero;
        Destroy(gameObject, .2f);
       
    }
    IEnumerator destroyParticle(ParticleSystem system)
    {
        yield return new WaitForSeconds(2);
        Destroy(system);
    }
}
