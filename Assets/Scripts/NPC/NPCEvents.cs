using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCEvents
{
    public static System.Action<NPC> OnNPCReachedBar;
    public static System.Action<NPC> OnNPCHitByBottle;
    public static System.Action<NPC> OnNPCDespawned;
    public static System.Action<NPC> OnNPCSatisfied;
}
