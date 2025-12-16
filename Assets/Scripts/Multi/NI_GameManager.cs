using System.Collections.Generic;
using PurrNet;
using PurrNet.Transports;
using UnityEngine;
using UnityEngine.UI;

public class NI_GameManager : NetworkIdentity
{
    public NI_Display_Unit blackBoard;
    [SerializeField] private NI_Timer timer; 
    
    public Button nextButton, startButton, checkMark, crossMark,restartButton;
    [SerializeField] private float timeRemaining;
    bool processedCurrent = false;
    bool pickingAnAnswer = false; //Permet de check s'il choisis d'accepter la pause ou pas

    bool notAnswered = false;

    Singleton instance;
    public UDPTransport udp;


    #region Deck
        [Header("Deck")]
        [SerializeField] public USJsonFile deck;

        [SerializeField] private int compteurItem = 0;
        private int lengthDeck = 0;
        [SerializeField] private SyncVar<string> title = new("");
        [SerializeField] private SyncVar<string> description = new("");
    #endregion

    [Header("List")]
    public SyncList<string> answers = new SyncList<string>();    // À changer quand on commencera le multi. Voir si on fait une préfab ou si chaque joueur à sa propre scène et à ce moment pas besoin d'y touché
    public List<NI_Selection> players = new List<NI_Selection>(); //réf players
    protected override void OnSpawned()
    {
        base.OnSpawned(); //Appel de la fonction de base dans le cas d'une intialisation interne
        blackBoard = FindFirstObjectByType<NI_Display_Unit>();
        
        
        if(isServer)
        {
            instance = FindFirstObjectByType<Singleton>();
            timer = GetComponent<NI_Timer>();
            
            //SetUp des infos choisis par l'utilisateur
            if(instance)
            {
                deck = instance.deck;
                if(!instance.revalutate && CheckScore())
                {
                    Debug.Log("+1 à l'init");
                    compteurItem++;
                }

                timeRemaining = instance.time;
                if(udp) udp.maxConnections = instance.numParticipants;
            }
            else //settings sans le singleton pour le debug.
            {
                USData us1 = new USData { titre = "Create procedural mob", desc = "Generate procedural cloth for these mobs" };
                USData us2 = new USData { titre = "Sword style", desc = "implement sword animations" };

                deck.usdata_list.Add(us1);
                deck.usdata_list.Add(us2);  

                timeRemaining = 30;
                if(udp) udp.maxConnections = 2;
            }
            
            lengthDeck = deck.usdata_list.Count;
            
            if(blackBoard == null)
                Debug.Log("Ratio");
            
            if(lengthDeck > 0)
            {
                title.value = deck.usdata_list[compteurItem].titre;
                description.value = deck.usdata_list[compteurItem].desc;
            }
            else
            {
                title.value ="Deck entiérement évalué";
                description.value = "";
            }
        }
        else
        {
            GameObject canvaButton = GetComponentInChildren<Canvas>().gameObject;
            canvaButton.SetActive(false);
        }
        if(blackBoard)
            blackBoard.InitDisplay(title,description);
    }

    [ServerOnly]
    void Update()
    {
        if(!timer) return;
        // On actualise l'état du bouton next selon timeRemaining et l'avancement dans la liste
        startButton.gameObject.SetActive((!timer.timerIsRunning && !processedCurrent && compteurItem < lengthDeck));
        nextButton.gameObject.SetActive((processedCurrent && !timer.timerIsRunning && compteurItem < lengthDeck && !pickingAnAnswer && !notAnswered));
        restartButton.gameObject.SetActive((processedCurrent && !timer.timerIsRunning && compteurItem < lengthDeck && !pickingAnAnswer && notAnswered));
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
        answers.Clear();
        foreach(var player in players)
        {
            answers.Add(player.answer);
        }
       blackBoard.ChangeDisplay("Valeur retenue :",ProcessAnswerByGameMode(answers)); // On applique le script de procession des différents effets possible
    }


    public void onStartButtonClicked()
    {
        timer.startTimer(timeRemaining);
        processedCurrent = true;
    }

    /// <summary>
    /// Fonction qui réagit au clic sur le bouton Next
    /// </summary>
    [Server]
    public void onNextButtonClicked()
    {
        compteurItem++; // on incrémente l'index
        if(instance)
            if(!instance.revalutate && CheckScore())
                compteurItem++; // Permet de skip si déjà évalué
        // On passe à la prochaine task et on reset les variables SSI on a pas encore fait toutes les Story
        if (compteurItem < lengthDeck)
        {
            resetPlayerValue();
            
            title.value = deck.usdata_list[compteurItem].titre;
            description.value = deck.usdata_list[compteurItem].desc;
            blackBoard.ChangeDisplay(title,description); // On actualise l'affichage tableau    
            timer.startTimer(timeRemaining);
        }
        else         // Sinon, fin du process
        {
            processedCurrent = false; // On reset processed

            Debug.Log("Fin du Deck");
            resetPlayerValue();

            title.value ="FIN";
            description.value = "";
            blackBoard.ChangeDisplay(title,description); // On actualise l'affichage tableau
        }
    }

    public void onCheckMarkButtonCliked()
    {
        foreach(var player in players)
            player.DestroyCoffee();
        Destroy(checkMark.gameObject);
        Destroy(crossMark.gameObject);
        blackBoard.ChangeDisplay("Aceptée",""); 


        notAnswered = true;
        pickingAnAnswer = false;
    }

    public void onCrossMarkButtonCliked()
    {
        crossMark.gameObject.SetActive(false);
        checkMark.gameObject.SetActive(false);

        blackBoard.ChangeDisplay("Refusée",""); // On actualise l'affichage tableau

        notAnswered = true;
        pickingAnAnswer = false;

    }

    /// <summary>
    /// Relance la US précédente
    /// </summary>
    public void onRestartButtonCliked()
    {
        notAnswered = false;
        resetPlayerValue();
            
        title.value = deck.usdata_list[compteurItem].titre;
        description.value = deck.usdata_list[compteurItem].desc;
        blackBoard.ChangeDisplay(title,description); // On actualise l'affichage tableau    
        timer.startTimer(timeRemaining);
    }

    /// <summary>
    /// Fonction qui gère le résultat de la carte sélectionnée par l'utilisateur
    /// </summary>
    /// <param name="answer"></param>
    [Server]
    string ProcessAnswerByGameMode(SyncList<string> answers)
    {
        string selectedAnswer = "error";
        Singleton.GameMode gameMode;
        if(answers.Contains("c"))
        {
            selectedAnswer = "Quelqu'un demande une pause";
            crossMark.gameObject.SetActive(true);
            checkMark.gameObject.SetActive(true);
            pickingAnAnswer = true;

            return selectedAnswer;
        }
        else if(answers.Contains("i"))
        {
            deck.usdata_list[compteurItem].score = -1;
            selectedAnswer = "Quelqu'un n'a pas compris la tache";
            notAnswered = true;
            return selectedAnswer;
        }

        if(deck.usdata_list[compteurItem].compteur == 0)
            gameMode = Singleton.GameMode.Unanimity;
        else
            gameMode = instance.mode;

        switch (gameMode)
        {
            case Singleton.GameMode.Unanimity :
                string firstAnswer =  answers[0];
                for(int i = 1; i < answers.Count; i++)
                {
                    if(answers[i] != firstAnswer)
                    {
                        selectedAnswer =  "Pas unanime";
                        notAnswered = true;
                        break;
                    }
                }
                selectedAnswer = firstAnswer;
                deck.usdata_list[compteurItem].score = int.Parse(firstAnswer);
                Debug.Log(deck.usdata_list[compteurItem].titre + " " +compteurItem);
                break;
        }
        deck.usdata_list[compteurItem].compteur++;
        
        C_DeckFunctions.LogDeck(deck); // Log temporaire pour checker le résultat de l'opération
        // On AutoSave le fichier JSON
        C_DeckFunctions.AutoSaveDeck(deck);
        return selectedAnswer;
    }

    /// <summary>
    /// Vérifie l'état du score
    /// </summary>
    /// <returns>Renvoie vrai si le score est différent de celui par défaut</returns>
    bool CheckScore()
    {
        if(deck.usdata_list[compteurItem].score != -1)
            return true;
        return false;
    }

    void OnEnable()
    {
        timer.onTimerStateChanged += OnTimerChanged;
    }
    void OnDisable()
    {
        timer.onTimerStateChanged -= OnTimerChanged; // retrait de listener 
    }

    /// <summary>
    /// Récupère les réponses lorsque le timer est fini 
    /// </summary>
    void OnTimerChanged()
    {
        if(!timer.timerIsRunning && processedCurrent)
            GetAnswers();
    }
}
