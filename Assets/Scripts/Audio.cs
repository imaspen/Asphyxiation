using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {
    private int bpm = 120;
    private AudioSource audioSource;
    public Transform oxygenBar;

    public AudioSource[] tracks = new AudioSource[4];

	// Use this for initialization
	void Start () {
        tracks[0].Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var scale = oxygenBar.localScale.x;
        if (scale < .75 && scale >= .5)
        {
            if (!tracks[1].isPlaying)
            {
                tracks[0].Stop();
                tracks[1].Play();
            }
        }
        else if (scale < .5 && scale >= .25)
        {
            if (!tracks[2].isPlaying)
            {
                tracks[1].Stop();
                tracks[2].Play();
            }
        }
        else if (scale < .25 && scale >= 0)
        {
            if (!tracks[3].isPlaying)
            {
                tracks[2].Stop();
                tracks[3].Play();
            }
        }
	}
}
