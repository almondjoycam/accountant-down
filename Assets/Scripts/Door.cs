using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    [SerializeField] GameObject doorUI;

    new void Start()
    {
        uiObject = doorUI;
        base.Start();
    }

    public override void Interact()
    {
        base.UIInteract();
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
