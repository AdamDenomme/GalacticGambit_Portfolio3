using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class turretController : MonoBehaviour
{
    [SerializeField] List<GameObject> variations;
    [SerializeField] GameObject bullet;
    public float attackSpeed;
    public int rotationSpeed;
    public int additionalDamage;
    [SerializeField] float tractorSpeed;
    [SerializeField] Sprite reticle;
    [SerializeField] Camera camera;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] ParticleSystem tractorParicle;
    [SerializeField] int maxAmmo;
    [SerializeField] int maxMissle;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject misslePrefab;
    [SerializeField] float missleAttackSpeed;
    public int missleAmmo;
    public int currentAmmo;
    public TMP_Text ammo;

    GameObject selectTurret;

    public bool isActive = false;
    bool isShooting;
    bool isMissling;

    private void Start()
    {
        missleAmmo = maxMissle;
        currentAmmo = maxAmmo;
        updateAmmoUI();
    }
    //
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            float rotationAngle;
            laser.SetActive(true);

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
            if (Input.GetButton("Missle"))
            {
                if (!isMissling)
                {
                    StartCoroutine(shootMissle());
                }
            }
            
            if (Input.GetButton("Tractor"))
            {
                //Debug.Log("Tractor");
                Ray ray = new Ray(bulletSpawn.position, transform.forward);
                //Debug.DrawRay(bulletSpawn.position, transform.forward * 200f, Color.green);
                RaycastHit hit;
                tractorParicle.gameObject.SetActive(true);
                if(Physics.Raycast(ray, out hit, 200f))
                {
                    hit.collider.transform.position = Vector3.MoveTowards(hit.collider.transform.position, bulletSpawn.position, tractorSpeed * Time.deltaTime);
                    //Debug.Log(Vector3.Distance(hit.collider.transform.position, bulletSpawn.position));
                    if(Vector3.Distance(hit.collider.transform.position, bulletSpawn.position) < 15f)
                    {
                        loot lootItem;
                        hit.collider.gameObject.TryGetComponent(out lootItem);
                        if(lootItem != null)
                        {
                            foreach(Item item in lootItem.lootItems)
                            {
                                shipManager.instance.inventory.addItem(item);
                            }
                            
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
        else
        {
            laser.SetActive(false);
        }
        updateAmmoUI();

    }

    IEnumerator shoot()
    {
        if (currentAmmo > 0)
        {
            isShooting = true;
            GameObject spawnedBullet = Instantiate(bullet, bulletSpawn);
            spawnedBullet.transform.parent = null;
            currentAmmo -= 1;
            yield return new WaitForSeconds(attackSpeed);
            isShooting = false;
            updateAmmoUI();
        }
    }
    IEnumerator shootMissle()
    {
        if(missleAmmo > 0)
        {
            isMissling = true;
            GameObject spawnedMissle = Instantiate(misslePrefab, bulletSpawn);
            spawnedMissle.transform.parent = null;
            missleAmmo -= 1;
            yield return new WaitForSeconds(missleAttackSpeed);
            isMissling = false;
            updateAmmoUI();

        }
    }

    public void updateAmmoUI()
    {
        ammo.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        shipManager.instance.ammoTotal = currentAmmo;
        shipManager.instance.missleTotal = missleAmmo;
    }
}
