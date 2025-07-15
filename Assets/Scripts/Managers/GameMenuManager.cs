using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("XR Input")]
    [SerializeField] private InputActionProperty showButton;

    [Header("Head Transform & Menu")]
    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private GameObject menu;

    [Header("Panels")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private CanvasGroup howToPlayPanel;

    [Header("Scene")]
    [SerializeField] private string sceneToLoad = "SceneNameHere";

    [Header("Settings")]
    [SerializeField] private Scrollbar volumeSlider;
    [SerializeField] private TMP_Dropdown turnDropdown;
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn;

    private OptionSettingsHandler optionSettings;

    private void Awake()
    {
        optionSettings = new OptionSettingsHandler(volumeSlider, turnDropdown, new TurnTypeSetter(snapTurn, continuousTurn));
        optionSettings.Initialize();

        menu.SetActive(false);
        var raycaster = menu.GetComponent<GraphicRaycaster>();
        if (raycaster != null) raycaster.enabled = false;

        showButton.action.performed += ctx => ToggleMenu();
        showButton.action.Enable();

        ShowPanel(startPanel);
        HidePanel(howToPlayPanel);
    }

    private void OnDestroy()
    {
        showButton.action.performed -= ctx => ToggleMenu();
        showButton.action.Disable();
    }

    public void ShowStartPanel() => SetPanel(startPanel, howToPlayPanel);
    public void ShowHowToPlayPanel() => SetPanel(howToPlayPanel, startPanel);

    private void SetPanel(CanvasGroup show, CanvasGroup hide)
    {
        ShowPanel(show);
        HidePanel(hide);
    }

    private void ToggleMenu()
    {
        bool active = !menu.activeSelf;
        menu.SetActive(active);

        var raycaster = menu.GetComponent<GraphicRaycaster>();
        if (raycaster != null) raycaster.enabled = active;

        if (active)
        {
            Vector3 lookDirection = new Vector3(head.forward.x, 0, head.forward.z).normalized;
            menu.transform.position = head.position + lookDirection * spawnDistance;
            menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
            menu.transform.forward *= -1;
        }
    }

    private void ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup panel)
    {
        panel.alpha = 0f;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    public void LoadDefaultScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad)) SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() => Application.Quit();
}
