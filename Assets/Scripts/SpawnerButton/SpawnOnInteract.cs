using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnOnInteract : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPosition;

    private XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(SpawnCube);
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(SpawnCube);
        }
    }

    private void SpawnCube(SelectEnterEventArgs args)
    {
        float xRotation = transform.rotation.eulerAngles.x;
        bool isWithinAngle = xRotation >= 315 || xRotation <= 45;

        if (isWithinAngle && prefabToSpawn != null && spawnPosition != null)
        {
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }
    }
}
