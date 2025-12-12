using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public UserStorys deck;
    private Display_Unit blackBoard;
    private Timer timer; 

    public float timeRemaining = 5;

    [SerializeField]private int compteurItem = 0;
    private int lengthDeck = 0;

    private string Answer; 
    private Selection player; // À changer quand on commencera le multi. Voir si on fait une préfab ou si chaque joueur à sa propre scène et à ce moment pas besoin d'y touché


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lengthDeck = deck.US.Length;
        
        blackBoard = FindFirstObjectByType<Display_Unit>();
        // blackBoard.deck = deck;
        // blackBoard.ChangeDisplay(compteurItem);

        timer = FindFirstObjectByType<Timer>();
        timer.timeRemaining = timeRemaining;
        timer.timerIsRunning = true;

        player = GetComponent<Selection>();
    }

    // Update is called once per frame
    void Update()
    {
        //Probablement à moove sur le singleton pour la com serveur, par encore réfléchit à ça
        if( compteurItem + 1 < deck.US.Length)
        {
            if(timer.timeRemaining == 0)
            {
                compteurItem++;
                timer.timeRemaining = timeRemaining;
                timer.timerIsRunning = true;

                // blackBoard.ChangeDisplay(compteurItem); // à retirer, présent pour check le fonctionnement du changement de US
                Answer = "";
                player.answer = "i";
                player.ChangeColor(null);
            }
        }
        else if(timer.timeRemaining == 0)
        {
            Answer = player.answer;
            Debug.Log(Answer);
            timer.timeText.text = "";
            Time.timeScale = 0;   
        }
    }

    // void loadCard()
    // {
    //     GameObject Parent =  GameObject.Find("Cards");
    //     GameObject[] Cards =  new GameObject[11];
    //     for (int i = 0; i < Cards.Length; i++)
    //     {
    //         Cards[i] = Parent.transform.GetChild(i).gameObject;
    //     }
    // }
}