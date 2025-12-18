using NUnit.Framework;
using UnityEngine;
using System.IO;

public class T_USJsonFile
{
    /// <summary>
    /// Test de création de la classe USJsonFile
    /// </summary>
    [Test]
    public void T_USJsonFileCreation()
    {
        USJsonFile usJsonFile = new USJsonFile();
        Assert.AreEqual("Untitled", usJsonFile.title);
        Assert.IsNull(usJsonFile.filePath);
        Assert.IsEmpty(usJsonFile.usdata_list);
    }

    /// <summary>
    /// Test de sauvegarde d'un fichier JSon
    /// </summary>
    [Test]
    public void T_SaveJson()
    {
        USJsonFile usJsonFile = new USJsonFile();
        usJsonFile.title = "FileTest";
        usJsonFile.usdata_list.Add(new USData() { titre = "Titre", desc = "Desc" });
        usJsonFile.usdata_list.Add(new USData() { titre = "Titre2", desc = "Desc2" });
        string path = Application.dataPath + "FileTest.json";
        C_JSonUtility.SaveDeck(usJsonFile, path);
        Assert.IsTrue(File.Exists(path));
    }

    /// <summary>
    /// Test de chargement d'un fichier JSon
    /// </summary>
    [Test]
    public void T_LoadJson()
    {
        string path = Application.dataPath + "FileTest.json";
        USJsonFile usFile = C_JSonUtility.LoadDeck(path);
        Assert.AreEqual("FileTest", usFile.title);
        Assert.AreEqual(path, usFile.filePath);
        Assert.AreEqual(2, usFile.usdata_list.Count);
    }
}
