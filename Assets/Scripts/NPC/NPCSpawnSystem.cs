using UnityEngine;
using System.Collections.Generic;

public class NPCSpawnSystem
{
    private List<SpawnPointData> spawnPoints;
    private float[] spawnCooldowns;
    private float globalSpawnCooldown;
    private float timeSinceLastSpawn;

    public NPCSpawnSystem(List<SpawnPointData> points, float globalCooldown = 2f)
    {
        spawnPoints = new List<SpawnPointData>(points);
        spawnCooldowns = new float[spawnPoints.Count];
        globalSpawnCooldown = globalCooldown;
        timeSinceLastSpawn = 0f;
    }

    public void Update(float deltaTime)
    {
        timeSinceLastSpawn += deltaTime;

        for (int i = 0; i < spawnCooldowns.Length; i++)
        {
            if (spawnCooldowns[i] > 0)
                spawnCooldowns[i] -= deltaTime;
        }
    }

    public Vector3? GetRandomSpawnPosition()
    {
        if (timeSinceLastSpawn < globalSpawnCooldown)
            return null;

        List<int> availableSpawns = new List<int>();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].isActive && spawnCooldowns[i] <= 0)
            {
                availableSpawns.Add(i);
            }
        }

        if (availableSpawns.Count == 0)
            return null;

        int randomIndex = availableSpawns[Random.Range(0, availableSpawns.Count)];
        spawnCooldowns[randomIndex] = spawnPoints[randomIndex].spawnCooldown;
        timeSinceLastSpawn = 0f;

        return spawnPoints[randomIndex].position;
    }
}