using UnityEngine;
using TMPro;

public class Display_Unit : MonoBehaviour
{
    public TMP_Text nameText,descText;

    //[SerializeField] public UserStorys deck;
    public USJsonFile deck;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    /// <summary>
    /// Actualise le contenu affiché au tableau
    /// </summary>
    /// <param name="compteur">index de la user story dans le deck</param>
    public void ChangeDisplay(int compteur)
    {
        if (compteur < deck.usdata_list.Count)
        {
            nameText.text = deck.usdata_list[compteur].titre;
            descText.text = deck.usdata_list[compteur].desc;
        }
        else
        {
            nameText.text = "FIN";
            descText.text = "";
        }
    }
}
