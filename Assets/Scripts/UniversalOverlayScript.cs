using System;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalOverlayScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float money = 13.37f;
    float happiness = 6.67f;
    bool at_work = false;

    float starting_time = 9f;
    float time_remaining = 9f;

    static bool already_made = false;

    [SerializeField] TextMeshProUGUI time_remaining_display;
    [SerializeField] TextMeshProUGUI money_display;
    [SerializeField] TextMeshProUGUI happiness_display;
    
    double marked_time;//only 1 happiness decay /time value


    void Start()
    {
        //only 1 universal overlay
        if (already_made)
        {
            Destroy(gameObject);
            return;
        }

        already_made = true;

        money_display.text = "$" + money.ToString();
        happiness_display.text = happiness.ToString();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time_remaining = starting_time - Time.time;
        if (time_remaining <=0) EndGame();//endgame
        if (Math.Round(time_remaining) % 20 == 0) {
            HappinessDecay(Math.Round(time_remaining));
            
            marked_time = Math.Round(time_remaining);

        }
        UpdateTimeDisplay();
    }

    public void SetWorking(bool working)
    {
        at_work = working;
    }

    private void HappinessDecay(double time)
    {
        if (marked_time == time) return;
        if(at_work) ChangeHappiness(-1f);
        ChangeHappiness(-1f);
    }
    private void Paycheck(double time)
    {
        if (marked_time == time) return;
        if (at_work) ChangeMoney(25.5f);
    }
    private void EndGame()
    {
        if (money < 1200)
        {
            SceneManager.LoadScene("Evicted");
        }
        SceneManager.LoadScene("Apartment");
        GameObject.FindGameObjectWithTag("Win Sprite").SetActive(true);
    }

    private void UpdateTimeDisplay()
    {
        int minutes = (int)time_remaining/ 60;
        String minutes_text = minutes.ToString();
        int seconds = (int)time_remaining % 60;
        String seconds_text = seconds.ToString();
        if (seconds / 10 == 0) seconds_text = "0" + seconds_text;
        time_remaining_display.text = "Rent Due: " +  minutes_text + ":" + seconds_text;

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

        if (happiness < 0) {
            SceneManager.LoadScene("Depression");
            Debug.Log("Depression.");}//trigger depression ending

    }


}
