using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [Header("Sounds")]
    [SerializeField] KeyValueSound[] kvSounds;
    [SerializeField] KeyValuesSounds[] kvsSounds;

    Dictionary<string, EventInstance> eventInstancesDict = new Dictionary<string, EventInstance>();
    Dictionary<string, EventInstance[]> eventsInstancesDict = new Dictionary<string, EventInstance[]>();
    
    //initialization
    void Start()
    {
        foreach (KeyValueSound kvSound in kvSounds) eventInstancesDict.Add(kvSound.name, CreateInstance(kvSound.sound));
        foreach (KeyValuesSounds kvsSound in kvsSounds)
        {
            int n = kvsSound.sounds.Length;
            EventInstance[] eventInstances = new EventInstance[n];

            for (int i = 0; i < n; i++) eventInstances[i] = CreateInstance(kvsSound.sounds[i]);

            eventsInstancesDict.Add(kvsSound.name, eventInstances);
        }
    }

    
    
    EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }

    //main methods
    public void PlayOneShot(string sound) => eventInstancesDict[sound].start();
    public void PlayOneShot(EventInstance sound) => sound.start();

    public void PlayOneShot(string sound, int i) => eventsInstancesDict[sound][i].start();

    //settings
    public void SetVolume(int index, float volume)
    {
        RuntimeManager.GetBus((index == 0? "bus:/SFX" : "bus:/MUSIC")).setVolume(volume);
        Settings.musicStats[index] = volume;
    }
}

[System.Serializable]
class KeyValueSound
{
    [SerializeField] public string name;
    [SerializeField] public EventReference sound;
}

[System.Serializable]
class KeyValuesSounds
{
    [SerializeField] public string name;
    [SerializeField] public EventReference[] sounds;
}
