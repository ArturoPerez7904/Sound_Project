using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DisableAllButtons : MonoBehaviour
{
    public GameObject[] objs;
    [SerializeField] private float[] timeStops;
    [SerializeField] private float stopTime;
    
    void Start()
    {
           objs = GameObject.FindGameObjectsWithTag("Selectable");
           Invoke("DeactivateAllButtons", timeStops[0]);
           Invoke("ActivateAllButtons", timeStops[1]);
    }

    public void DeactivateAllButtons()
	{
        foreach (GameObject selectable in objs){
            Button button;
	        button = selectable.GetComponentInChildren<Button>();
	        button.interactable = false;
        }
    }

    public void ActivateAllButtons()
	{
        foreach (GameObject selectable in objs){
            Button button;
	        button = selectable.GetComponentInChildren<Button>();
	        button.interactable = true;
        }
    }


}
