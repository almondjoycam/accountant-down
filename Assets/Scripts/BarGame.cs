using System;
using UnityEngine;

public class BarGame : MonoBehaviour
{

    float drunkeness = 1f;
    float rotation_state = 0f;
    int direction = 1;
    float max_angle = 30;
    float slider_state = 0f;
    float slider_bounds = 15f;
    int slider_direction = 1;

    Transform slider;
    /** TO DOS:
        Accept Drink input
        Correct timing
        Rotate interface
        Move inidcator for timing
        Progress drinks stage
        Leave minigame
        Beneift for winning?
        Play drink animation
    */
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = transform.GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        RotateBar();  
        MoveSlider(); 
    }

    //called each frame, relies on 'drunkeness' meter
    void RotateBar()
    {
        float rot_amount = 3 * direction * drunkeness * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rot_amount));
        rotation_state += rot_amount;
        if (Math.Abs(rotation_state) > max_angle) direction = direction * -1;
    }


    void MoveSlider()
    {
        float slider_movespeed = slider_direction * 5.3f * Time.deltaTime;

        slider.Translate(new Vector3(slider_movespeed, 0, 0));
        slider_state += slider_movespeed;
        if (Math.Abs(slider_state) > slider_bounds) slider_direction = slider_direction * -1;
        

    }
}
