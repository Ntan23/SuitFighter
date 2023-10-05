using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsUI : MonoBehaviour
{
    #region FloatVaribles
    [HideInInspector] public float BGMMixerVolume;
    [HideInInspector] public float SFXMixerVolume;
    #endregion

    #region OtherVariables
    [SerializeField] private GameObject resetGroup;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider BGMMixerVolumeSlider;
    [SerializeField] private Slider SFXMixerVolumeSlider;
    #endregion

    void Start()
    {
        BGMMixerVolume = PlayerPrefs.GetFloat("BGMMixerVolume",0);
        SFXMixerVolume = PlayerPrefs.GetFloat("SFXMixerVolume",0);

        audioMixer.SetFloat("BGM_Volume",BGMMixerVolume);
        audioMixer.SetFloat("SFX_Volume",SFXMixerVolume);

        BGMMixerVolumeSlider.value = BGMMixerVolume;
        SFXMixerVolumeSlider.value = SFXMixerVolume;
    }

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    void UpdateResetGroupAlpha(float alpha) => resetGroup.GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenSetting()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f);
    }
    
    public void CloseSetting() => LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    });

    public void ChangeBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM_Volume",value);
        PlayerPrefs.SetFloat("BGMMixerVolume",value);
    }

    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX_Volume",value);
        PlayerPrefs.SetFloat("SFXMixerVolume",value);
    }

    public void ResetGame()
    {
        StartCoroutine(ShowReset());
        PlayerPrefs.SetInt("LevelReached", 1);
        PlayerPrefs.SetInt("Health", 3);
    }

    IEnumerator ShowReset()
    {
        resetGroup.GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(resetGroup, UpdateResetGroupAlpha, 0.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(1.2f);
        LeanTween.value(resetGroup, UpdateResetGroupAlpha, 1.0f, 0.0f, 0.5f);
        resetGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
