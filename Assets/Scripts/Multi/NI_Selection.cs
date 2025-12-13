using PurrNet;
using UnityEngine;

public class NI_Selection : NetworkIdentity
{
    Camera cam;
    public string playerAnswer = "i";

    private GameObject lastObject = null;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void  Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RayCast();
        }
    }

    void RayCast()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit,15) )
        { 
            if(hit.transform.CompareTag("Card"))
            {
                ChangeAnwser(hit.transform.name);
                ChangeColor(hit.transform.gameObject);
            }
        }
    }

    //[ObserversRpc] // pas nécessaire puisque les autres joueurs ne voient pas mais c'est pour mes test ^^
    public void ChangeColor(GameObject gameObject)
    {
        Renderer render;
        if(lastObject)
        {
            render = lastObject.GetComponent<Renderer>();
            render.materials[1].color = Color.red;
        } 
        if(gameObject)
        {
            render = gameObject.GetComponent<Renderer>();
            //Change the GameObject's Material Color to red
            render.materials[1].color = Color.green;
            lastObject = gameObject;   
        }        
    }

    [ServerRpc]
    void ChangeAnwser(string answer)
    {
        playerAnswer = answer;
        Debug.Log("Change value : " + playerAnswer);
    }
    

    [ObserversRpc]
    public void resetValue()
    {
        Debug.Log("Reset");
        ChangeColor(null);
        playerAnswer ="i";
    }

    /// <summary>
    /// S'enregistre seul auprès du GameModemanager à son apparition
    /// </summary>
    protected override void OnSpawned()
    {
        if (isServer)
        {
            NI_GameManager manager = FindFirstObjectByType<NI_GameManager>();
            if(manager)
            {
                manager.players.Add(this);
                manager.Answers.Add(playerAnswer);
            }
        }
    }

    /// <summary>
    /// Se retire seul auprès du GameModemanager à sa disparition
    /// </summary>
    protected override void OnDespawned()
    {
        if (isServer)
        {
            NI_GameManager manager = FindFirstObjectByType<NI_GameManager>();
            if(manager)
            {
                manager.players.Remove(this);
                manager.Answers.Remove(playerAnswer);                
            }

        }
    }
}
