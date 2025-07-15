using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxActiveNPCs = 5;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private float spawnPointCooldown = 5f;

    [Header("Bar Configuration")]
    [SerializeField] private Transform barPosition;
    [SerializeField] private float barInteractionRange = 2f;

    // Systems
    private NPCPool npcPool;
    private NPCSpawnSystem spawnSystem;
    private BarData barData;

    // State
    private float timeSinceLastSpawn;
    private int activeNPCCount;

    // ==================================================
    // LIFECYCLE
    // ==================================================

    private void Awake()
    {
        InitializeBarData();
        InitializeSpawnSystem();
        InitializePool();
        InitializeEventListeners();

        timeSinceLastSpawn = 0f;
        activeNPCCount = 0;
    }

    private void InitializeBarData()
    {
        barData = new BarData
        {
            position = barPosition.position,
            lookDirection = barPosition.forward,
            interactionRange = barInteractionRange
        };
    }

    private void InitializeSpawnSystem()
    {
        List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

        foreach (Transform spawnTransform in spawnTransforms)
        {
            spawnPoints.Add(new SpawnPointData
            {
                position = spawnTransform.position,
                rotation = spawnTransform.eulerAngles,
                isActive = true,
                spawnCooldown = spawnPointCooldown
            });
        }

        spawnSystem = new NPCSpawnSystem(spawnPoints, spawnInterval);
    }

    private void InitializePool()
    {
        npcPool = new NPCPool(npcPrefab, poolSize, transform);
    }

    private void InitializeEventListeners()
    {
        NPCEvents.OnNPCReachedBar += OnNPCReachedBar;
        NPCEvents.OnNPCHitByBottle += OnNPCHitByBottle;
        NPCEvents.OnNPCDespawned += OnNPCDespawned;
        NPCEvents.OnNPCSatisfied += OnNPCSatisfied;
    }

    private void OnDestroy()
    {
        NPCEvents.OnNPCReachedBar -= OnNPCReachedBar;
        NPCEvents.OnNPCHitByBottle -= OnNPCHitByBottle;
        NPCEvents.OnNPCDespawned -= OnNPCDespawned;
        NPCEvents.OnNPCSatisfied -= OnNPCSatisfied;
    }

    // ==================================================
    // MAIN UPDATE
    // ==================================================

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // Update spawn system
        spawnSystem.Update(deltaTime);

        // Update all active NPCs
        npcPool.UpdateAllNPCs(deltaTime);

        // Try to spawn new NPCs
        TrySpawnNPC();
    }

    // ==================================================
    // SPAWNING
    // ==================================================

    private void TrySpawnNPC()
    {
        if (activeNPCCount >= maxActiveNPCs)
            return;

        Vector3? spawnPosition = spawnSystem.GetRandomSpawnPosition();

        if (spawnPosition.HasValue)
        {
            SpawnNPC(spawnPosition.Value);
        }
    }

    private void SpawnNPC(Vector3 position)
    {
        NPC npc = npcPool.GetNPC();

        if (npc != null)
        {
            npc.Initialize(this, barData, position);
            activeNPCCount++;
        }
    }

    // ==================================================
    // POOL MANAGEMENT
    // ==================================================

    public void ReturnNPCToPool(NPC npc)
    {
        npcPool.ReturnNPC(npc);
    }

    // ==================================================
    // EVENT HANDLERS
    // ==================================================

    private void OnNPCReachedBar(NPC npc)
    {
        Debug.Log($"NPC {npc.name} llegó a la barra. ¡Game Over!");
        SceneManager.LoadScene("loseScene"); // ← Cambia a la escena de derrota
    }

    private void OnNPCHitByBottle(NPC npc)
    {
        Debug.Log($"NPC {npc.name} fue golpeado por una botella.");
    }

    private void OnNPCDespawned(NPC npc)
    {
        activeNPCCount--;
        Debug.Log($"NPC {npc.name} se fue. NPCs activos: {activeNPCCount}");
    }

    private void OnNPCSatisfied(NPC npc)
    {
        Debug.Log($"NPC {npc.name} está satisfecho.");
    }

    // ==================================================
    // PUBLIC API
    // ==================================================

    public List<NPC> GetActiveNPCs()
    {
        return npcPool.GetActiveNPCs();
    }

    public int GetActiveNPCCount()
    {
        return activeNPCCount;
    }

    public void SetMaxActiveNPCs(int max)
    {
        maxActiveNPCs = max;
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }
}
