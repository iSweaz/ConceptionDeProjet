using UnityEngine;

public class Selection : MonoBehaviour
{
    Camera cam;
    public string answer = "i";

    private GameObject lastObject;
    private Renderer m_ObjectRenderer;

    void Start()
    {
        cam = GetComponent<Camera>();

    }

    void Update()
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
                answer = hit.transform.name;
                ChangeColor(hit.transform.gameObject);
            }
        }
    }
    public void ChangeColor(GameObject gameObject)
    {
        if(lastObject)
        {
            m_ObjectRenderer.materials[1].color = Color.red;
        } 
        if(gameObject)
        {
            m_ObjectRenderer = gameObject.GetComponent<Renderer>();
            //Change the GameObject's Material Color to red
            m_ObjectRenderer.materials[1].color = Color.green;
            lastObject = gameObject;   
        }        
    }
}
