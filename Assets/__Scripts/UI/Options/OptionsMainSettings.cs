﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMainSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField customLevelField;
    [SerializeField] private TextMeshProUGUI installFieldErrorText;
    [SerializeField] private Toggle discordToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeSliderDisplay;
    [SerializeField] private Slider initialBatchSizeSlider;
    [SerializeField] private TextMeshProUGUI initialBatchSizeDisplay;
    [SerializeField] private Toggle darkThemeToggle;
    [SerializeField] private Slider metronomeSlider;
    [SerializeField] private TextMeshProUGUI metronomeSliderDisplay;
    private MetronomeHandler _metronomeHandler;

    private void Start()
    {
        _metronomeHandler = OptionsController.Find<MetronomeHandler>();
        customLevelField.text = Settings.Instance.BeatSaberInstallation;
        discordToggle.isOn = Settings.Instance.DiscordRPCEnabled;
        volumeSlider.value = AudioListener.volume * 10;
        volumeSliderDisplay.text = $"{volumeSlider.value * 10}%";
        initialBatchSizeSlider.value = Settings.Instance.InitialLoadBatchSize / 50;
        initialBatchSizeDisplay.text = $"{Settings.Instance.InitialLoadBatchSize}";
        darkThemeToggle.isOn = Settings.Instance.DarkTheme;
        metronomeSlider.value = Settings.Instance.MetronomeVolume * 10;
        metronomeSliderDisplay.text = $"{metronomeSlider.value * 10}%";
    }

    public void UpdateDiscordRPC(bool enable)
    {
        if (Settings.Instance.DiscordRPCEnabled == enable) return;
        Settings.Instance.DiscordRPCEnabled = enable;
        PersistentUI.Instance.ShowDialogBox("A restart is required for changes to apply.", null, PersistentUI.DialogBoxPresetType.Ok);
    }

    public void UpdateBeatSaberInstall(string value)
    {
        string old = Settings.Instance.BeatSaberInstallation;
        Settings.Instance.BeatSaberInstallation = value;
        installFieldErrorText.text = "All good!";
        if (!Settings.ValidateDirectory(ErrorFeedback))
            Settings.Instance.BeatSaberInstallation = old;
    }

    private void ErrorFeedback(string feedback)
    {
        installFieldErrorText.text = feedback;
    }

    public void UpdateGameVolume(float value)
    {
        AudioListener.volume = value / 10;
        Settings.Instance.Volume = value / 10;
        volumeSliderDisplay.text = $"{volumeSlider.value * 10}%";
    }
    
    public void UpdateMetronomeVolume(float value)
    {
        Settings.Instance.MetronomeVolume = value / 10;
        metronomeSliderDisplay.text = $"{metronomeSlider.value * 10}%";
        if (_metronomeHandler != null && Settings.Instance.MetronomeVolume*10 != value)
        {
            _metronomeHandler.CowBell = Input.GetKey(KeyCode.LeftControl);
        }
    }

    public void UpdateInitialBatchSize(float value)
    {
        int batchSize = Mathf.RoundToInt(value * 50);
        Settings.Instance.InitialLoadBatchSize = batchSize;
        initialBatchSizeDisplay.text = batchSize.ToString();
    }

    public void UpdateDarkTheme(bool enable)
    {
        if (enable == Settings.Instance.DarkTheme) return;
        PersistentUI.Instance.ShowDialogBox("A restart may be required for all changes to apply.", null, PersistentUI.DialogBoxPresetType.Ok);
        Settings.Instance.DarkTheme = enable;
    }
}
