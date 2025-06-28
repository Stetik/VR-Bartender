using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [Header("XR Input")]
    [SerializeField] private InputActionProperty showButton;

    [Header("Menu Settings")]
    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private GameObject menu;

    [Header("Option UI Elements")]
    [SerializeField] private Scrollbar volumeSlider;
    [SerializeField] private TMP_Dropdown turnDropdown;

    [Header("XR Turn Providers")]
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn;

    private OptionSettingsHandler optionSettings;

    private void Awake() 
    {
        // Inicializar lógica de opciones
        var turnSetter = new TurnTypeSetter(snapTurn, continuousTurn);
        optionSettings = new OptionSettingsHandler(volumeSlider, turnDropdown, turnSetter);
        optionSettings.Initialize();

        // Ocultar el menú al inicio
        menu.SetActive(false);

        // Desactivar Raycaster (rendimiento)
        var raycaster = menu.GetComponent<GraphicRaycaster>();
        if (raycaster != null) raycaster.enabled = false;

        // Suscribirse al input sin OnEnable()
        showButton.action.performed += OnShowButtonPressed;
        showButton.action.Enable();
    }

    private void OnDestroy() // Necesario en esta ocacion para evitar memory leaks o duplicaciones
    {
        showButton.action.performed -= OnShowButtonPressed;
        showButton.action.Disable();
    }

    private void OnShowButtonPressed(InputAction.CallbackContext ctx)
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        bool isActive = !menu.activeSelf;
        menu.SetActive(isActive);

        var raycaster = menu.GetComponent<GraphicRaycaster>();
        if (raycaster != null) raycaster.enabled = isActive;

        if (isActive)
        {
            Vector3 lookDirection = new Vector3(head.forward.x, 0, head.forward.z).normalized;
            menu.transform.position = head.position + lookDirection * spawnDistance;

            menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
            menu.transform.forward *= -1;
        }
    }
}
