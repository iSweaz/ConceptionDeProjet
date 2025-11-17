using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using UnityEditor;


[System.Serializable]
public class userStoryData
{
    public string name;
    public string description;
}

[System.Serializable]
public class UserStorys
{
    public userStoryData[] deck;
}

public class JSON_Manager : MonoBehaviour
{
    // public TextAsset textFile;
    private Button button;
    [SerializeField]
    private UserStorys datas;

    [SerializeField]
    private Manager gameManager;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenFile);

    }

    /// <summary>
    /// Permet de choisir le fichier JSON à inspecter
    /// </summary>
    private void OpenFile()
    {
        string[] filters = { "JSON files", "json" };
        string path = EditorUtility.OpenFilePanelWithFilters("Choose a deck", "", filters);
        datas = SetDeck(path);

        gameManager.deck = datas; 
        GameObject Parent = transform.parent.gameObject;
        Destroy(Parent);
    }

    /// <summary>
    /// Permet de choisir le fichier JSON à inspecter.
    /// </summary>
    /// <param name="path"> Chemin du fichier JSON pour importer le deck</param>
    /// <returns>Retourne le contenue du deck.</returns>
    protected UserStorys SetDeck(string path)
    {
        if (path.Length == 0)
            return null;
        UserStorys uSList = JsonUtility.FromJson<UserStorys>(File.ReadAllText(path)); //File.ReadAllText() is necesseary beaucause i give a path rather than a file 
        userStoryData[] deck = new userStoryData[uSList.deck.Length];

        for (int i = 0; i < deck.Length; i++)
        {
            deck[i] = new userStoryData
            {
                name = uSList.deck[i].name,
                description = uSList.deck[i].description
            };
        }
        uSList.deck = deck;
        return uSList;
    }

    public UserStorys GetDeck()
    {
        Debug.Log("GetDeck");
        if(datas != null)
        {
            //temp
            GameObject parent = transform.parent.gameObject;
            parent.SetActive(false);
            
            return datas;
        }
        return null;
    }
}
