using PurrNet;
using UnityEngine;

public class NI_Selection : NetworkIdentity
{
    Camera cam;
    public string playerAnswer = "i";

    private GameObject lastObject;
    private Renderer objectRenderer;

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
            Debug.Log(hit.transform.CompareTag("Card"));
            if(hit.transform.CompareTag("Card"))
            {
                ChangeAnwser(hit.transform.name);
                ChangeColor(hit.transform.gameObject,objectRenderer);
            }
        }
    }

    //[ObserversRpc] // pas nécessaire puisque les autres joueurs ne voient pas mais c'est pour mes test ^^
    public void ChangeColor(GameObject gameObject, Renderer renderer)
    {
        if(lastObject)
        {
            if(renderer == null)
                renderer = lastObject.GetComponent<Renderer>();
            renderer.materials[1].color = Color.red;
        } 
        if(gameObject)
        {
            renderer = gameObject.GetComponent<Renderer>();
            //Change the GameObject's Material Color to red
            renderer.materials[1].color = Color.green;
            lastObject = gameObject;   
        }        
    }

    [ServerRpc]
    void ChangeAnwser(string answer)
    {
        playerAnswer = answer;
    }



}
