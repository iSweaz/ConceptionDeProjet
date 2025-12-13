using UnityEngine;
using System.Collections.Generic;

public class deck : MonoBehaviour
{
    [System.Serializable]
    public class userStoryData
    {
        public string name;
        public string description;
    }

    [System.Serializable]
    public class UserStorys
    {
           public List<userStoryData> US = new List<userStoryData>();

    }
}
