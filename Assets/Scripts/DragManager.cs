using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image img;
    private Transform gridLayoutParent;
    private int initialSiblingIndex;
    private int targetSiblingIndex;
    private GameObject spacer;

    private OfficeGame officeGame;
    int start_index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        officeGame = GameObject.FindAnyObjectByType<OfficeGame>();
        img = GetComponent<Image>();
        initialSiblingIndex = transform.GetSiblingIndex();
        gridLayoutParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.raycastTarget = false;
        start_index = transform.GetSiblingIndex(); // used for OfficeGame
        transform.SetParent(gridLayoutParent.parent);
        spacer = new GameObject("", typeof(RectTransform));
        spacer.transform.SetParent(gridLayoutParent, false);
        spacer.transform.SetSiblingIndex(initialSiblingIndex);
        targetSiblingIndex = initialSiblingIndex;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        if (eventData.pointerEnter != null && eventData.pointerEnter != gameObject)
        {
            targetSiblingIndex =
                eventData.pointerEnter.transform.GetSiblingIndex();
            spacer.transform.SetSiblingIndex(targetSiblingIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        //grab values for office game
        

        //Camber code
        transform.SetParent(gridLayoutParent);
        transform.SetSiblingIndex(targetSiblingIndex);
        img.raycastTarget = true;
        Destroy(spacer);

        officeGame.UpdateGame(start_index, targetSiblingIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
