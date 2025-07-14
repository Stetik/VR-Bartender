using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    Inactive,
    MovingToBar,
    WaitingAtBar,
    Satisfied,
    Despawning
}

public enum NPCType
{
    Regular,
    Impatient,
    Drunk
}