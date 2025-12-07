using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
///  Classe qui contient les différentes datas propres aux users stories : titre, desc et score
/// </summary>
[System.Serializable]
public class USData
{
    public string titre;
    public string desc;
    public int score;
}

/// <summary>
/// Classe qui contient les datas du deck json : titre et liste des users stories
/// </summary>
[System.Serializable]
public class USJsonFile
{
    public string title;
    [JsonIgnore] public string filePath; // Path du fichier, non sauvegardé dans le JSON
    public List<USData> usdata_list;

    public USJsonFile()
    {
        title = "Titre";
        usdata_list = new List<USData>();
    }
        
}
