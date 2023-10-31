using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonToConsole : MonoBehaviour
{
	public Button yourButton;
	private string buttonString;
	public TextMeshProUGUI textMeshProUGUI;

	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		buttonString = textMeshProUGUI.text;
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log (buttonString);
	}
}
