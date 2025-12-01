using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui contient les fonctions de gestion du deck
/// </summary>
public static class C_DeckFunctions
{
    /// <summary>
    /// Fonction qui crée une user story et l'ajoute à la liste des users stories
    /// </summary>
    /// <param name="title">Titre de la user story</param>
    /// <param name="desc">Description de la user story (non obligatoire)</param>
    /// <param name="us_list">Liste qui contient les users stories</param>
    public static void CreateItem(string title, string desc, List<USData> us_list)
    {
        if (CheckTitle(title, us_list))
        {
            USData newUS = new USData();
            newUS.titre = title;
            newUS.desc = desc;
            newUS.score = -1;
            us_list.Add(newUS);
        }
    }

    /// <summary>
    /// Fonction qui sauvegarde le deck en .json, avec le titre du deck et la liste contenant les users stories
    /// </summary>
    /// <param name="deck">Type avec titre du deck + la liste qui contient les users stories</param>
    /// <param name="deckTitle">Titre du deck ("Titre" par défaut s'il n'est pas renseigné)</param>
    public static void SaveDeck(USJsonFile deck, string deckTitle)
    {
        if (deckTitle != "")
        {
            deck.title = deckTitle;
            string path = C_PathManager.SaveFindPath(deckTitle);
            if (path != "")
            {
                C_JSonUtility.SaveDeck(deck, path);
            }
        }
        else
        {
            Debug.Log("Il faut renseigner un titre pour le deck");
        }
    }

    /// <summary>
    /// Fonction qui charge le deck à partir d'un .json
    /// </summary>
    /// <returns>Deck chargé ou null en cas de problème</returns>
    public static USJsonFile LoadDeck()
    {
        string path = C_PathManager.LoadFindPath();
        if (path == "")
        {
            return null;
        }
        else
        {
            return C_JSonUtility.LoadDeck(path);
        }
    }

    /// <summary>
    /// Fonction qui vérifie le titre (non vide et pas déjà utilisé)
    /// </summary>
    /// <param name="title">Titre de la user story</param>
    /// <param name="us_list">Liste qui contient les users stories</param>
    /// <returns>T/F si le titre est valide ou non</returns>
    static bool CheckTitle(string title, List<USData> us_list)
    {
        if (title != "")
        {
            for (int i = 0; i < us_list.Count; i++)
            {
                if (us_list[i].titre == title)
                {
                    Debug.Log("Titre déjà utilisé pour une autre user story");
                    return false;
                }
            }
            return true;
        }
        Debug.Log("Il faut renseigner un titre à la user story");
        return false;
    }

    /// <summary>
    /// Fonction qui log le contenu du deck dans la console
    /// </summary>
    /// <param name="deck">Deck à log</param>
    public static void LogDeck(USJsonFile deck)
    {
        string _log = "";
        _log += "Titre du Deck : " + deck.title;
        _log += "\n" + "-------------------------------";
        for (int i = 0; i < deck.usdata_list.Count; i++)
        {
            _log += "\n" + "    Note : " + deck.usdata_list[i].score;
            _log += "\n" + "    Titre : " + deck.usdata_list[i].titre;
            _log += "\n" + "    Description : " + deck.usdata_list[i].desc;
            _log += "\n" + "    --------------------------";
        }
        Debug.Log(_log);
    }
}
