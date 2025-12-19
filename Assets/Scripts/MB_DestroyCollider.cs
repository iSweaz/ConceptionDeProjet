using UnityEngine;

public class MB_DestroyCollider : MonoBehaviour
{
    private BoxCollider collision;
    void Start()
    {
        collision = GetComponent<BoxCollider>();
    }

    public void destroyCollider()
    {
        Destroy(collision);
    }
}
