using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Mono-behavior qui gère l'écran de création de session
/// </summary>
public class Create_Session : MonoBehaviour
{
    private string debugHeader = "[Create_Session] ";
    public Button createSessionButton;
    public Button createDeckButton;
    public Button modifyDeckButton;
    public Button importDeckButton;
    public TMP_Dropdown mode;
    public TMP_InputField pseudo,sizeSession,time;
    public TMP_Text TMPdeckPreview;
    public TMP_Text TMPdeckTitle;
    private Singleton instance;

    void Start()
    {
        instance = Singleton.instance;
        if (instance.deck != null)
        {
            modifyDeckButton.interactable = true;
            // On actualise la preview de deck
            TMPdeckPreview.text = instance.deck.FormatDeckToString();
            TMPdeckTitle.text = instance.deck.title;
        }
    }

    private void Update()
    {
        createSessionButton.interactable = (instance.deck != null && instance.deck.usdata_list.Count > 0 && 
            pseudo.text != string.Empty && sizeSession.text != string.Empty && time.text != string.Empty);
    }

    /// <summary>
    /// Fonction qui permet de charger la meeting room
    /// </summary>
    public void Load()
    {
        instance.numParticipants = int.Parse(sizeSession.text);
        instance.time =  float.Parse(time.text);
        SetModeValue();
        instance.Players.Add(pseudo.text);

        Debug.Log(instance.time);
        Debug.Log(instance.numParticipants);
        Debug.Log(instance.mode);
        Debug.Log(instance.Players[0]);

        SceneManager.LoadScene("Meeting_Room");
    }

    void SetModeValue()
    {
        switch(mode.value)
        {
            case 0:
                instance.mode = Singleton.GameMode.Unanimity;
                break;

            case 1:
                instance.mode = Singleton.GameMode.Average;
                break;

            case 2:
                instance.mode = Singleton.GameMode.Median;
                break;

            case 3:
                instance.mode = Singleton.GameMode.AbsMajority;
                break;

            case 4:
                instance.mode = Singleton.GameMode.RelMajority;
                break;

            default:
                instance.mode = Singleton.GameMode.Unanimity;
                break;
        }
    }

    #region Fonctions réactives aux buttons (voir dans l'inspector les évènements OnClick() des buttons)

    /// <summary>
    /// Fonction déclenchée quand l'utilisateur clique sur le button "Create Deck". Crée un fichier JSon à l'endroit choisi.
    /// </summary>
    public void CreateDeck()
    {
        USJsonFile newDeck = new USJsonFile();
        instance.deck = newDeck;
        SceneManager.LoadScene("DeckCreation");
    }

    /// <summary>
    /// Fonction déclenchée quand l'utilisateur clique sur Modify deck. Ouvre la scène de gestion de deck.
    /// </summary>
    public void ModifyDeck()
    {
        SceneManager.LoadScene("DeckCreation");
    }
    /// <summary>
    /// Fonction déclenchée lorsque l'utilisateur clique sur le button "Import Deck". Charge un .JSON, actualise la preview de deck et passe les données chargées au Singleton.
    /// </summary>
    public void ImportDeck()
    {
        USJsonFile importedDeck = C_DeckFunctions.LoadDeck();
        if (importedDeck != null)
        {
            instance.deck = importedDeck; // On passe le deck chargé au singleton
            C_DeckFunctions.LogDeck(importedDeck);
            // On actualise la preview de deck
            TMPdeckPreview.text = instance.deck.FormatDeckToString();
            TMPdeckTitle.text = instance.deck.title;
            modifyDeckButton.interactable = true;
        }
        else Debug.Log(debugHeader + "Importation d'un deck annulée");
    }

    #endregion

}
