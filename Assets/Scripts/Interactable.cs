using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour
{
    protected GameObject uiObject;
    InputActionMap player;
    InputActionMap ui;
    InputAction cancel;

    protected void Start()
    {
        player = InputSystem.actions.FindActionMap("Player");
        ui = InputSystem.actions.FindActionMap("UI");

        cancel = ui.FindAction("Cancel");
        cancel.performed += OnCancel;
        uiObject?.SetActive(false);
    }


    void OnCancel(InputAction.CallbackContext context)
    {
        uiObject?.SetActive(false);
        ui.Disable();
        player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void UIInteract()
    {
        uiObject?.SetActive(true);
        player.Disable();
        ui.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public abstract void Interact();
}
