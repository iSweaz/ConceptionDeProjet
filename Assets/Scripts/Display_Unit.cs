using UnityEngine;

using UnityEditor;
using TMPro;

public class Display_Unit : MonoBehaviour
{
    private TMP_Text nameText,descText;

    [SerializeField] public UserStorys deck;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
      nameText = texts[0];
      descText = texts[1];
    }

    /// <summary>
    /// Actualise le contenu affiché au tableau
    /// </summary>
    /// <param name="compteur">index de la user story dans le deck</param>
    public void ChangeDisplay(int compteur)
    {
      nameText.text = deck.US[compteur].name;
      descText.text = deck.US[compteur].description;
    }
}
