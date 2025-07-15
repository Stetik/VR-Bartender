using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("NPC Configuration")]
    [SerializeField] private NPCData npcData;
    [SerializeField] private Collider npcCollider;
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private Transform visualRoot;

    // State
    private NPCState currentState;
    private Vector3 targetPosition;
    private float stateTimer;
    private int bottlesReceived;
    private bool isInitialized;

    // Referencias
    private NPCManager manager;
    private BarData barData;

    // Properties
    public NPCData Data => npcData;
    public NPCState State => currentState;
    public bool IsActive => currentState != NPCState.Inactive;
    public bool IsSatisfied => bottlesReceived >= npcData.bottlesRequired;

    // ==================================================
    // LIFECYCLE
    // ==================================================

    private void Awake()
    {
        if (npcData == null)
            npcData = new NPCData();

        currentState = NPCState.Inactive;
        bottlesReceived = 0;
        isInitialized = false;

        SetupCollider();
    }

    private void SetupCollider()
    {
        if (npcCollider == null)
            npcCollider = GetComponent<Collider>();

        if (npcCollider != null)
        {
            npcCollider.isTrigger = true;
            npcCollider.enabled = false;
        }
    }

    // ==================================================
    // INITIALIZATION
    // ==================================================

    public void Initialize(NPCManager npcManager, BarData bar, Vector3 spawnPosition)
    {
        manager = npcManager;
        barData = bar;
        transform.position = spawnPosition;
        targetPosition = barData.position;

        ResetNPC();
        isInitialized = true;

        ActivateNPC();
    }

    private void ResetNPC()
    {
        bottlesReceived = 0;
        stateTimer = 0f;
        currentState = NPCState.Inactive;

        if (npcCollider != null)
            npcCollider.enabled = false;

        if (npcAnimator != null)
            npcAnimator.SetBool("IsWalking", false);
    }

    // ==================================================
    // MAIN UPDATE
    // ==================================================

    public void UpdateNPC(float deltaTime)
    {
        if (!isInitialized || currentState == NPCState.Inactive)
            return;

        stateTimer += deltaTime;

        switch (currentState)
        {
            case NPCState.MovingToBar:
                UpdateMovingToBar(deltaTime);
                break;

            case NPCState.WaitingAtBar:
                UpdateWaitingAtBar(deltaTime);
                break;

            case NPCState.Satisfied:
                UpdateSatisfied(deltaTime);
                break;

            case NPCState.Despawning:
                UpdateDespawning(deltaTime);
                break;
        }
    }

    // ==================================================
    // STATE UPDATES
    // ==================================================

    private void UpdateMovingToBar(float deltaTime)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.1f)
        {
            transform.position += direction * npcData.moveSpeed * deltaTime;
            transform.LookAt(targetPosition);
        }
        else
        {
            ChangeState(NPCState.WaitingAtBar);
        }
    }

    private void UpdateWaitingAtBar(float deltaTime)
    {
        if (IsSatisfied)
        {
            ChangeState(NPCState.Satisfied);
        }
        else if (stateTimer >= npcData.waitTimeAtBar)
        {
            ChangeState(NPCState.Despawning);
        }
    }

    private void UpdateSatisfied(float deltaTime)
    {
        if (stateTimer >= 0.5f)
        {
            ChangeState(NPCState.Despawning);
        }
    }

    private void UpdateDespawning(float deltaTime)
    {
        if (stateTimer >= npcData.despawnTime)
        {
            DeactivateNPC();
        }
    }

    // ==================================================
    // STATE MANAGEMENT
    // ==================================================

    private void ChangeState(NPCState newState)
    {
        currentState = newState;
        stateTimer = 0f;

        switch (newState)
        {
            case NPCState.MovingToBar:
                if (npcAnimator != null)
                    npcAnimator.SetBool("IsWalking", true);
                break;

            case NPCState.WaitingAtBar:
                if (npcAnimator != null)
                    npcAnimator.SetBool("IsWalking", false);

                NPCEvents.OnNPCReachedBar?.Invoke(this);
                break;

            case NPCState.Satisfied:
                NPCEvents.OnNPCSatisfied?.Invoke(this);
                break;

            case NPCState.Despawning:
                if (npcCollider != null)
                    npcCollider.enabled = false;
                break;
        }
    }

    // ==================================================
    // INTERACTIONS
    // ==================================================

    public void OnHitByBottle(GameObject bottle)
    {
        if (currentState == NPCState.Despawning || currentState == NPCState.Inactive)
            return;

        bottlesReceived++;
        NPCEvents.OnNPCHitByBottle?.Invoke(this);

        if (IsSatisfied)
        {
            ChangeState(NPCState.Satisfied);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bottle"))
        {
            OnHitByBottle(other.gameObject);
            DestroyNPC();
        }
    }

    private void DestroyNPC()
    {
        ChangeState(NPCState.Despawning);
        stateTimer = npcData.despawnTime;
    }

    // ==================================================
    // ACTIVATION/DEACTIVATION
    // ==================================================

    private void ActivateNPC()
    {
        gameObject.SetActive(true);

        if (npcCollider != null)
            npcCollider.enabled = true; // ← Activamos el collider desde el inicio

        ChangeState(NPCState.MovingToBar);
    }

    private void DeactivateNPC()
    {
        NPCEvents.OnNPCDespawned?.Invoke(this);
        currentState = NPCState.Inactive;
        gameObject.SetActive(false);

        if (manager != null)
            manager.ReturnNPCToPool(this);
    }
}
