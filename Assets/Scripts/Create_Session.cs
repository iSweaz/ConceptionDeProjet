using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Create_Session : MonoBehaviour
{
    [SerializeField]   private Button button;
    [SerializeField]   private TMP_InputField[] inputs;
    [SerializeField]   private TMP_Dropdown mode;
    [SerializeField]   private TMP_InputField pseudo,SizeSession,time;
    [SerializeField]   private Singleton instance;

    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Load);

        mode = GetComponentInChildren<TMP_Dropdown>();

        inputs = GetComponentsInChildren<TMP_InputField>();

        pseudo =        inputs[0];
        SizeSession =   inputs[1];
        time =          inputs[2];
        instance = Singleton.instance;
    }

    void Load()
    {
        instance.numParticipants = int.Parse(SizeSession.text);
        instance.time =  float.Parse(time.text);
        SetModeValue();
        instance.Players.Add(pseudo.text);

        Debug.Log(instance.time);
        Debug.Log(instance.numParticipants);
        Debug.Log(instance.mode);
        Debug.Log(instance.Players[0]);
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
