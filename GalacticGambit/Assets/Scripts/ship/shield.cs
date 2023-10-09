using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour, IDamage
{
    [SerializeField] int regenTime;
    [SerializeField] Collider meshCollider;
    [SerializeField] Renderer meshRenderer;
    public int health;

    public int currentHealth;

    bool regenUI;
    bool isRegening;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRegening && !regenUI)
        {
            StartCoroutine(regenShieldUI());
        }
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log("Shield: Regenerating!");
            StartCoroutine(regenShield());
        }
        
    }
    public IEnumerator regenShieldUI()
    {
        regenUI = true;
        currentHealth += health / regenTime;
        yield return new WaitForSeconds(1);
        regenUI = false;
    }

    public IEnumerator regenShield()
    {
        isRegening = true;
        meshCollider.enabled = false;
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(regenTime);
        meshCollider.enabled = true;
        meshRenderer.enabled = true;
        isRegening = false;
    }
    public void toggleState(bool state)
    {
        if (state)
        {
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
            currentHealth = 0;
        }
        else
        {
            meshCollider.enabled = true;
            meshRenderer.enabled = true;
            currentHealth = 0;
            StartCoroutine(regenShield());
        }
    }
}
