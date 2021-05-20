using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchField : MonoBehaviour , IPointerDownHandler , IPointerUpHandler 
{
    [HideInInspector] Vector2 TouchDist;
    [HideInInspector] Vector2 PointerOld;
    [HideInInspector] int PointerId;
    [HideInInspector] bool pressed;



    // Update is called once per frame
    void Update()
    {
        if(pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }

        else
        {
            TouchDist = new Vector2();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;

    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;

    }
    public float mouseX()
    {
        return TouchDist.x;

    }
    public float mouseY()
    {
        return TouchDist.y;

    }

    
}
