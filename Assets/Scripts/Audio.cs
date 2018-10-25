using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {
    private int bpm = 120;


	// Use this for initialization
	void Start () {
        var sources = gameObject.GetComponentsInChildren(typeof(AudioSource));
        double startTime = AudioSettings.dspTime + 2;
        foreach (AudioSource source in sources)
        {
            source.PlayScheduled(startTime);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
