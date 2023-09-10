using System;
using UnityEngine;
using TMPro;

/*
Class keeps track of time passing
*/
public class TimeObserver : MonoBehaviour
{

    private bool isTimeStarted = false;
    public bool IsTimeStarted
    {
        get { return isTimeStarted; }
        set { isTimeStarted = value;  }
    }
    
    private static float seconds = 0f;
    public static float Seconds { get{ return seconds;} private set { seconds = value;}}

    private static float minutes = 0f;
    public static float Minutes { get{ return minutes;} private set { minutes = value;}}
    
    public TextMeshProUGUI inGameTimetext;

    
    private float timer;

    void Start()
    {
        Minutes = 0f;
        Seconds = 0f;
        timer = 1; //countdown 
        UpdateUITime();

    }

    void Update()
    {
        if (!IsTimeStarted) return;
        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Seconds++;
            if(Seconds >= 60)
            {
                Seconds = 0;
                Minutes++;
            }
            UpdateUITime();
            timer = 1;
        }
    }

    private void UpdateUITime()
    {
        inGameTimetext.text = $"{TimeObserver.Minutes:00} : {TimeObserver.Seconds:00}";
    }
}
