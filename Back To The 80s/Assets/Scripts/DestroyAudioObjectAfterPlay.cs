using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioObjectAfterPlay : MonoBehaviour
{

    private AudioSource thisAudio;

    // Start is called before the first frame update
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        InvokeRepeating("CheckIfAudioEnded", 1f, 1f);
    }

    void CheckIfAudioEnded() {
        if (thisAudio.isPlaying) {
            // nothing...
        } else {
            Destroy(gameObject);
        }
    }

}
