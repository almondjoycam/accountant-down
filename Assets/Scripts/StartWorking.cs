using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWorking : Interactable
{
    public override void Interact()
    {
        base.UIInteract();
        SceneManager.LoadScene("Scenes/OfficeGame");
    }
}
