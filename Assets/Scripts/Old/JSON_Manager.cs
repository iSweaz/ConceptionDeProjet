using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

//Plugin
using SimpleFileBrowser;
using System.Collections;

public class JSON_Manager : MonoBehaviour
{
    // public TextAsset textFile;
    private deck deck;
    private Button button;
    public deck.UserStorys US;
    
    private bool isDialogOpen = false;

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

#if UNITY_EDITOR_WIN
        // string[] filters = { "JSON files", "json" };
        // string path = EditorUtility.OpenFilePanelWithFilters("Choose a deck", "", filters);
        // datas = SetDeck(path);
        // Debug.Log(path);
        if(!isDialogOpen)
            StartCoroutine(ShowLoadDialogCoroutine());

#else
        StartCoroutine(ShowLoadDialogCoroutine());
#endif
    }
    
    private IEnumerator ShowLoadDialogCoroutine()
    {
        isDialogOpen = true;
        FileBrowser.SetFilters(false, new FileBrowser.Filter("JSON files", ".json"));
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Choose a deck", "Load");

        if (FileBrowser.Success)
        {
            string path = FileBrowser.Result[0].Trim().Replace("\\", "/");
            Debug.Log(path);
            string json = File.ReadAllText(path);
            Debug.Log("JSON Content:\n" + json);

            if (File.Exists(path))
            {
                US = SetDeck(path);
                Debug.Log("JSON loaded successfully");
            }
            else
            {
                Debug.LogError("File not found: " + path);
            }
        }
        isDialogOpen = false;
    }


    /// <summary>
    /// Permet de charger un deck depuis un fichier JSON.
    /// </summary>
    /// <param name="path">Chemin du fichier JSON.</param>
    /// <returns>Tableau des userStoryData contenus dans le deck.</returns>
    protected deck.UserStorys SetDeck(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<deck.UserStorys>(json);
    }

    public deck.UserStorys GetDeck()
    {
        return US;
    }
}
