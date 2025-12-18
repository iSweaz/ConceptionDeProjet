using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// Mono-behavior qui g�re l'�cran de cr�ation de session
/// </summary>
public class Create_Session : MonoBehaviour
{
    private string debugHeader = "[Create_Session] ";
    [SerializeField]   private Button createSessionButton;
    [SerializeField]   private Button createDeckButton;
    [SerializeField]   private Button modifyDeckButton;
    [SerializeField]   private Button importDeckButton;
    [SerializeField]   private TMP_Dropdown mode;
    public TMP_InputField pseudo,sizeSession,time;
    [SerializeField]   private TMP_Text TMPdeckPreview;
    [SerializeField]   private TMP_Text TMPdeckTitle;
    [SerializeField]   private Toggle toggle;
    

    [Header("Secene to load")]
    [SerializeField]   private string scene = "Meeting_Room";
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
        instance.playersName.Add(pseudo.text);
        Debug.Log(toggle.isOn);
        instance.revalutate = toggle.isOn;

        SceneManager.LoadScene(scene);
    }

    public void LoadOld()
    {
        instance.numParticipants = int.Parse(sizeSession.text);
        instance.time = float.Parse(time.text);
        SetModeValue();
        instance.playersName.Add(pseudo.text);
        instance.revalutate = toggle.isOn;

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

    #region Fonctions r�actives aux buttons (voir dans l'inspector les �v�nements OnClick() des buttons)

    /// <summary>
    /// Fonction d�clench�e quand l'utilisateur clique sur le button "Create Deck". Cr�e un fichier JSon � l'endroit choisi.
    /// </summary>
    public void CreateDeck()
    {
        USJsonFile newDeck = new USJsonFile();
        instance.deck = newDeck;
        SceneManager.LoadScene("DeckCreation");
    }

    /// <summary>
    /// Fonction d�clench�e quand l'utilisateur clique sur Modify deck. Ouvre la sc�ne de gestion de deck.
    /// </summary>
    public void ModifyDeck()
    {
        SceneManager.LoadScene("DeckCreation");
    }
    /// <summary>
    /// Fonction d�clench�e lorsque l'utilisateur clique sur le button "Import Deck". Charge un .JSON, actualise la preview de deck et passe les donn�es charg�es au Singleton.
    /// </summary>
    public void ImportDeck()
    {
        USJsonFile importedDeck = C_DeckFunctions.LoadDeck();
        if (importedDeck != null)
        {
            instance.deck = importedDeck; // On passe le deck charg� au singleton
            C_DeckFunctions.LogDeck(importedDeck);
            // On actualise la preview de deck
            TMPdeckPreview.text = instance.deck.FormatDeckToString();
            TMPdeckTitle.text = instance.deck.title;
            modifyDeckButton.interactable = true;
            CheckUSState();
        }
        else Debug.Log(debugHeader + "Importation d'un deck annul�e");
        
    }

    #endregion
    void CheckUSState()
    {
        foreach(var US in instance.deck.usdata_list)
        {
            if(US.score != -1)
            {
                toggle.gameObject.SetActive(true);
                return;
            }
        }
    }
}
