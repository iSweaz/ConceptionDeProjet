using TMPro;
using PurrNet;

public class NI_Display_Unit : NetworkIdentity
{
  private TMP_Text nameText,descText;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Awake()
  {
    TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
    nameText = texts[0];
    descText = texts[1];
  }

  /// <summary>
  /// Actualise le contenu affiché au tableau
  /// </summary>
  /// <param name="title">titre de la user story actuelle</param>
  /// <param name="description">description de la user story actuelle</param>
  [ObserversRpc]
  public void ChangeDisplay(string title, string description)
  {
    nameText.text = title;
    descText.text = description;
  }
   
/// <summary>
/// Initialise le contenu affiché au tableau
/// </summary>
/// <param name="title">titre de la première user story</param>
/// <param name="description">description de la première user story</param>
  public void InitDisplay(string title, string description)
  {
    nameText.text = title;
    descText.text = description;
  }
}
