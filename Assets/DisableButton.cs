using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisableButton : MonoBehaviour
{
    public Button yourButton;
	public GameObject[] objs;

	void Start () {
		yourButton.GetComponent<Button>();
		yourButton.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		yourButton.interactable = false;
		//DeactivateAllButtons();
        Invoke("MakeButtonInteractable", 2);
	}
    void MakeButtonInteractable(){
        yourButton.interactable = true;
    }

	// public void DeactivateAllButtons()
	// {
 
    // objs = GameObject.FindGameObjectsWithTag("Selectable");
 
    // foreach (GameObject button in objs)
	// {
	// btn = button.GetComponent<Button>();
	// btn.interactable = false;

	// }
 
	// }
	
}
