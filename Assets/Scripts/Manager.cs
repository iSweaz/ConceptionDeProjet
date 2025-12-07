using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //[SerializeField] public UserStorys deck;
    public USJsonFile deck;
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
        lengthDeck = deck.usdata_list.Count();
        
        blackBoard = FindFirstObjectByType<Display_Unit>();
        blackBoard.deck = deck;
        blackBoard.ChangeDisplay(compteurItem);

        timer = FindFirstObjectByType<Timer>();
        timer.timeRemaining = timeRemaining;
        timer.timerIsRunning = true;

        player = FindFirstObjectByType<Selection>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si le timer est à 0, on check
        if (timer.timeRemaining <= 0 && timer.timerIsRunning)
        {
            // On process l'answer 
            ProcessAnswer(player.answer); // On applique le script de procession des différents effets possible

            // Si on a pas encore atteint le bout de la liste
            if (compteurItem + 1 < deck.usdata_list.Count())
            {
                // On passe à la prochaine task et on reset les variables
                compteurItem++; // on incrémente l'index
                timer.timeRemaining = timeRemaining; // On remet le timer au temps défini 
                player.ChangeColor(null); // On réinitialise l'état de toutes les cartes
                player.answer = "i"; // On remet player.answer à sa valeur par défaut
                Answer = null;
                blackBoard.ChangeDisplay(compteurItem); // On actualise l'affichage tableau
            }
            // Sinon, on a atteint la fin de la liste : on désactive le timer
            else
            {
                timer.timerIsRunning = false; // On passe à false le booléen qui permet d'entrer dans la boucle
                timer.timeText.text = ""; // On reset le texte
                Time.timeScale = 0; // On bloque l'écoulement du timer
                Answer = null;
            }
        }

        //Probablement à moove sur le singleton pour la com serveur, par encore réfléchit à ça
        /*if( compteurItem + 1 < deck.usdata_list.Count())
        {
            if(timer.timeRemaining == 0)
            {
                compteurItem++;
                timer.timeRemaining = timeRemaining;
                timer.timerIsRunning = true;
                ProcessAnswer(player.answer);

                blackBoard.ChangeDisplay(compteurItem); // à retirer, présent pour check le fonctionnement du changement de US
                Answer = "";
                player.answer = "i";
                player.ChangeColor(null);
            }
        }
        else if(timer.timeRemaining == 0)
        {
            Answer = player.answer;
            Debug.Log(Answer); // -> On fait des actions ici
            //ProcessAnswer();

            timer.timeText.text = "";
            Time.timeScale = 0;   
        }*/
    }

    /// <summary>
    /// Fonction qui gère le résultat de la carte sélectionnée par l'utilisateur
    /// </summary>
    /// <param name="answer"></param>
    void ProcessAnswer(string answer)
    {
        switch(answer)
        {
            case "0": deck.usdata_list[compteurItem].score = 0; break;
            case "1": deck.usdata_list[compteurItem].score = 1; break;
            case "2": deck.usdata_list[compteurItem].score = 2; break;
            case "3": deck.usdata_list[compteurItem].score = 3; break;
            case "5": deck.usdata_list[compteurItem].score = 5; break;
            case "8": deck.usdata_list[compteurItem].score = 8; break;
            case "13": deck.usdata_list[compteurItem].score = 13; break;
            case "20": deck.usdata_list[compteurItem].score = 20; break;
            case "40": deck.usdata_list[compteurItem].score = 40; break;
            case "100": deck.usdata_list[compteurItem].score = 100; break;
            case "c": break;
            case "i": break;
            default: break;
        }
        C_DeckFunctions.LogDeck(deck); // Log temporaire pour checker le résultat de l'opération
        // On AutoSave le fichier JSON
        C_DeckFunctions.AutoSaveDeck(deck);
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
