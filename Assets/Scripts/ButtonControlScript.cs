using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControlScript : MonoBehaviour
{
    public UnityEvent buttonEvent = new UnityEvent();
    public GameObject button;
    void Start()
    {
        button = this.gameObject;
    }
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray,out hit) && hit.collider.gameObject == gameObject)
            {
                buttonEvent.Invoke();
            }
        }

    }
}
