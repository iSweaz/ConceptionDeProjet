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
    private userStoryData[] datas;

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
        Debug.Log(datas);
    }

    /// <summary>
    /// Permet de choisir le fichier JSON à inspecter.
    /// </summary>
    /// <param name="path"> Chemin du fichier JSON pour importer le deck</param>
    /// <returns>Retourne le contenue du deck.</returns>
    protected userStoryData[] SetDeck(string path)
    {
        if (path.Length == 0)
            return null;
        UserStorys uSList = JsonUtility.FromJson<UserStorys>(File.ReadAllText(path)); //File.ReadAllText() is necesseary beaucause i give a path rather than a file 
        userStoryData[] datas = new userStoryData[uSList.deck.Length];

        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = new userStoryData
            {
                name = uSList.deck[i].name,
                description = uSList.deck[i].description
            };
        }
        return datas;
    }

    public userStoryData[] GetDeck()
    {
        return datas;
    }
}
