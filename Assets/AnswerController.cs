using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AnswerController : MonoBehaviour
{
    public List<Button> buttons = new List<Button>{};
    private int answer;
    public List<int> correctAnswerList = new List<int>{};
    private int correctAnswer;
    List<int> answers = new List<int>{};
    public int finalAnswer;
    private float testStartTime;
    private float sentencePairStartTime;
    private float timeToAnswer;

    public GameObject[] objs;
    [SerializeField] private float[] timeStops;
    [SerializeField] private float stopTime;
    

    private void Awake()
        {
            AddListeners();
        }


    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag("Selectable");
        DeactivateAllButtons();
        testStartTime = Time.time;
        //add 0 to the answers list as a default to check if there is no answer
        answers.Add(0); 
        BeginTest(); 
		
    }

    void BeginTest(){
        int i = 0;

        //Must be even i value or will throw error
        foreach (float stopTime in timeStops){
            if (i%2==0){
                Invoke("ActivateAllButtons", timeStops[i]);
                sentencePairStartTime = Time.time;
                i++;
            }
            else{
                Invoke("DeactivateAllButtons", timeStops[i]);
                Invoke("GiveFinalAnswer", timeStops[i]);
                i++;
            }
        }   
    }

    void GiveFinalAnswer(){
        finalAnswer = answers.Last();
        int i = 0;

        if (finalAnswer == 0){
            Debug.Log ("No Answer Recorded");
            Debug.Log ("Time to answer is null");
            i++;
        }
        else{
            Debug.Log ("Final Answer: " + finalAnswer);
            Debug.Log("Time to answer: " + timeToAnswer);
            int correctAnswer = correctAnswerList[i];
            // int difference = (Mathf.Abs(finalAnswer - correctAnswer));
            // int degreeOffSet = difference * 22.5;
            // Debug.Log("Summary: ");
            i++;
        }
	}

    private void ListenerHandler(Button btn, int index)
        {
            btn.onClick.AddListener(() => { AddAnswer(index); });
        }
 
        private void AddAnswer(int index)
        {
            Debug.Log($"index = {index}");
            answer = index + 1;
            answers.Add(answer);
            timeToAnswer = Time.time - testStartTime;
            finalAnswer = answers.Last();

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
    }

}
