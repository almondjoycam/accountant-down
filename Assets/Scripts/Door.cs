using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour, IInteractable
{
    [Header("UI reference for choosing where to go")]
    [SerializeField] GameObject doorUI;

    InputActionMap player;
    InputActionMap ui;
    InputAction cancel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorUI.SetActive(false);
        player = InputSystem.actions.FindActionMap("Player");
        ui = InputSystem.actions.FindActionMap("UI");

        cancel = ui.FindAction("Cancel");
        cancel.performed += OnCancel;
    }

    public void Interact()
    {
        doorUI.SetActive(true);
        player.Disable();
        ui.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        doorUI.SetActive(false);
        ui.Disable();
        player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
