using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerScript : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private RectTransform rectTr;
    private Image image;

    private void Awake()
    {
        rectTr = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(target.position);
        rectTr.position = screenPoint;

        var viewportPoint = Camera.main.WorldToViewportPoint(target.position);
        var disFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);

        var show = disFromCenter < 0.3f;

        if (screenPoint.z < 0)
            show = false;
        image.enabled = show;
    }
}
