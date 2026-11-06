using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // SaveData.cs
    [System.Serializable]
    public class GameSaveData
    {
        public string Version = "1.0";
        public string Timestamp;
        public Dictionary<string, string> DataBlocks = new();
    }
}
