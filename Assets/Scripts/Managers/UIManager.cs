using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Scrollbar volumeSlider;
    [SerializeField] private TMP_Dropdown turnDropdown;

    [Header("XR Turn Providers")]
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn;

    private OptionSettingsHandler optionSettings;

    private void Awake()
    {
        var turnSetter = new TurnTypeSetter(snapTurn, continuousTurn);
        optionSettings = new OptionSettingsHandler(volumeSlider, turnDropdown, turnSetter);
        optionSettings.Initialize();
    }
}
