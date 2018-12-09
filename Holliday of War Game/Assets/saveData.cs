using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveData : MonoBehaviour {

    string Level1DataPath = "Assets/Resources/SaveData/Level1Data";
    // Use this for initialization
    void recordLevelComplete(int LevelNumber)
    {
        StreamWriter writer = new StreamWriter(Level1DataPath,false);
        writer.WriteLine("Complete");
    }
    bool isLevelCompleted(int LevelNumber)
    {
        StreamReader reader = new StreamReader(Level1DataPath, true);
        return reader.ReadLine().Equals("Complete");
    }
}
