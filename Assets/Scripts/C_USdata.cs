using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
///  Classe qui contient les diff�rentes datas propres aux users stories : titre, desc et score
/// </summary>
[System.Serializable]
public class USData
{
    public string titre;
    public string desc;
    public float score;
    public int compteur;
}

/// <summary>
/// Classe qui contient les datas du deck json : titre et liste des users stories
/// </summary>
[System.Serializable]
public class USJsonFile
{
    public string title;
    [JsonIgnore] public string filePath; // Path du fichier, non sauvegard� dans le JSON
    public List<USData> usdata_list;

    public USJsonFile()
    {
        title = "Untitled";
        usdata_list = new List<USData>();
    }

    /// <summary>
    /// Fonction qui permet de formatter un deck en string
    /// </summary>
    /// <param name="deck"></param>
    /// <returns></returns>
    public string FormatDeckToString()
    {
        string deckPreview = "";
        for (int i = 0; i < usdata_list.Count; i++)
        {
            USData next = usdata_list[i];
            // On le set le nombre
            string num = i.ToString();
            if (i < 10) num = "0" + num;
            num = "[" + num + "] ";
            // On set le titre
            deckPreview += "<u>" + num + next.titre + "</u>\n";
            // On set la description -> couleur A8A9FF
            deckPreview += "<color=#A8A9FF>" + next.desc + "</color>\n\n";
        }
        return deckPreview;
    }

    /// <summary>
    /// Fonction qui formatte une US Story en string
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string FormatUstoryToString(int index)
    {
        string usPreview = "";
        USData next = usdata_list[index];
        // On le set le nombre
        string num = index.ToString();
        if (index < 10) num = "0" + num;
        num = "[" + num + "] ";
        // On set le titre
        usPreview += num + next.titre + "\n";
        // On set la description -> couleur A8A9FF
        usPreview += "<color=#A8A9FF>" + next.desc + "</color>";
        return usPreview;
    }
        
}
