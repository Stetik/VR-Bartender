using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPointData
{
    public Vector3 position;
    public Vector3 rotation;
    public bool isActive;
    public float spawnCooldown;
}
