using UnityEngine;

using UnityEditor;
using TMPro;

public class Display_Unit : MonoBehaviour
{
    private TMP_Text nameText,descText;
    [SerializeField]
    private JSON_Manager json;
    [SerializeField]
    public UserStorys deck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
      nameText = texts[0];
      descText = texts[1];

      
  
    }

    // Update is called once per frame
    void Update()
    {
      //temp the deck will be choose or create before the meeting room
      if (deck == null)
      {
        deck = json.GetDeck();
        Debug.Log(JsonUtility.ToJson(deck, true));
      }
        
        
    }
}
