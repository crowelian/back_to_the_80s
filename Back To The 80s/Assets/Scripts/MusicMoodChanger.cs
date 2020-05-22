using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMoodChanger : MonoBehaviour
{

    private MusicPlayer mPlayer;
    public int selectMood = 0;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && other.gameObject.tag != "RoadSpawner" && other.gameObject.tag != "RoadDestroyer") {
            if (GameManager.debugIsOn) {
                Debug.Log("<color=red>Player Hit MOOD CHANGE!</color>");
            }
                if (mPlayer.musicMoodChangeOn == false) {
                    mPlayer.MoodChange(selectMood);
                    if (GameManager.debugIsOn) {
                        Debug.Log("<color=green>Mood change to: </color>"+selectMood);
                    }
                }
        }
    }
    
}
