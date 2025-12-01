using SFB;

/// <summary>
/// Classe qui permet de gérer la récupération de path sur l'OS à l'aide de SFB (StandaloneFileBrowser)
/// <param name="defaultName">Nom par défaut du fichier JSon (facultatif)</param>
/// </summary>
public static class C_PathManager
{
    /// <summary>
    /// Fonction avec la library SFB : ouvre une fenêtre pour choisir l'emplacement de sauvegarde du deck
    /// </summary>
    /// <param name="defaultName">Nom du deck (par défaut : Deck)</param>
    /// <returns>Chemin du fichier</returns>
    public static string SaveFindPath(string defaultName = "Deck")
    {
        return StandaloneFileBrowser.SaveFilePanel("Save File", "", defaultName, "json");
    }

    /// <summary>
    /// Fonction avec la library SFB : ouvre une fenêtre pour choisir l'emplacement de load du deck
    /// </summary>
    /// <returns>Chemin du fichier</returns>
    public static string LoadFindPath()
    { 
        return StandaloneFileBrowser.OpenFilePanel("Open file", "", "json", false)[0];
    }
}
