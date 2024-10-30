using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingCustomAction : MonoBehaviour
{
    public GameObject objectToHide;
    private void OnMouseDown()
    {
        Debug.Log($"inside PaintingCustomAction onMouseDown()");
        if (objectToHide != null)
        {
            objectToHide.SetActive(!objectToHide.activeSelf);
        }
        Debug.Log($"exiting PaintingCustomAction onMouseDown()");
    }

}
