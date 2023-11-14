using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class AnswerController : MonoBehaviour
{

    string filename="";
    public List<Button> buttons = new List<Button>{};
    private int currentAnswer;
    List<int> answers = new List<int>{};
    List<double> degreeOffsetList = new List<double>{};
    List<float> timeToAnswerList = new List<float>{};
    [SerializeField] List<int> correctAnswers = new List<int>{};
    private float timeToAnswer;
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    public GameObject[] objs;
    public GameObject starter;
    [SerializeField] private float[] timeStops;
    [SerializeField] private float stopTime;
    private int answerCounter = 0;
    

    private void Awake()
        {
            AddListeners();
        }

    void Start()
    {
        filename = Application.dataPath + "/test.csv";
        objs = GameObject.FindGameObjectsWithTag("Selectable");
        DeactivateAllButtons();
        AddStartListener();
        
    }

    void BeginTest(){
        Destroy(starter);
        int i = 0;

        foreach (float stopTime in timeStops){
            if (i%2==0){
                Invoke("ActivateAllButtons", timeStops[i]);
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
        answers.Add(currentAnswer);
        int correctAnswer = correctAnswers[answerCounter];
        if (currentAnswer == 0){
            Debug.Log ("No Answer Recorded");
            degreeOffsetList.Add(0);
            timeToAnswerList.Add(0);
        }
        else{
            double difference = Mathf.Abs(currentAnswer - correctAnswers[answerCounter]);
            double degreeOffSet = difference * 22.5;
            timeToAnswer /= 1000;

            Debug.Log ("Final Answer: " + currentAnswer);
            Debug.Log ("Correct Answer: " + correctAnswers[answerCounter]);
            Debug.Log("Time to answer: " + timeToAnswer + " seconds");
            Debug.Log("Degree difference: " + degreeOffSet);

            degreeOffsetList.Add(degreeOffSet);
            timeToAnswerList.Add(timeToAnswer);

        }

        //Puts the current test to a .csv document
        if(answers.Count == 5){
            WriteCSV();
        }
        answerCounter++;
	}
 
        private void AnswerListener(int index)
        {
            currentAnswer = index + 1;
            timeToAnswer = stopwatch.ElapsedMilliseconds;
        }

    public void DeactivateAllButtons()
	{
        stopwatch.Reset();

        foreach (GameObject selectable in objs){
            Button button;
	        button = selectable.GetComponentInChildren<Button>();
	        button.interactable = false;
        }
    }
    public void ActivateAllButtons()
	{
        stopwatch.Start();
        currentAnswer = 0;

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

    private void ListenerHandler(Button btn, int index)
        {
            btn.onClick.AddListener(() => { AnswerListener(index); });
        }

    void WriteCSV()
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Sentence Pair, Correct Answer, Given Answer, Time to Answer, Degree Offset");
        tw.Close();

        tw = new StreamWriter(filename, true);
        int row = 0;
        foreach (int answer in answers){
            tw.WriteLine((row+1) + "," + correctAnswers[row] + "," + answers[row] + "," + timeToAnswerList[row] + "," + degreeOffsetList[row]);
            row++;
        }
        tw.Close();
    }
    private void AddStartListener()
    {
        starter = GameObject.FindGameObjectWithTag("Start");
        Button btn = starter.GetComponentInChildren<Button>();
        btn.onClick.AddListener(BeginTest);
    }

}
