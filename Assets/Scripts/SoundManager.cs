using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;
    private DeliveryCounter _deliveryCounter;

    private float _soundEffectsVolume = 1f;
    private void Awake()
    {
        Instance = this;
        _soundEffectsVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        _deliveryCounter = DeliveryCounter.Instance;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAynCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(_audioClipRefsSO.Trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(_audioClipRefsSO.ObjectDrop, baseCounter.transform.position);

    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSO.ObjectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAynCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_audioClipRefsSO.Chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSO.DeliveryFail, _deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSO.DeliverySuccess, _deliveryCounter.transform.position);
    }


    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {

        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume );
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {

        AudioSource.PlayClipAtPoint(audioClip, position, volume * _soundEffectsVolume);
    }
    public void PlayFootstepsSound(Vector3 position, float volume)
    { 
        PlaySound(_audioClipRefsSO.Footstep, position, volume);
    }
    /// <summary>
    /// For when clicking on the sound effects button
    /// </summary>
    public void ChangeVolume()
    {
        _soundEffectsVolume += 0.1f;
        if(_soundEffectsVolume > 1.05f)
        {
            _soundEffectsVolume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _soundEffectsVolume);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// For when changing the volume slider
    /// </summary>
    /// <param name="value"></param>
    public void SetVolume(float value)
    {
        _soundEffectsVolume = value;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _soundEffectsVolume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _soundEffectsVolume;
    }
}
