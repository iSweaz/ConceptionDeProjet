using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe qui g�re la sc�ne de cr�ation/modification de deck 
/// </summary>
public class MB_CreateDeckManager : MonoBehaviour
{
    Singleton instance;
    public GameObject contentFitter;
    public Button prefabUs;
    [Header("Champs de cr�ation/�dition")]
    public TMP_Text headerText;
    public Button deleteButton;
    public Button editButton;
    public Button applyButton;
    public TMP_Text applyText;
    public Button cancelButton;
    public TMP_InputField titleInput;
    public TMP_InputField descInput;
    [Header("Menu retour")]
    public Button saveAsButton;
    public Button backButton;
    public TMP_InputField deckTitle;
    [Header("Autre")]
    public string menuScene;
    int selectedUS = -1; // US actuellement s�lectionn�e, d�fault � -1
    byte newStoryMode = 1;  // 1 == create, 2 == modify
    //public TMP_Text deckPreview;

    void Start()
    {
        instance = Singleton.instance; // On r�cup�re l'instance active du Singleton
        //if (instance.deck != null) deckPreview.text = instance.deck.FormatDeckToString();
        if (instance.deck != null)
        {
            deckTitle.text = instance.deck.title;
            UpdateDeckPreview();
        }
        cancelButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        applyButton.interactable = !(titleInput.text == string.Empty || descInput.text == string.Empty); // On update applyButton selon l'�tat des champs
        deleteButton.interactable = (selectedUS != -1); // On update deleteButton selon l'�tat de selectedUS
        editButton.interactable = (selectedUS != -1 && newStoryMode == 1); // On update editButton selon l'�tat de selectedUS
        if (newStoryMode == 1) applyText.text = "Create story";
        else applyText.text = "Apply changes";
        if (instance.deck.usdata_list.Count >= 1) saveAsButton.interactable = true;
        else saveAsButton.interactable = false;
    }

    /// <summary>
    /// Fonction qui update la preview de deck dans deck modify/create
    /// </summary>
    void UpdateDeckPreview()
    {
        // On clear enti�rement le contentFitter
        foreach (Transform child in contentFitter.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // On le remplit avec le contenu de deck
        List<USData> usdata_list = instance.deck.usdata_list;
        for (int i = 0; i < usdata_list.Count; i++)
        {
            int index = i; // COPIE LOCALE
            string _txtNext = instance.deck.FormatUstoryToString(i);
            Button newButton = CreateUsButton(_txtNext);
            newButton.onClick.AddListener(() => OnUsClicked(index));
        }
    }

    /// <summary>
    /// Cr�er et retourne un nouveau UsButton
    /// </summary>
    /// <param name="_texte"></param>
    /// <returns></returns>
    Button CreateUsButton(string _texte)
    {
        // Instancie le prefab
        Button newButton = Instantiate(prefabUs);

        // D�finit le parent
        newButton.transform.SetParent(contentFitter.transform, false);

        //  Modifie le texte du bouton
        TMP_Text tmpText = newButton.GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = _texte;
        }
        return newButton;
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur une UsStory
    /// </summary>
    /// <param name="index"></param>
    public void OnUsClicked(int index)
    {
        selectedUS = index;
        if (newStoryMode == 2)
        {
            titleInput.text = instance.deck.usdata_list[selectedUS].titre;
            descInput.text = instance.deck.usdata_list[selectedUS].desc;
        }
        Debug.Log(selectedUS);
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur "Edit"
    /// </summary>
    public void OnEditClicked()
    {
        Debug.Log(selectedUS);
        cancelButton.gameObject.SetActive(true);
        headerText.text = "Edit story";
        newStoryMode = 2; // Edit mode
        titleInput.text = instance.deck.usdata_list[selectedUS].titre;
        descInput.text = instance.deck.usdata_list[selectedUS].desc;
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur "Cancel"
    /// </summary>
    public void OnCancelClicked()
    {
        cancelButton.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        headerText.text = "New story";
        newStoryMode = 1; // Edit mode
        titleInput.text = "";
        descInput.text = "";
        selectedUS = -1;
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur "Apply"
    /// </summary>
    public void OnApplyClicked()
    {
        // Cas ou l'on est en mode New Story
        if (newStoryMode == 1)
        {
            USData newItem = new USData();  
            newItem.titre = titleInput.text;
            newItem.desc = descInput.text;
            newItem.score = -1;
            newItem.compteur = 0;
            instance.deck.usdata_list.Add(newItem);
            titleInput.text = "";
            descInput.text = "";
            UpdateDeckPreview();
        }
        // Cas ou l'on est en monde Edit Story
        else
        {
            instance.deck.usdata_list[selectedUS].titre = titleInput.text;
            instance.deck.usdata_list[selectedUS].desc = descInput.text;
            cancelButton.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            headerText.text = "New story";
            newStoryMode = 1; // Edit mode
            titleInput.text = "";
            descInput.text = "";
            UpdateDeckPreview();
        }
        EventSystem.current.SetSelectedGameObject(null);
        selectedUS = -1;
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur "Save As"
    /// </summary>
    public void OnSaveAsClicked()
    {
        if (deckTitle.text != string.Empty) instance.deck.title = deckTitle.text;
        C_DeckFunctions.SaveDeck(instance.deck, instance.deck.title);
    }

    /// <summary>
    /// Fonction qui r�agit au clic sur "Back to main menu"
    /// </summary>
    public void OnBackClicked()
    {
        if (deckTitle.text != string.Empty) instance.deck.title = deckTitle.text;
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Fonction qui delete la story � l'index selectedUS
    /// </summary>
    public void DeleteStory()
    {
        if (selectedUS != -1 && selectedUS < instance.deck.usdata_list.Count)
        {
            instance.deck.usdata_list.RemoveAt(selectedUS);
            EventSystem.current.SetSelectedGameObject(null);
            selectedUS = -1;
            UpdateDeckPreview();
        }
    }
}
