using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe à glisser sur un button pour pouvoir choisir une couleur de texte selon l'état interactable du button
/// </summary>
public class TMPButtonInteractableColor : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private Color interactableColor = Color.white;
    [SerializeField] private Color disabledColor = Color.gray;

    private void Update()
    {
        if (button == null || buttonText == null)
            return;

        if (button.interactable)
            buttonText.color = interactableColor;
        else
            buttonText.color = disabledColor;
    }
}