using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("--- Components ---")]
    public CharacterController controller;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] GameObject bulletFlash;
    [SerializeField] GameObject gunModel;
    [SerializeField] AudioSource gunshotSound;
    [SerializeField] AudioSource playerHurtAudio;
    [SerializeField] AudioSource playerJumpAudio;
    public Camera camera;

    [Header("--- Player Stats ---")]
    public float healthPoints;
    public float playerSpeed;
    public float runSpeed;
    public int jumpsMax;
    private float exhSpeed = 2;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int pushBackResolve;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    //[SerializeField] float lifeSteal;
    [SerializeField] float leanSpeed;
    [SerializeField] float leanMaxAngle;
    [SerializeField] Vector3 pushBack;
    public float startHealth;
    [SerializeField] int totalStamina;
    public float playerStamina;
    private float tickRate = .1f;
    private bool isRegening;
    private bool isExhausted;

    [Header("--- Interaction Stats ---")]
    [SerializeField] int interactionDistance;

    //Variable Defintions: 
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpedTimes;
    private bool isShooting;
    private int pushBackResTemp;
    [SerializeField] float baseSpeed = 6;
    private bool isSprinting;
    private ParticleSystem gunshotEffect;
    private float baseSpeedOrignal;


    private IInteractable lastInteract;
    public bool amEnabled;
    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        startHealth = healthPoints;
        playerStamina = totalStamina;
        pushBackResTemp = pushBackResolve;
        canMove = true;
        //gamemanager.instance.playerScript.spawnPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movement();
        }
        Sprint();
        checkInteractable();
    }
    public void toggleMovement(bool state)
    {
        canMove = state;
    }

    void checkInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, interactionDistance))
        {
            IInteractable interact;
            if(hit.collider.TryGetComponent(out interact)){
                Debug.Log(interact);
                interact.onInteractable(true);
                lastInteract = interact;
            }
            else if(lastInteract != null) 
            {
                lastInteract.onInteractable(false);
                lastInteract = null;
            }
        }
    }

    void movement()
    {
    
        if (pushBack.magnitude > 0.01f)
        {
            pushBack.x = Mathf.Lerp(pushBack.x, 0, Time.deltaTime * pushBackResolve);
            pushBack.y = Mathf.Lerp(pushBack.y, 0, Time.deltaTime * pushBackResolve * 3);
            pushBack.z = Mathf.Lerp(pushBack.z, 0, Time.deltaTime * pushBackResolve);
        }
    
    
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }
    
        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;
    
        if (isExhausted)
        {
            controller.Move(move * Time.deltaTime * (baseSpeed - exhSpeed));
        }
        else
        {
            controller.Move(move * Time.deltaTime * (baseSpeed + runSpeed));
        }
    
    
    
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
            StartCoroutine(playAudioClip(playerJumpAudio));
        }
    
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

    }

    IEnumerator playAudioClip(AudioSource clip)
    {
        if (clip != null)
        {
            clip.Play();
            yield return new WaitForSeconds(clip.clip.length);
        }
    }

    void Sprint()
    {
        if (playerStamina > 0)
        {
            if (Input.GetButton("Sprint"))
            {
                if (!isSprinting)
                {
                    runSpeed = baseSpeed * 1.1f;
                    isSprinting = true;
                }
    
                if (!isRegening)
                {
                    if (playerStamina > 0)
                    {
                        StartCoroutine(DrainStamina());  // 1
                    }
                    else if (playerStamina <= 0)
                    {
                        StopCoroutine(DrainStamina());
                        playerStamina = 0;
                    }
                }
            }
            else
            {
                StopCoroutine(DrainStamina());
                runSpeed = runSpeed / 2;
                isSprinting = false;
    
                if (playerStamina < totalStamina && !isRegening)
                {
                    StartCoroutine(RegenStamina());
                }
            }
        }
        else if (isExhausted)
        {
            isSprinting = false;
            StartCoroutine(RegenStamina());
        }
        else if (playerStamina <= 0)
        {
            StartCoroutine(StaminaBreak());
            StopCoroutine(StaminaBreak());
        }
    }
    
    IEnumerator DrainStamina()
    {
    
        if (isSprinting && playerStamina > 0 && !isRegening)
        {
            playerStamina -= 20 * Time.deltaTime;
            yield return new WaitForSeconds(tickRate);
        }
    }
    
    IEnumerator RegenStamina()
    {
        if (playerStamina < totalStamina && !isSprinting)
        {
            Debug.Log("Regenning Stamina");
            isRegening = true;
    
            playerStamina += 2;
    
            yield return new WaitForSeconds(tickRate);
    
            isRegening = false;
        }
    }
    
    IEnumerator StaminaBreak()
    {
        isExhausted = true;
        yield return new WaitForSeconds(5);
        isExhausted = false;
    }

    public void setPosition(Transform position)
    {
        controller.enabled = false;
        transform.position = position.position;
        controller.enabled = true;
    }
    
    //IEnumerator shoot()
    //{
    //    isShooting = true;
    //    RaycastHit hit;
    //
    //    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
    //    {
    //        try
    //        {
    //            IDamage damagable = hit.collider.GetComponent<IDamage>();
    //            if (damagable != null)
    //            {
    //                damagable.takeDamage(shootDamage);
    //                if (startHealth >= healthPoints + (shootDamage * lifeSteal))
    //                {
    //                    healthPoints += shootDamage * lifeSteal;
    //                    updatePlayerUI();
    //                }
    //                else if (startHealth <= healthPoints + (shootDamage * lifeSteal))
    //                {
    //                    float healToFull = startHealth - healthPoints;
    //                    healthPoints += healToFull;
    //                    updatePlayerUI();
    //                }
    //
    //            }
    //        }
    //        catch
    //        {
    //
    //        }
    //    }
    //
    //    yield return new WaitForSeconds(shootRate);
    //    isShooting = false;
    //    gunshotEffect.Stop();
    //}

    //public void giveHealthPoints(int amount)
    //{
    //    healthPoints += amount;
    //    updatePlayerUI();
    //}
    //
    //public void takeDamage(int amount)
    //{
    //    healthPoints -= amount;
    //    StartCoroutine(gamemanager.instance.playerFlashDamage());
    //    updatePlayerUI();
    //    StartCoroutine(playAudioClip(playerHurtAudio));
    //
    //    if (healthPoints <= 0 && !gamemanager.instance.isPaused)
    //    {
    //        gamemanager.instance.youLoseMenu();
    //    }
    //}
    //
    //public void updatePlayerUI()
    //{
    //    gamemanager.instance.playerHPBar.fillAmount = (float)healthPoints / startHealth;
    //    gamemanager.instance.healthPointsText.text = healthPoints.ToString() + " / " + startHealth.ToString();
    //}
    //
    //public void spawnPlayer()
    //{
    //    healthPoints = startHealth;
    //    //updatePlayerUI();
    //    controller.enabled = false;
    //    transform.position = gamemanager.instance.playerSpawnPos.transform.position;
    //    controller.enabled = true;
    //}
    //public void physics(Vector3 push)
    //{
    //    pushBack += push;
    //}
    //public void addPlayerSeed(float amount)
    //{
    //    baseSpeedOrignal = baseSpeed;
    //    baseSpeed += amount;
    //}
    //public void setPlayerSpeed(float amount)
    //{
    //    baseSpeedOrignal = baseSpeed;
    //    baseSpeed = amount;
    //}
    //public void addPlayerDamage(int amount)
    //{
    //    shootDamage += amount;
    //}
    //public void setPlayerDamage(int amount)
    //{
    //    shootDamage = amount;
    //}
    //public void addPlayerJumps(int amount)
    //{
    //    jumpsMax += amount;
    //}
    //public void setPlayerJumps(int amount)
    //{
    //    jumpsMax = amount;
    //}
    //public void addShootRate(float amount)
    //{
    //    shootRate -= amount;
    //}
    //public void setShootRate(float amount)
    //{
    //    shootRate = amount;
    //}
    //
    //public void addPlayerHealth(int amount)
    //{
    //    healthPoints += amount;
    //    startHealth += amount;
    //}
    //public void setPlayerHealth(int amount)
    //{
    //    healthPoints = amount;
    //}
    //public void addPlayerLifesteal(float amount)
    //{
    //    lifeSteal += amount;
    //}
    //public void addPlayerShootDistance(int amount)
    //{
    //    shootDistance += amount;
    //}
    //public void airLiftToggle()
    //{
    //    if (pushBack.y == 0)
    //    {
    //        pushBackResolve = 0;
    //    }
    //    if (pushBack.y != 0)
    //    {
    //        pushBackResolve = pushBackResTemp;
    //    }
    //}
    //public void setGunModel(gun model)
    //{
    //    GameObject spawnedGun = Instantiate(model.model, gunModel.transform.position, gunModel.transform.rotation);
    //    spawnedGun.transform.parent = gunModel.transform;
    //    gunshotEffect = gunModel.GetComponentInChildren<ParticleSystem>();
    //    shootDamage = model.shootDamage;
    //    shootDistance = model.shootDistance;
    //    shootRate = model.shootRate;
    //}
}
