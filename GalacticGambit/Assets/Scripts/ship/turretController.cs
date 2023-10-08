using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretController : MonoBehaviour
{
    [SerializeField] List<GameObject> variations;
    [SerializeField] GameObject bullet;
    [SerializeField] float attackSpeed;
    [SerializeField] int rotationSpeed;
    [SerializeField] float tractorSpeed;
    [SerializeField] Sprite reticle;
    [SerializeField] Camera camera;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] ParticleSystem tractorParicle;

    GameObject selectTurret;

    bool isActive = true;
    bool isShooting;
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            float rotationAngle;

            if (Input.GetButton("turret+"))
            {
                rotationAngle = 1  * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.down, rotationAngle);
            }

            if (Input.GetButton("turret-"))
            {
                rotationAngle = -1 * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.down, rotationAngle);
            }

            if (Input.GetButton("Jump"))
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
            }
            
            if (Input.GetButton("Tractor"))
            {
                Debug.Log("Tractor");
                Ray ray = new Ray(bulletSpawn.position, transform.forward);
                Debug.DrawRay(bulletSpawn.position, transform.forward * 200f, Color.green);
                RaycastHit hit;
                tractorParicle.gameObject.SetActive(true);
                if(Physics.Raycast(ray, out hit, 200f))
                {
                    hit.collider.transform.position = Vector3.MoveTowards(hit.collider.transform.position, bulletSpawn.position, tractorSpeed * Time.deltaTime);
                    Debug.Log(Vector3.Distance(hit.collider.transform.position, bulletSpawn.position));
                    if(Vector3.Distance(hit.collider.transform.position, bulletSpawn.position) < 15f)
                    {
                        loot lootItem;
                        hit.collider.gameObject.TryGetComponent(out lootItem);
                        if(lootItem != null)
                        {
                            shipManager.instance.inventory.addItem(lootItem.lootItem);
                        }

                        Destroy(hit.collider.gameObject);
                    }
                }

            }
            else
            {
                tractorParicle.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        GameObject spawnedBullet = Instantiate(bullet, bulletSpawn);
        spawnedBullet.transform.parent = null;
        yield return new WaitForSeconds(attackSpeed);
        isShooting = false;
    }
}
