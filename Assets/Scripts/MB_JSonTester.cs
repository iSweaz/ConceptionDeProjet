using TMPro;
using UnityEngine;

/// <summary>
/// MB de test qu'on greffe à un empty game object pour récupérer les boutons et accéder aux fonctions de gestion de json
/// </summary>
public class MB_JSonTester : MonoBehaviour
{
    USJsonFile deck = new USJsonFile();
    [SerializeField] TMP_InputField usTitre;
    [SerializeField] TMP_InputField usDesc;
    [SerializeField] TMP_InputField deckTitre;


    public void CreateItem()
    {
        C_DeckFunctions.CreateItem(usTitre.text, usDesc.text, deck.usdata_list);
        usTitre.text = "";
        usDesc.text = "";
    }

    public void SaveDeck()
    {
        C_DeckFunctions.SaveDeck(deck, deckTitre.text);
        deckTitre.text = "";
    }

    public void LoadDeck()
    {
        deck = C_DeckFunctions.LoadDeck();
    }

    public void LogDeck()
    { 
        C_DeckFunctions.LogDeck(deck);
    }

}
