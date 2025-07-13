using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMenuManager : MonoBehaviour
{
    [Header("CanvasGroup Panels")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private CanvasGroup howToPlayPanel;

    [Header("Scene to Load")]
    [SerializeField] private string sceneToLoad = "SceneNameHere";

    private void Start()
    {
        // Show start panel and hide how-to-play panel on start
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

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
