using PurrNet;
using UnityEngine;

public class NI_Selection : NetworkIdentity
{
    Camera cam;
    public string answer = "i";
    [SerializeField] private MB_DestroyCollider card;
    private GameObject lastObject = null;

    void Start()
    {
        cam = GetComponent<Camera>();
        card = gameObject.transform.parent.GetComponentInChildren<MB_DestroyCollider>();

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RayCast();
        }
    }

    /// <summary>
    /// Crée un laser vers des coordonnées dans le monde 3D
    /// </summary>
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

    /// <summary>
    /// Change la couleur de la carte sélectionnée ou désélectionnée
    /// </summary>
    /// <param name="gameObject">Carte</param>
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

    /// <summary>
    /// Informe le serveur d'un changement de réponse
    /// </summary>
    /// <param name="newAnswer">Nouvelle réponse sélectionnée</param>
    [ServerRpc]
    void ChangeAnwser(string newAnswer)
    {
        answer = newAnswer;
    }
    
    /// <summary>
    /// Remet la réponse par défaut "i"
    /// </summary>
    [ObserversRpc]
    public void resetValue()
    {
        Debug.Log("Reset");
        ChangeColor(null); // On réinitialise l'état de toutes les cartes
        answer = "i"; // On remet player.answer à sa valeur par défaut
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
            }

        }
    }

    [ObserversRpc]
    public void DestroyCoffee()
    {
        card.destroyCollider();    
    }
}
