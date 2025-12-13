using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using UnityEditor;
using Unity.VisualScripting;
using PurrNet;


[System.Serializable]
public class userStoryData
{
    public string name;
    public string description;
}

[System.Serializable]
public class UserStorys
{
    public userStoryData[] US;
}

public class JSON_Manager : MonoBehaviour
{
    // public TextAsset textFile;
    private Button button;
    [SerializeField]
    private UserStorys datas;

    private NI_GameManager gameManager;

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

        SendToManager();
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
        userStoryData[] deck = new userStoryData[uSList.US.Length];

        for (int i = 0; i < deck.Length; i++)
        {
            deck[i] = new userStoryData
            {
                name = uSList.US[i].name,
                description = uSList.US[i].description
            };
        }
        uSList.US = deck;
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

    /// <summary>
    /// Fonction temporaire en attendant la session ? 
    /// </summary>
    public void SendToManager()
    {
        gameManager = FindFirstObjectByType<NI_GameManager>();

        //gameManager.lengthDeck = datas.US.Length;
        // gameManager.deck = datas;
        GameObject Parent = transform.parent.gameObject;
        Destroy(Parent);
    }

}
