using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level Generation/New Level")]
public class level : ScriptableObject
{
    public List<GameObject> encounters;
    public List<GameObject> loot;
    public float lootRatio;
    public Material skybox;
    public int timeToCompleteMax;
    public int timeToCompleteMin;
    public int numberOfEncountersMax;
    public int numberOfEncountersMin;
}
