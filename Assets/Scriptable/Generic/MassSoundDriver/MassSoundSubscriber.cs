using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSoundSubscriber : MonoBehaviour {

    public static MassSoundMaster master;

    public List<SoundEntries> sounds;

    private Dictionary<string, SoundEntries> soundLookup;

    // Use this for initialization
	void Start () {
		soundLookup = new Dictionary<string, SoundEntries>();
	    foreach (var sound in sounds)
	    {
	        soundLookup[sound.soundName] = sound;
	    }
	}

    public void playSound(Vector3 pos, string soundName)
    {
        if (soundLookup.ContainsKey(soundName))
        {
            master.playSound(soundLookup[soundName].sound ,pos, soundLookup[soundName].soundScale);

        }
        else
        {
            Debug.LogError($"Could not find sound {soundName}");
        }
    }

    [Serializable]
    public class SoundEntries
    {
        public string soundName;
        public AudioClip sound;
        public float soundScale;
    }
}
