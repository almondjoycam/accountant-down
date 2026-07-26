using UnityEngine;

public class ApartmentDesk : Interactable
{
    [SerializeField] GameObject storeUI;

    new void Start()
    {
        uiObject = storeUI;
        base.Start();
    }

    public override void Interact()
    {
        base.UIInteract();
    }
}
