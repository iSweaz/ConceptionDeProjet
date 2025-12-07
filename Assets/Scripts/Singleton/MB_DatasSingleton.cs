using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe Singleton qui tient une référence permanente et universelle aux données JSON
/// </summary>
public class MB_DatasSingleton : MonoBehaviour
{
    public static MB_DatasSingleton instance; // Variable static qui référence l'instance active de MB_DatasSingleton
    public string nextScene = "Meeting_Room"; // Scène vers laquelle on transit après avoir initialisé le singleton

    /// <summary>
    /// Au démarrage de la scène "Bootstrap", déclenche Awake() qui initialise le singleton
    /// </summary>
    private void Awake()
    {
        // Si une instance existe déjà et que ce n'est pas celle-ci, on détruit
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Sinon, on définit l'instance
        instance = this;

        // Permet de faire persister l'instance entre les scènes
        DontDestroyOnLoad(gameObject);

        // On lance ensuite la première scène de jeu
        SceneManager.LoadScene(nextScene);
    }
}
