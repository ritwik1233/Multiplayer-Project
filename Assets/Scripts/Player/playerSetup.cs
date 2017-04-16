
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class playerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    string remoteLayerName="RemotePlayer";
    
    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIinstance;
    Camera sceneCamera;
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
         playerUIinstance=Instantiate(playerUIPrefab);
            playerUIinstance.name = playerUIPrefab.name;
        }
        GetComponent<Player>().Setup();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netID,player);
    }
    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }

    }
    void AssignRemoteLayer()
        {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        
        }
    void OnDisable()
    {

        Destroy(playerUIinstance);
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }
    
}
