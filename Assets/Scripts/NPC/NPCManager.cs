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

    [Header("Timer to Win (in seconds)")]
    [SerializeField] private float winTime = 180f; // 3 minutos

    private float timer;
    private bool gameEnded = false;

    // Systems
    private NPCPool npcPool;
    private NPCSpawnSystem spawnSystem;
    private BarData barData;

    // State
    private float timeSinceLastSpawn;
    private int activeNPCCount;

    private void Awake()
    {
        InitializeBarData();
        InitializeSpawnSystem();
        InitializePool();
        InitializeEventListeners();

        timeSinceLastSpawn = 0f;
        activeNPCCount = 0;
        timer = 0f;
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

    private void Update()
    {
        if (gameEnded) return;

        float deltaTime = Time.deltaTime;

        // Timer de victoria
        timer += deltaTime;
        if (timer >= winTime)
        {
            WinGame();
            return;
        }

        // Actualizar spawn y NPCs
        spawnSystem.Update(deltaTime);
        npcPool.UpdateAllNPCs(deltaTime);
        TrySpawnNPC();
    }

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

    public void ReturnNPCToPool(NPC npc)
    {
        npcPool.ReturnNPC(npc);
    }

    // ====================== EVENTOS ======================

    private void OnNPCReachedBar(NPC npc)
    {
        if (gameEnded) return;

        Debug.Log($"NPC {npc.name} llegó a la barra. ¡Game Over!");
        gameEnded = true;
        SceneManager.LoadScene("loseScene");
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

    private void WinGame()
    {
        Debug.Log("¡Ganaste! Sobreviviste 3 minutos.");
        gameEnded = true;
        SceneManager.LoadScene("winScene");
    }

    // ====================== API PÚBLICA ======================

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
