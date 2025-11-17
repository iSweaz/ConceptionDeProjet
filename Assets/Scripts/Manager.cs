using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public UserStorys deck;
    private Display_Unit blackBoard;
    private Timer timer; 

    public float timeRemaining = 5;

    private int compteurItem = 0;
    private int lengthDeck = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lengthDeck = deck.US.Length;
        
        blackBoard = FindFirstObjectByType<Display_Unit>();
        blackBoard.deck = deck;
        blackBoard.ChangeDisplay(compteurItem);

        timer = FindFirstObjectByType<Timer>();
        timer.timeRemaining = timeRemaining;
        timer.timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if( compteurItem < deck.US.Length)
        {
            if(timer.timeRemaining == 0)
            {
                Debug.Log("Change Item");
                compteurItem++;
                timer.timeRemaining = timeRemaining;
                timer.timerIsRunning = true;

                blackBoard.ChangeDisplay(compteurItem);
            }
        }
        else
        {
            Destroy(timer);
            Time.timeScale = 0;
        }
    }

}
