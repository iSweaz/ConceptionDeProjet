using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class T_DisplayBoard
{
    private IEnumerator GoToMeetingScene()
    {
        //Chemin pour aller sur la scène Meeting avec un deck et des paramètres chargés
        SceneManager.LoadScene("Menu");
        yield return null;
        Create_Session session = Object.FindFirstObjectByType<Create_Session>();
        Assert.IsNotNull(session);
        session.pseudo.text = "UnitTester";
        session.sizeSession.text = "10";
        session.time.text = "2";
        Singleton instance = Singleton.instance;
        instance.deck = new USJsonFile();
        instance.deck.title = "FileTest";
        instance.deck.usdata_list.Add(new USData() { titre = "Titre", desc = "Desc" });
        instance.deck.usdata_list.Add(new USData() { titre = "Titre2", desc = "Desc2" });
        string path = Application.dataPath + "FileTest.json";
        instance.deck.filePath = path;
        session.Load();
        yield return null;
    }

    /// <summary>
    /// Test de l'affichage des users stories sur le blackboard
    /// </summary>
    [UnityTest]
    public IEnumerator T_DisplayBlackBoard()
    {
        yield return GoToMeetingScene();
        Singleton instance = Singleton.instance;
        Manager manager = Object.FindFirstObjectByType<Manager>();
        Display_Unit board = manager.blackBoard;
        Assert.AreEqual(board.nameText.text, instance.deck.usdata_list[0].titre);
        Assert.AreEqual(board.descText.text, instance.deck.usdata_list[0].desc);
        yield return null;
    }

    /// <summary>
    /// Test de l'affichage de la prochaine user story sur le blackboard
    /// </summary>
    [UnityTest]
    public IEnumerator T_ChangeDisplayBoard()
    {
        yield return GoToMeetingScene();
        Singleton instance = Singleton.instance;
        Manager manager = Object.FindFirstObjectByType<Manager>();
        Display_Unit board = manager.blackBoard;
        manager.onStartButtonClicked();
        Assert.AreEqual(board.nameText.text, instance.deck.usdata_list[0].titre);
        Assert.AreEqual(board.descText.text, instance.deck.usdata_list[0].desc);
        yield return new WaitForSeconds(3); //car on a setup le timer à 2 secondes
        manager.onNextButtonClicked();
        Assert.AreEqual(board.nameText.text, instance.deck.usdata_list[1].titre);
        Assert.AreEqual(board.descText.text, instance.deck.usdata_list[1].desc);
        yield return null;
    }
}
