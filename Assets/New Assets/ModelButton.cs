using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModelButton : MonoBehaviour
{
    
    void Start(){
        //Invoke("ActivateObject", 3f);
        gameObject.SetActive(true);
    }

    void ActivateObject(){
        gameObject.SetActive(false);
    }
    
}
