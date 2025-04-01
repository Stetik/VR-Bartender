using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnOnInteract : MonoBehaviour
{
    public GameObject prefabToSpawn; // The cube prefab you want to spawn
    public Transform spawnPosition; // The empty game object that defines where to spawn
    private XRSimpleInteractable interactable;

    void Start()
    {
        // Get the XR Simple Interactable component
        interactable = GetComponent<XRSimpleInteractable>();

        // Add listener for the selectEntered event
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(SpawnCube);
        }
    }

    public void SpawnCube(SelectEnterEventArgs args)
    {
        // Check if the rotation condition is met (around -45 degrees)
        if (transform.rotation.eulerAngles.x >= 315 || transform.rotation.eulerAngles.x <= 45)
        {
            // Spawn the prefab at the position of the empty game object
            if (prefabToSpawn != null && spawnPosition != null)
            {
                Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
            }
        }
    }
}
