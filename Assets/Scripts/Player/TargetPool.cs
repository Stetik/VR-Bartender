// TargetPool.cs
// This MonoBehaviour manages the object pool for the targets. It handles the
// creation, retrieval, and return of target objects.

using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    public GameObject targetPrefab;
    public int poolSize = 10;
    public List<Transform> spawnPoints;

    private readonly Queue<Target> availableTargets = new Queue<Target>();
    private readonly List<Target> activeTargets = new List<Target>();

    void Start()
    {
        // Initialize the object pool
        for (int i = 0; i < poolSize; i++)
        {
            var newTarget = new Target(targetPrefab, transform, this);
            availableTargets.Enqueue(newTarget);
        }

        // Spawn an initial target
        SpawnTarget();
    }

    // Retrieves a target from the pool and activates it at a random spawn point
    public void SpawnTarget()
    {
        if (availableTargets.Count == 0)
        {
            Debug.LogWarning("No available targets in the pool!");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        Target targetToSpawn = availableTargets.Dequeue();
        activeTargets.Add(targetToSpawn);

        // Get a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[spawnIndex];

        targetToSpawn.Activate(spawnPoint.position, spawnPoint.rotation);
    }

    // Returns a target to the pool and spawns a new one
    public void ReturnTarget(Target target)
    {
        target.Deactivate();
        activeTargets.Remove(target);
        availableTargets.Enqueue(target);

        // Spawn a new target immediately after one is returned
        SpawnTarget();
    }
}
