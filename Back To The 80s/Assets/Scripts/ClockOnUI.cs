using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClockOnUI : MonoBehaviour
{

    public Text clockText;
    public bool showOnlySecsPassd = false;

    // Start is called before the first frame update
    void Start()
    {
        clockText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showOnlySecsPassd) {
            clockText.text = Time.time.ToString("00");
        } else {
            DateTime time = DateTime.Now;
            string hour = LeadingZero(time.Hour);
            string minute = LeadingZero(time.Minute);
            string second = LeadingZero(time.Second);

            clockText.text = hour + ":" + minute + ":" + second;
        }
        
    }

    string LeadingZero(int t) {
        return t.ToString().PadLeft(2, '0');
    }
}
