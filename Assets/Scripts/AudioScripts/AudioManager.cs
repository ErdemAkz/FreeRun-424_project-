using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    public static float masterVolume { get; private set; }
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

    //[SerializeField] private TextMeshProUGUI musicSliderText;
    //[SerializeField] private TextMeshProUGUI soundEffectsSliderText;
    private void Awake()
    {
        Instance = this;

        float tempMaster;
        float tempMusic;
        float tempSfx;

        masterMixerGroup.audioMixer.GetFloat("Master Volume", out tempMaster);
        musicMixerGroup.audioMixer.GetFloat("Music Volume", out tempMusic);
        soundEffectsMixerGroup.audioMixer.GetFloat("Sound Effects Volume", out tempSfx);

        masterVolume = Mathf.Pow(10, (tempMaster / 20.0f));
        musicVolume = Mathf.Pow(10, (tempMusic / 20.0f));
        soundEffectsVolume = Mathf.Pow(10, (tempSfx / 20.0f));
    }

    public void UpdateMixerVolume()
    {
        masterMixerGroup.audioMixer.SetFloat("Master Volume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(soundEffectsVolume) * 20);

        //soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(soundEffectsVolume) * 20);
    }

    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        Instance.UpdateMixerVolume();
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        Instance.UpdateMixerVolume();
        //soundEffectsSliderText.text = ((int)(value * 100)).ToString();
    }

    public float GetMusicVolumeLevel()
    {
        return masterVolume * musicVolume;
    }

    public float GetSfxVolumeLevel()
    {
        return masterVolume * soundEffectsVolume;
    }
}
