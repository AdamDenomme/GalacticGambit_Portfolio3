using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class saveManager
{
    public static void SaveJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        saveData sd = new saveData();
        foreach (var saveable in a_Saveables)
        {
           // saveable.SetSaveData(sd);
        }

        if (FileManager.WriteToFile("SaveData01.dat", sd.SetJSON()))
        {
            Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(IEnumerable<ISaveable> a_Saveables)
    {
        if (FileManager.LoadFromFile("SaveData01.dat", out var json))
        {
            saveData sd = new saveData();
            sd.LoadJSON(json);

            foreach (var saveable in a_Saveables)
            {
                //saveable.LoadData(sd);
            }

            Debug.Log("Load complete");
        }
    }
}