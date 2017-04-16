using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class Player : NetworkBehaviour {

    [SyncVar]
    private bool isDead = false;
    public bool _isDead
    {
        get { return isDead; }
        protected set { isDead = value; }
    }
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar] //used to push the values to all the clients
    private int currentHealth;
    Animator anim;
    // Use this for initialization
    void Awake ()
    {
        
        anim = GetComponent<Animator>();
	}
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;
        Debug.Log(transform.name +" now has "+ currentHealth +" health");
        if (currentHealth <= 0)
        {
            Die();

        }
    }
    private void Die()
    {
        isDead = true;
        anim.SetBool("Dead",true);
        //Disable Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        //respawn
        StartCoroutine(Respawn());
       
    }
    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        anim.SetBool("Dead", false);
    }
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(100);
        }
	}
}
