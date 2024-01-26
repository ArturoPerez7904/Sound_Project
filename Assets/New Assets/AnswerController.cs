using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour
{
    public GameObject ConnectionButton;
    public List<Button> buttons = new List<Button>{};
    public GameObject[] objs;
    public GameObject client;
    private bool connectionStatus;

    private void Awake()
        {
            AddListeners();
        }

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag("Selectable");
        client = GameObject.FindGameObjectWithTag("Client");
        ConnectionButton = GameObject.FindGameObjectWithTag("Start");

        AddStartListener();
        
    }

    void BeginTest(){
        connectionStatus = client.GetComponent<ClientScript>().ConnectToServer();
        if(connectionStatus){

            Destroy(ConnectionButton);

        }
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

    void AddListeners()
    {
        int i = 0;
        foreach (Button btn in buttons)
        {
            ListenerHandler(btn, i);

            i++;
        }

        i = 1;
    }

    private void ListenerHandler(Button btn, int index)
        {
            btn.onClick.AddListener(() => { AnswerListener(index); });
        }

    private void AnswerListener(int index)
        {
            int answer = 0;
            switch(index){
                case 8:
                    answer = 5;
                    break;
                case 7:
                    answer = 4;
                    break;
                case 6:
                    answer = 3;
                    break;
                case 5:
                    answer = 2;
                    break;
                case 4:
                    answer = 1;
                    break;
                case 3:
                    answer = 9;
                    break;
                case 2:
                    answer = 8;
                    break;
                case 1:
                    answer = 7;
                    break;
                case 0:
                    answer = 6;
                    break;
            }

            string message = "" + answer;

            client.GetComponent<ClientScript>().SendMessageToServer(message);
        }

    private void AddStartListener()
    {
        Button btn = ConnectionButton.GetComponentInChildren<Button>();
        btn.onClick.AddListener(BeginTest);
    }


}
