using System.Collections.Generic;
using PurrNet;
using UnityEngine;


public class NI_GameManager : NetworkIdentity
{
    public NI_Display_Unit blackBoard;
    [SerializeField] private NI_Timer timer; 

    [SerializeField] private float timeRemaining = 30;

    #region Deck
        [Header("Deck")]
        [SerializeField] public USJsonFile deck;

        [SerializeField] private int compteurItem = 0;
        private int lengthDeck = 0;
        [SerializeField] private SyncVar<string> title = new("");
        [SerializeField] private SyncVar<string> description = new("");
    #endregion

    [Header("List")]
    public SyncList<string> Answers = new SyncList<string>();    // À changer quand on commencera le multi. Voir si on fait une préfab ou si chaque joueur à sa propre scène et à ce moment pas besoin d'y touché
    public List<NI_Selection> players = new List<NI_Selection>(); //réf players
    protected override void OnSpawned()
    {
        base.OnSpawned(); //Appel de la fonction de base dans le cas d'une intialisation interne
        blackBoard = FindFirstObjectByType<NI_Display_Unit>();
        
        if(isServer)
        {

            timer = GetComponent<NI_Timer>();
            timer.timeRemaining.value = timeRemaining;
            timer.timerIsRunning = true;

            deck = new USJsonFile();

            USData us1 = new USData { titre = "Create procedural mob", desc = "Generate procedural cloth for these mobs" };
            USData us2 = new USData { titre = "Sword style", desc = "implement sword animations" };

            deck.usdata_list.Add(us1);
            deck.usdata_list.Add(us2);

            lengthDeck = deck.usdata_list.Count;
            
            if(blackBoard == null)
                Debug.Log("Ratio");

            title.value = deck.usdata_list[compteurItem].titre;
            description.value = deck.usdata_list[compteurItem].desc;
            
        }
        if(blackBoard)
            blackBoard.InitDisplay(title,description);
    }

    [ServerOnly]
    void Update()
    {
        if(timer == null) return; // Ignore Update tant que timer n'est pas assigné

        if(timer.timerIsRunning)
        {       
            //Probablement à moove sur le singleton pour la com serveur, par encore réfléchit à ça
            if(compteurItem + 1 < lengthDeck)
            {
                if(timer.timeRemaining == 0)
                {
                    timer.timeRemaining.value = timeRemaining;
                    timer.timerIsRunning = true;

                    compteurItem++;              

                    title.value = deck.usdata_list[compteurItem].titre;
                    description.value = deck.usdata_list[compteurItem].desc;
                    blackBoard.ChangeDisplay(title,description); // à retirer, présent pour check le fonctionnement du changement de US

                    GetAnswers();
                    resetPlayerValue();
                }
            }
            else if(timer.timeRemaining == 0)
            {
                timer.DisplayTime(0);
                Time.timeScale = 0;   
                timer.timerIsRunning =false;

                GetAnswers();
                // resetPlayerValue();
            }
        }
    }

    [Server]
    void resetPlayerValue()
    {
        foreach(var player in players)
        {
            player.resetValue();
        }
    }

    void GetAnswers()
    {
        Answers.Clear();
        foreach(var player in players)
        {
            Answers.Add(player.playerAnswer);
        }
    }
}
