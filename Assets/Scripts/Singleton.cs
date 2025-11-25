using System.Collections.Generic;
using System.Threading;
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
    public Deck.UserStorys  deck            {get; set;}
    public int              numParticipants {get; set;}
    public float            time            {get; set;}
    public GameMode         mode            {get; set;}
    public List<string>     Players         {get; set;}
    

    private void Awake()
    {
        if(instance !=null && instance!= this)
            Destroy(this);
        else
        {
            instance = this; 
            DontDestroyOnLoad(this);
        }
    }
}


