using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMenuManager : MonoBehaviour
{
    [Header("CanvasGroup Panels")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private CanvasGroup howToPlayPanel;

    [Header("Default Scene to Load (optional)")]
    [SerializeField] private string sceneToLoad = "SceneNameHere";

    private void Start()
    {
        ShowPanel(startPanel);
        HidePanel(howToPlayPanel);
    }

    public void ShowStartPanel()
    {
        ShowPanel(startPanel);
        HidePanel(howToPlayPanel);
    }

    public void ShowHowToPlayPanel()
    {
        ShowPanel(howToPlayPanel);
        HidePanel(startPanel);
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

    // ✅ Método por defecto que usa el string seteado en el inspector
    public void LoadDefaultScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // ✅ Método nuevo para botones con parámetro
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
