using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgManager : MonoBehaviour
{

    public GameObject msgPanel;
    public Text msgText;

    public float timer;
    

    // Start is called before the first frame update
    void Start()
    {
        msgText.text = " ";
        msgPanel.SetActive(false);
    }

    void Update() {

        timer -= 1f * Time.deltaTime;
        if (timer < 0) {
            msgPanel.SetActive(false);
        }
    }


    public static void SetTheMsg(string msg, float time = 15f) {
        GameObject.Find("GameManager").GetComponent<MsgManager>().SetMsg(msg, time);
    }

    public void SetMsg(string msg, float time) {
        string oldMsg = "";
        if (timer > 0) {
            oldMsg = " <color=orange>+++</color> " + msgText.text;
        }
        timer = time;
        msgPanel.SetActive(true);
        msgText.text = msg + oldMsg;

        if (GameManager.debugIsOn) {
            Debug.Log("The message is: " + msg);
        }
        
    }

    public void CloseTheMessagePanel() {
        timer = 0f;
        msgPanel.SetActive(false);
    }
    
}
