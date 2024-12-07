using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class SoundItem
{
    public string name;
    public AudioClip clip;
}
public class Sound_Manager : MonoBehaviour
{
    // Singleton instance
    private static Sound_Manager instance;

    // List to store sound items
    public List<SoundItem> soundItems = new List<SoundItem>();

    // AudioSource component to play audio
    private AudioSource audioSource;

    // Method to get the singleton instance
    public static Sound_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject soundManagerObject = new GameObject("Sound_Manager");
                instance = soundManagerObject.AddComponent<Sound_Manager>();
                DontDestroyOnLoad(soundManagerObject);
            }
            return instance;
        }
    }

    // Method to play audio clip by name
    public void PlaySound(string clipName, float volume = 1f)
    {
        SoundItem soundItem = soundItems.Find(item => item.name == clipName);

        if (soundItem != null && soundItem.clip != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.PlayOneShot(soundItem.clip, volume);
        }
        else
        {
            Debug.LogWarning("Clip with name " + clipName + " not found.");
        }
    }

    // Method to stop playing audio
    public void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Method to destroy the singleton instance
    private void OnDestroy()
    {
        instance = null;
    }
}
