using UnityEngine;
using UnityEngine.Networking;


public class PlayerShooting : NetworkBehaviour
{
    
    public PlayerWeapon weapon;
    AudioSource gunAudio;
    ParticleSystem gunfire;
    private const string PLAYERTAG = "player";    
    [SerializeField]
    private GameObject barrel;
    [SerializeField]
    private LayerMask mask;
    void Start()
    {
        gunAudio = GetComponentInChildren<AudioSource>();
        gunfire = GetComponentInChildren<ParticleSystem>();
        if (barrel == null)
        {

            Debug.LogError("PlayerShoot :No camera referenced");
            this.enabled = false;
        }

    }
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
           // gunAudio.Stop();
        }
        
    }
    [Client]
    void Shoot()
    {
       
        RaycastHit hit;
        gunAudio.Play();
        gunfire.Stop();
        gunfire.Play();
        
        if (Physics.Raycast(barrel.transform.position,barrel.transform.forward,out hit,weapon.range,mask))
        {
            //we hit something

            if (hit.collider.tag==PLAYERTAG)
            {
                CmdPlayerShot(hit.collider.name,weapon.damage);
            }
           
        }
        
    }
    [Command]
    void CmdPlayerShot(string playerID,int damage)
    {
        Debug.Log(playerID + "shot");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }

}
