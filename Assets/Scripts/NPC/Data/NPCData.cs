using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData
{
    public float moveSpeed = 2f;
    public float waitTimeAtBar = 3f;
    public float despawnTime = 1f;
    public float detectionRadius = 1f;
    public int bottlesRequired = 1;
    public NPCType npcType;
    public string npcName;
}
