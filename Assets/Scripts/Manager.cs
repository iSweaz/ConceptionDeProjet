using PurrNet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe qui manage la scène meeting room
/// </summary>
public class Manager : MonoBehaviour
{
    public USJsonFile deck;
    public Button nextButton, startButton;
    public Display_Unit blackBoard;
    private Timer timer;
    Singleton instance;
    bool processedCurrent = false;

    public float timeRemaining = 5;

    private int compteurItem = 0;
    private int lengthDeck = 0;

    private Selection player; // À changer quand on commencera le multi. Voir si on fait une préfab ou si chaque joueur à sa propre scène et à ce moment pas besoin d'y touché


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Manager Start");
        // On récupère une référence vers le Singleton ainsi que le deck
        instance = Singleton.instance;
        deck = instance.deck;

        lengthDeck = deck.usdata_list.Count();
        
        blackBoard = FindFirstObjectByType<Display_Unit>();
        blackBoard.deck = deck;
        blackBoard.ChangeDisplay(compteurItem);

        timer = FindFirstObjectByType<Timer>();
        timeRemaining = instance.time;
        //timer.startTimer(timeRemaining);

        player = FindFirstObjectByType<Selection>();
    }

    // Update is called once per frame
    void Update()
    {
        // On actualise l'état du bouton next selon timeRemaining et l'avancement dans la liste
        startButton.gameObject.SetActive((!timer.timerIsRunning && !processedCurrent && compteurItem < deck.usdata_list.Count()));
        nextButton.gameObject.SetActive((processedCurrent && !timer.timerIsRunning && compteurItem < deck.usdata_list.Count()));
        //nextButton.interactable = (!timer.timerIsRunning && compteurItem + 1 < deck.usdata_list.Count());
    }
    /// <summary>
    /// Fonction qui réagit au clic sur le bouton Start
    /// </summary>
    public void onStartButtonClicked()
    {
        timer.startTimer(timeRemaining);
        processedCurrent = true;
    }


    /// <summary>
    /// Fonction qui réagit au clic sur le bouton Next
    /// </summary>
    public void onNextButtonClicked()
    {
        // On process l'answer
        ProcessAnswer(player.answer); // On applique le script de procession des différents effets possible

        processedCurrent = false; // On reset processed

        // On passe à la prochaine task et on reset les variables SSI on a pas encore fait toutes les Story
        if (compteurItem < deck.usdata_list.Count())
        {
            compteurItem++; // on incrémente l'index
            player.ChangeColor(null); // On réinitialise l'état de toutes les cartes
            player.answer = "i"; // On remet player.answer à sa valeur par défaut
            blackBoard.ChangeDisplay(compteurItem); // On actualise l'affichage tableau
        }
        // Sinon, fin du process
        else
        {
            player.ChangeColor(null); // On réinitialise l'état de toutes les cartes
            player.answer = "i"; // On remet player.answer à sa valeur par défaut
            blackBoard.ChangeDisplay(compteurItem); // On actualise l'affichage tableau
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameMode"></param>
    /// <param name="answers"></param>
    /// <returns></returns>
    public static string ProcessAnswerByGameMode(Singleton.GameMode gameMode, List<string> answers)
    {
        if (answers.Contains("c"))
        {
            return "Quelqu'un demande une pause";
        }
        else if (answers.Contains("i"))
        {
            return "Quelqu'un n'a pas compris la tache";
        }

        switch (gameMode)
        {
            case Singleton.GameMode.Unanimity: return ProcessGameMode_Unanimity(answers);
            case Singleton.GameMode.Average: return ProcessGameMode_Average(answers);
            case Singleton.GameMode.Median: return ProcessGameMode_Median(answers);
            default: return "Erreur : GameMode non pris en charge.";
        }
    }

    #region Fonctions propres à chaque gameMode

    /// <summary>
    /// Fonction qui process la liste des réponses si le game mode est set à "Unanimity"
    /// </summary>
    /// <param name="answers"></param>
    /// <returns></returns>
    public static string ProcessGameMode_Unanimity(List<string> answers)
    {
        string firstAnswer = answers[0];
        for (int i = 1; i < answers.Count; i++)
        {
            if (answers[i] != firstAnswer)
            {
                return "Pas unanime";
            }
        }
        return firstAnswer;
    }

    /// <summary>
    /// Fonction qui process la liste des réponses si le game mode est set à "Average"
    /// </summary>
    /// <param name="answers"></param>
    /// <returns></returns>
    public static string ProcessGameMode_Average(List<string> answers)
    {
        float result = 0.0f;
        for (int i = 0; i < answers.Count; i++)
        {
            result += float.Parse(answers[i]);
        }
        result /= answers.Count;
        return result.ToString();
    }

    /// <summary>
    /// Fonction qui process la liste des réponses si le game mode est set à "Median"
    /// </summary>
    /// <param name="answers"></param>
    /// <returns></returns>
    public static string ProcessGameMode_Median(List<string> answers)
    {
        List<float> values = new List<float>();
        for (int i = 0; i < answers.Count; i++)
        {
            values.Add(float.Parse(answers[i]));
        }
        values.Sort();

        float median;
        int n = values.Count;

        if (n % 2 == 1)
            median = values[n / 2];
        else
            median = (values[n / 2 - 1] + values[n / 2]) / 2f;

        return median.ToString();
    }

    #endregion

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
}