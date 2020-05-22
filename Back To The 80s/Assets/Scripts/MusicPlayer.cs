using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public AudioSource[] tracks;
    private float percussionBreak = 10.2f;

    public bool musicMoodChangeOn = false;


    // Start is called before the first frame update
    void Start()
    {
        PlayTracks();
        StartCoroutine(PercBreak());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoodChange(int mood) {
        if (mood == 0) {
            musicMoodChangeOn = true;
            StartCoroutine(PercBreak());
        } else if (mood == 2) {
            // Nothing yet...
        }
        
    }

    IEnumerator PercBreak() {
        StopPercussionAndBass();
        yield return new WaitForSeconds(percussionBreak);
        PlayPercussionAndBass();
        musicMoodChangeOn = false;
    }

    public void PlayTracks() {
        for (int p = 0; p < tracks.Length; p++) {
            tracks[p].Play();
        }
    }

    public void StopTracks() {
        for (int s = 0; s < tracks.Length; s++) {
            tracks[s].Stop();
        }
    }

    public void StopPercussionAndBass() {
        musicMoodChangeOn = true;
        tracks[3].volume = 0;
        tracks[4].volume = 0;
        tracks[5].volume = 0;
    }

    public void PlayPercussionAndBass() {
        musicMoodChangeOn = false;
        tracks[3].volume = 1;
        tracks[4].volume = 1;
        tracks[5].volume = 1;
    }


    public void EnergyAlmosGone() {
        musicMoodChangeOn = true;
        tracks[0].volume = 0;
        tracks[1].volume = 0;
        tracks[2].volume = 0;
        tracks[3].volume = 1;
        tracks[4].volume = 1;
        tracks[5].volume = 1;
    }

    public void NormalSetup() {
        musicMoodChangeOn = false;
        tracks[0].volume = 1;
        tracks[1].volume = 1;
        tracks[2].volume = 1;
        tracks[3].volume = 1;
        tracks[4].volume = 1;
        tracks[5].volume = 1;
    }


}
