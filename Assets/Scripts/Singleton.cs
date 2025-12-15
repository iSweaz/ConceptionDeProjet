using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public enum GameMode
    {
        Unanimity,
        Average,
        Median,
        AbsMajority,
        RelMajority
    }

    public static Singleton instance        {get; private set;}
    public USJsonFile       deck            {get; set;}
    public int              numParticipants {get; set;}
    public float            time            {get; set;}
    public GameMode         mode            {get; set;}
    public List<string>     playersName     {get; set;} 
    public bool             revalutate      {get; set;}

    private void Awake()
    {
        if(instance !=null && instance!= this)
            Destroy(this);
        else
        {
            playersName = new List<string>();
            instance = this; 
            DontDestroyOnLoad(this);
        }
    }
}


