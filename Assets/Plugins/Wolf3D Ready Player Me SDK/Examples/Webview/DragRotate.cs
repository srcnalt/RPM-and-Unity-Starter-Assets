using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour, IDragHandler
{
    private GameObject avatar;

    public void OnDrag(PointerEventData eventData)
    {
        if (avatar)
        {
            avatar.transform.Rotate(Vector3.up, -eventData.delta.x);
        }
        else
        {
            avatar = GameObject.Find("Avatar");
        }
    }
}
