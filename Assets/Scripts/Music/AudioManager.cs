using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    /*
    [field: Header("Ambience")]

    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Rain { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Enverenment")]
    [field: SerializeField] public EventReference RandomSFX { get; private set; }
    [field: SerializeField] public EventReference Thunder { get; private set; }
    [field: SerializeField] public EventReference Exit { get; private set; }


    [field: Header("Player")]

    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference PlayerStepSoundOnWater { get; private set; }
    [field: SerializeField] public EventReference DieSound { get; private set; }

    [field: Header("Enemy")]


    [field: SerializeField] public EventReference DefIdle { get; private set; }
    [field: SerializeField] public EventReference DefJump { get; private set; }

    [field: SerializeField] public EventReference BlindIdle { get; private set; }
    [field: SerializeField] public EventReference BlindJump { get; private set; }

    [field: Header("UI")]

    [field: SerializeField] public EventReference ButtonPress { get; private set; }
    */

    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();
    }

    void Start()
    {
        /*
        EventInstancesDict.Add("Music", CreateInstance(FMODManager.Instance.Music)); 
        EventInstancesDict.Add("RandomSFX", CreateInstance(FMODManager.Instance.RandomSFX));
        EventInstancesDict.Add("Ambience", CreateInstance(FMODManager.Instance.Ambience));
        EventInstancesDict.Add("Rain", CreateInstance(FMODManager.Instance.Rain));

        EventInstancesDict.Add("ButtonPress", CreateInstance(FMODManager.Instance.ButtonPress));
        EventInstancesDict.Add("Exit", CreateInstance(FMODManager.Instance.Exit));
        //EventInstancesDict.Add("PlaySound", CreateInstance(FMODManager.Instance.PlaySound));
        //EventInstancesDict.Add("GameOverSound", CreateInstance(FMODManager.Instance.GameOverSound));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(FMODManager.Instance.PlayerStepSound));
        EventInstancesDict.Add("DieSound", CreateInstance(FMODManager.Instance.DieSound));

        EventInstancesDict.Add("PlayerStepSoundOnWater", CreateInstance(FMODManager.Instance.PlayerStepSoundOnWater));
        */
    }

    public void PlayOneShot(string sound)
    {
        EventInstancesDict[sound].start();
    }
    public void PlayOneShot(EventReference sound, Vector2 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }

    public void SetVolume(int index, float volume)
    {
        RuntimeManager.GetBus((index == 0? "bus:/SFX" : "bus:/MUSIC")).setVolume(volume);
        Settings.musicStats[0] = volume;
    }
}