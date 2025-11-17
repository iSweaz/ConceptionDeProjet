using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public UserStorys deck;
    [SerializeField] private Display_Unit blackBoard;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blackBoard.deck = deck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
