using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 offset; // Offset between mouse and object position
    private bool is_dragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        is_dragging = true;

        transform.SetAsLastSibling();

        // Calculate the offset between the object's position and the mouse position
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform.parent,
            Input.mousePosition,
            eventData.pressEventCamera,
            out Vector3 worldMousePos
        );
        offset = transform.position - worldMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!is_dragging) return;

        // Update the position of the object based on the mouse position plus the offset
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform.parent,
            Input.mousePosition,
            eventData.pressEventCamera,
            out Vector3 worldMousePos
        );
        transform.position = worldMousePos + (Vector3)offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Cursor.SetCursor(Resources.Load<Texture2D>("hand_cursor"), Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
