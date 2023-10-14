using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEditor.Experimental.RestService;

[System.Serializable]
public class saveCrew
{
    [System.Serializable]
    public struct crewData
    {
        public Vector3 currentPosition;
        public int savedHealth;
        public IInteractable savedInteraction;
    }

    public List<crewData> m_crewDatas = new List<crewData>();

    public string SetJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadJSON(string loadData)
    {
        JsonUtility.FromJsonOverwrite(loadData, this);
    }
}

public interface ISaveable
{
    void SetSaveData(saveCrew savedCrewData);
    void LoadData(saveCrew savedCrewData);
}