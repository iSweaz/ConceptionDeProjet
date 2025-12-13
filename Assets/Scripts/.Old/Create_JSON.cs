using System.ComponentModel;
using System.IO;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Create_JSON : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Button")]
    [SerializeField] private Button cancel;
    [SerializeField] private Button save;
    [SerializeField] private Button create;

    [Header("Previous name Scene")]
    [SerializeField] private string scene;

    [Header("Prefab item")]
    [SerializeField] private GameObject prefab;

    private GameObject contentHandler;

    [Header("Fomulaire")]
    [SerializeField] GameObject display;
    private Button close,confirm;
    private TMP_InputField nameUS, descrptionUS;
    private bool displayIsActive = false;

    private GameObject lastCreated;
    private deck.UserStorys USList;
   


    void Start()
    {
        // TMP_Text text = GetComponent<TMP_Text>();
        // text.text = Application.dataPath;

        contentHandler = GameObject.Find("Content");

        Button[] buttons = display.GetComponentsInChildren<Button>();
        close = buttons[0];
        confirm = buttons[1];

        TMP_InputField[] inputs = display.GetComponentsInChildren<TMP_InputField>();
        nameUS = inputs[0];
        descrptionUS = inputs[1];

        USList = new deck.UserStorys();

        cancel.onClick.AddListener(CancelDeck);
        save.onClick.AddListener(Save);
        create.onClick.AddListener(Create);;
        confirm.onClick.AddListener(form);
        close.onClick.AddListener(Close);
    }

     /// <summary>
    /// Renvoie à la scène précedente dont le nom est indiquer.
    /// </summary>
    void CancelDeck()
    {
        SceneManager.LoadScene(scene);
    }

    void Save()
    {
        string deckName = "Deck";
        TMP_InputField DeckNameInput = GameObject.Find("DeckName").GetComponent<TMP_InputField>();
        if (DeckNameInput.text != "")
            deckName = DeckNameInput.text;

        #if UNITY_EDITOR
        string path = Application.dataPath + "/Decks/";
        #else
        string path = Path.GetDirectoryName(Application.dataPath) + "/Decks/";
        #endif

        // Vérifie si le dossier existe
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    
        string json = JsonUtility.ToJson(USList,true);
        File.WriteAllText(path + deckName + ".json", json);

        Debug.Log(path);
    }

    void Create()
    {
        nameUS.text = "";
        descrptionUS.text = "";

        lastCreated = Instantiate(prefab, contentHandler.transform);
        displayIsActive = !displayIsActive;
        display.SetActive(displayIsActive);
    }

    void form()
    {
        if (lastCreated)
        {
            //set new texts values in the card
            lastCreated.name = nameUS.text;
            TMP_Text[] texts = lastCreated.GetComponentsInChildren<TMP_Text>();
            texts[0].text = nameUS.text;
            texts[1].text = descrptionUS.text;

            //add card to the deck
            deck.userStoryData newItem = new deck.userStoryData();
            newItem.name = nameUS.text;
            newItem.description = descrptionUS.text;
            USList.US.Add(newItem);

            displayIsActive = !displayIsActive;
            display.SetActive(displayIsActive);
            lastCreated = null;

            //actualize the content size to the new text
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentHandler.GetComponent<RectTransform>());
        }
    }

    void Close()
    {
        displayIsActive = !displayIsActive;
        Destroy(lastCreated);
        display.SetActive(displayIsActive);
        nameUS.text = "";
        descrptionUS.text = "";
    }
}
