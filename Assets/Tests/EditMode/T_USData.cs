using NUnit.Framework;

public class T_USData
{
    /// <summary>
    /// Test de création de la classe USData
    /// </summary>
    [Test]
    public void T_USDataCreation()
    {
        USData usData = new USData();
        Assert.AreEqual("Description", usData.desc);
        Assert.AreEqual("Titre", usData.titre);
        Assert.AreEqual(-1, usData.score);
    }
}
