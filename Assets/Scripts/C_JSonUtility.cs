using Newtonsoft.Json;
using System.IO;
using UnityEngine;

/// <summary>
/// Classe qui contient les fonctions de gestion des fichiers JSON
/// </summary>
public static class C_JSonUtility
{
    /// <summary>
    /// Fonction de sauvegarde des fichiers JSON
    /// </summary>
    /// <param name="usList">Liste qui contient l'ensemble des users stories</param>
    /// <param name="path">Chemin de sauvegarde du fichier JSON</param>
    public static void SaveDeck(USJsonFile deck, string path)
    {
        string json = JsonConvert.SerializeObject(deck, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);
        deck.filePath = path;
    }

    /// <summary>
    /// Fonction de chargement des fichiers JSON
    /// </summary>
    /// <param name="path">Chemin de chargement du fichier JSON</param>
    /// <returns>Deck chargé ou null en cas de problème</returns>
    public static USJsonFile LoadDeck(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Fichier JSON introuvable : " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        USJsonFile deck = JsonConvert.DeserializeObject<USJsonFile>(json);
        deck.filePath = path; // On set le filePath
        return deck;
    }
}
