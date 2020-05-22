using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{

    public float waitTimer;
    public Slider loadBar;
    public Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        loadBar.maxValue = waitTimer;
    }

    // Update is called once per frame
    void Update()
    {
        waitTimer -= 1f * Time.deltaTime;
        loadBar.value += 1f * Time.deltaTime;

        if (waitTimer <= 1.2f) {
            loadingText.text = "ALMOST DONE";
        }
        if (waitTimer < 0.6f) {
            loadingText.text = "DONE";
        }
        if (waitTimer < 0f) {
            
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
