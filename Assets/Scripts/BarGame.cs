using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class BarGame : MonoBehaviour
{

    float drunkeness = 0f;
    float rotation_state = 0f;
    int direction = 1;
    float max_angle = 30;
    float slider_state = 0f;
    float slider_bounds = 7.4f;
    int slider_direction = 1;


    UniversalOverlayScript ui;
    //controls

    InputActionMap drinkingControlMap;

    InputAction drink;
    InputAction leave;


    Transform slider;
    /** TO DOS:
        Accept Drink input
        Correct timing
        Done - Rotate interface
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
        ui = FindAnyObjectByType<UniversalOverlayScript>();

        drinkingControlMap = InputSystem.actions.FindActionMap("Drinking");
        drink = drinkingControlMap.FindAction("Drink");
        leave = drinkingControlMap.FindAction("Leave");

        Debug.Log(drinkingControlMap);
        

        drink.performed += OnDrink;
        leave.performed += OnLeave;
        
        Debug.Log(drink);
        Debug.Log(leave);
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
        float rot_amount = 7 * direction * drunkeness * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rot_amount));
        rotation_state += rot_amount;
        if (rotation_state > max_angle) direction = -1;
        if (Math.Abs(rotation_state) > max_angle && rotation_state < 0) direction = 1;
    }


    void MoveSlider()
    {
        float slider_movespeed = slider_direction * (5.3f + drunkeness*.75f) * Time.deltaTime;

        slider.Translate(new Vector3(slider_movespeed, 0, 0));
        slider_state += slider_movespeed;
        

        if (slider_state > slider_bounds) slider_direction = -1;
        if (Math.Abs(slider_state) > slider_bounds && slider_state < 0) slider_direction = 1;

    }

    void OnDrink(InputAction.CallbackContext context)
    {
        Debug.Log(slider_state);
        if(Math.Abs(slider_state) < 1.3f) {
            drunkeness++;
            //trigger happiness
            float hapiness_from_drink = UnityEngine.Random.Range(-1.0f *(.6f + .52f * drunkeness), .5f + .5f * drunkeness) *(drunkeness *.1f);
            if (hapiness_from_drink > 0) hapiness_from_drink = hapiness_from_drink * 1.5f;
            ui.ChangeHappiness(hapiness_from_drink);
            

            ui.ChangeMoney(-1); //costs a dollar

            //play vampire animation
        } else
        {
            LoseDrinking();
        }
    }

    void LoseDrinking()
    {
        //load apartment scene
        //play sound effect?
    }

    void OnLeave(InputAction.CallbackContext context)
    {
        LeaveGame();
    }
    void LeaveGame()
    {

        Debug.Log("Drinking Game Over");        
    }

    
}
