using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Create_Session : MonoBehaviour
{
    private Button button;
    private TMP_InputField[] inputs;
    private TMP_Dropdown mode;
    private TMP_InputField pseudo,SizeSession,time;
    private Singleton instance = Singleton.instance;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Load);

        mode = GetComponent<TMP_Dropdown>();

        inputs = GetComponents<TMP_InputField>();
        pseudo =        inputs[0];
        SizeSession =   inputs[1];
        time =          inputs[2];
    }

    void Load()
    {
        instance.numParticipants = int.Parse(SizeSession.text);
        instance.time =  float.Parse(time.text);
        SetModeValue();
        instance.Players.Add(pseudo.text);
    }

    void SetModeValue()
    {
        switch(mode.value)
        {
            case 0:
                instance.mode = Singleton.GameMode.Unanimity;
                break;

            case 1:
                instance.mode = Singleton.GameMode.Average;
                break;

            case 2:
                instance.mode = Singleton.GameMode.Median;
                break;

            case 3:
                instance.mode = Singleton.GameMode.AbsMajority;
                break;

            case 4:
                instance.mode = Singleton.GameMode.RelMajority;
                break;

            default:
                instance.mode = Singleton.GameMode.Unanimity;
                break;
        }
    }

}
