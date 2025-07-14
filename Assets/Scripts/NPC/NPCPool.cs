using System.Collections.Generic;
using UnityEngine;

public class NPCPool
{
    private Queue<NPC> availableNPCs;
    private List<NPC> activeNPCs;
    private GameObject npcPrefab;
    private Transform poolParent;
    private int poolSize;

    public NPCPool(GameObject prefab, int size, Transform parent = null)
    {
        npcPrefab = prefab;
        poolSize = size;
        poolParent = parent;

        availableNPCs = new Queue<NPC>();
        activeNPCs = new List<NPC>();

        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject npcObj = Object.Instantiate(npcPrefab, poolParent);
            NPC npc = npcObj.GetComponent<NPC>();

            if (npc != null)
            {
                npcObj.SetActive(false);
                availableNPCs.Enqueue(npc);
            }
        }
    }

    public NPC GetNPC()
    {
        if (availableNPCs.Count > 0)
        {
            NPC npc = availableNPCs.Dequeue();
            activeNPCs.Add(npc);
            return npc;
        }

        return null;
    }

    public void ReturnNPC(NPC npc)
    {
        if (activeNPCs.Contains(npc))
        {
            activeNPCs.Remove(npc);
            availableNPCs.Enqueue(npc);
        }
    }

    public List<NPC> GetActiveNPCs()
    {
        return new List<NPC>(activeNPCs);
    }

    public void UpdateAllNPCs(float deltaTime)
    {
        for (int i = activeNPCs.Count - 1; i >= 0; i--)
        {
            if (activeNPCs[i] != null)
            {
                activeNPCs[i].UpdateNPC(deltaTime);
            }
        }
    }
}