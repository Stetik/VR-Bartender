using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSettingsHandler
{
    private Scrollbar volumeSlider;
    private TMP_Dropdown turnDropdown;
    private TurnTypeSetter turnSetter;

    public OptionSettingsHandler(Scrollbar volumeSlider, TMP_Dropdown turnDropdown, TurnTypeSetter turnSetter)
    {
        this.volumeSlider = volumeSlider;
        this.turnDropdown = turnDropdown;
        this.turnSetter = turnSetter;
    }

    public void Initialize()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        turnDropdown.onValueChanged.AddListener(SetTurnOption);

        if (PlayerPrefs.HasKey("turn"))
        {
            int val = PlayerPrefs.GetInt("turn");
            turnDropdown.SetValueWithoutNotify(val);
            turnSetter.ApplyFromIndex(val);
        }
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void SetTurnOption(int index)
    {
        PlayerPrefs.SetInt("turn", index);
        turnSetter.ApplyFromIndex(index);
    }
}
