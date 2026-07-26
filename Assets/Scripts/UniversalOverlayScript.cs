using System;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;

public class UniversalOverlayScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float money = 1.37f;
    float happiness = 1.67f;

    float starting_time = 6000f;
    float time_remaining = 6000f;

    [SerializeField] TextMeshProUGUI time_remaining_display;
    [SerializeField] TextMeshProUGUI money_display;
    [SerializeField] TextMeshProUGUI happiness_display;
    


    void Start()
    {
        money_display.text = "$" + money.ToString();
        happiness_display.text = happiness.ToString();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time_remaining = starting_time - Time.time;
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        int minutes = (int)time_remaining/ 60;
        String minutes_text = minutes.ToString();
        int seconds = (int)time_remaining % 60;
        String seconds_text = seconds.ToString();
        time_remaining_display.text = "Time " +  minutes_text + ":" + seconds_text;

    }

    public void ChangeMoney(float amount)
    {
        money += amount;
        //only displayed to cents - should never really be required
        double money_whole = Math.Round((double)money * 100);
        money_whole = money_whole / 100;

        money_display.text = "$" + money_whole.ToString();
    }

    public void ChangeHappiness(float amount)
    {
        happiness += amount;
        //cut happiness display off at 2 decimal places
        double happiness_whole = Math.Round((double)happiness * 100);
        happiness_whole = happiness_whole / 100;

        happiness_display.text = happiness_whole.ToString();

        if (happiness < 0) Debug.Log("Depression.");//trigger depression ending

    }


}
