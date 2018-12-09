using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveData : MonoBehaviour {


    public static void recordLevelComplete(int LevelNumber)
    {
        if (LevelNumber == 1)
        {
            StreamWriter writer = new StreamWriter("Assets/Resources/SaveData/Level1Data.txt", true);
            writer.WriteLine("Complete");
        }
        else if (LevelNumber == 2)
        {
            StreamWriter writer = new StreamWriter("Assets/Resources/SaveData/Level2Data.txt", true);
            writer.WriteLine("Complete");
        }
        else if (LevelNumber == 3)
        {
            StreamWriter writer = new StreamWriter("Assets/Resources/SaveData/Level3Data.txt", true);
            writer.WriteLine("Complete");
        }
        else
        {
            Debug.Log("cannot record data: error level doesnt exist");
        }
    }
    public static bool isLevelCompleted(int LevelNumber)
    {
        if (LevelNumber == 1)
        {
            StreamReader reader = new StreamReader("Assets/Resources/SaveData/Level1Data.txt", true);
            return reader.ReadLine().Equals("Complete");
        }
        else if (LevelNumber == 2)
        {
            StreamReader reader = new StreamReader("Assets/Resources/SaveData/Level2Data.txt", true);
            return reader.ReadLine().Equals("Complete");
        }
        else if (LevelNumber == 3)
        {
            StreamReader reader = new StreamReader("Assets/Resources/SaveData/Level3Data.txt", true);
            return reader.ReadLine().Equals("Complete");
        }
        else
        {
            Debug.Log("cannot read data: error level doesnt exist");
            return false;
        }
    }
}
