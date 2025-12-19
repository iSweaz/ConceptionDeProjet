using NUnit.Framework;
using System.Collections.Generic;

public class T_GameModes
{
    /// <summary>
    /// Test de la gestion des réponses avec cartes spéciales
    /// </summary>
    [Test]
    public void T_ProcessAnswer()
    {
        List<string> testCoffee = new List<string>() { "1", "c", "8", "3" };
        List<string> testUnknown = new List<string>() { "1", "100", "i", "3" };
        List<string> testError = new List<string>() { "1", "100", "8", "3" };
        string retCoffee = Manager.ProcessAnswerByGameMode(Singleton.GameMode.Unanimity, testCoffee);
        string retUnknown = Manager.ProcessAnswerByGameMode(Singleton.GameMode.Unanimity, testUnknown);
        Assert.AreEqual("Quelqu'un demande une pause", retCoffee);
        Assert.AreEqual("Quelqu'un n'a pas compris la tache", retUnknown);
    }

    /// <summary>
    /// Test du calcul de la réponse dans le mode de jeu unanimité
    /// </summary>
    [Test]
    public void T_Unanimity()
    {
        List<string> testfalse = new List<string>() {"1", "3", "8", "3"};
        List<string> testtrue = new List<string>() {"3", "3", "3", "3"};
        string repfalse = Manager.ProcessGameMode_Unanimity(testfalse);
        string reptrue = Manager.ProcessGameMode_Unanimity(testtrue);
        Assert.AreEqual("Pas unanime", repfalse);
        Assert.AreEqual("3", reptrue);
    }

    /// <summary>
    /// Test du calcul de la réponse dans le mode de jeu Moyenne
    /// </summary>
    [Test]
    public void T_Average()
    {
        List<string> test = new List<string>() { "1", "3", "5", "3" };
        string rep = Manager.ProcessGameMode_Average(test);
        Assert.AreEqual("3", rep);
    }

    /// <summary>
    /// Test du calcul de la réponse dans le mode de jeu Médiane
    /// </summary>
    [Test]
    public void T_Median()
    {
        List<string> testeven = new List<string>() { "2", "3", "5", "3" };
        string repeven = Manager.ProcessGameMode_Median(testeven);
        List<string> testuneven = new List<string>() { "1", "2", "5", "3", "2" };
        string repuneven = Manager.ProcessGameMode_Median(testuneven);
        Assert.AreEqual("3", repeven);
        Assert.AreEqual("2", repuneven);
    }

    /// <summary>
    /// Test du calcul de la réponse dans le mode de jeu Majorité absolue
    /// </summary>
    [Test]
    public void T_AbsMaj()
    {
        List<string> testeven = new List<string>() { "3", "3", "5", "3" };
        string repeven = Manager.ProcessGameMode_AbsMaj(testeven);
        List<string> testuneven = new List<string>() { "1", "2", "5", "3", "2" };
        string repuneven = Manager.ProcessGameMode_AbsMaj(testuneven);
        Assert.AreEqual("3", repeven);
        Assert.AreEqual("Pas de majorité absolue", repuneven);
    }

    /// <summary>
    /// Test du calcul de la réponse dans le mode de jeu Majorité relative
    /// </summary>
    [Test]
    public void T_RelMaj()
    {
        List<string> test = new List<string>() { "1", "2", "5", "3", "2" };
        string rep = Manager.ProcessGameMode_RelMaj(test);
        Assert.AreEqual("2", rep);
    }
}
