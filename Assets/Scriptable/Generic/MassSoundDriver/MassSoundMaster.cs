using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MassSoundMaster : MonoBehaviour
{

    public int NumberOfSources;

    private List<AudioSource> emitters;

    private List<AudioRequest> queue;

	// Use this for initialization
	void Start ()
	{

	    MassSoundSubscriber.master = this;
        emitters = new List<AudioSource>();
        queue = new List<AudioRequest>();

	    for (int i = 0; i < NumberOfSources; i++)
	    {
	        var thing = new GameObject($"{this.name}-Emitter-{i}");
	        thing.transform.position = this.transform.position;
	        var comp = thing.AddComponent<AudioSource>();
	        comp.loop = false;
            emitters.Add(comp);
	    }

	}
	
	// Update is called once per frame
	void Update () {

	    foreach (var emitter in emitters)
	    {
	        if (!emitter.isPlaying && queue.Any())
	        {
	            var request = queue[0];
	            emitter.transform.position = request.position;
                emitter.PlayOneShot(request.sound, request.loudness);
	            queue.Remove(request);
	        }
	    }

	    if (queue.Any())
	    {
	        queue = new List<AudioRequest>();
	    }
	}

    public void playSound(AudioClip clip, Vector3 pos, float soundScale = .8f)
    {
        queue.Add(new AudioRequest(pos, clip, soundScale));
    }

    private class AudioRequest
    {
        public AudioRequest(Vector3 pos, AudioClip noise, float volume = .8f)
        {
            position = pos;
            sound = noise;
            played = false;
            loudness = volume;
        }

        public Vector3 position;
        public AudioClip sound;
        public bool played;
        public float loudness;
    }
}
