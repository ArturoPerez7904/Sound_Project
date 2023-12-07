using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables;

public class AnswerController : MonoBehaviour
{

    string filename="";
    public GameObject starter;
    public GameObject testSelector;
    public List<Button> buttons = new List<Button>{};
    public List<Button> testButtons = new List<Button>{};
    private int currentAnswer;
    List<int> answers = new List<int>{};
    List<double> degreeOffsetList = new List<double>{};
    List<float> timeToAnswerList = new List<float>{};
    private float timeToAnswer;
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    public GameObject[] objs;
    [SerializeField] List<int> correctAnswers = new List<int>{};
    [SerializeField] List<float> timeStops = new List<float>{};
    private int answerCounter = 0;
    public TestAnswerList ListOfTestAnswers = new TestAnswerList();
    [System.Serializable]
    public class TestAnswers
    {
        public List<int> theseAnswers;
    }
 
    [System.Serializable]
    public class TestAnswerList
    {
        public List<TestAnswers> answersList;
    }

    public StopTimesList ListOfStopTimes = new StopTimesList();
    [System.Serializable]
    public class StopTimes
    {
        public List<int> theseStopTimes;
    }
 
    [System.Serializable]
    public class StopTimesList
    {
        public List<StopTimes> stopTimesList;
    }
    TextWriter twTrue;
    bool twTrueIsOpen = false;

    private void Awake()
        {
            AddListeners();
        }

    void Start()
    {
        CreateFile();
        objs = GameObject.FindGameObjectsWithTag("Selectable");
        DeactivateAllButtons();
        AddStartListener();
        GameObject.FindGameObjectWithTag("Start").transform.localScale = new Vector3(0, 0, 0);
        
    }

    void Quit()
    {
        if(twTrueIsOpen){
            twTrue.Close();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    void RunOnStart()
    {
        Application.quitting += Quit;
    }

    private void CreateFile()
    {
        int i = 1;
        filename = Application.persistentDataPath + "/" + i + "test.csv";
        while (File.Exists(filename)){
            filename = Application.persistentDataPath + "/" + i + "test.csv";
            i++;
        }
        TextWriter twFalse = new StreamWriter(filename, false);
        twFalse.WriteLine("Sentence Pair, Correct Answer, Given Answer, Time to Answer, Degree Offset");
        twFalse.Close();
        
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
        if (currentAnswer == 0){
            degreeOffsetList.Add(0);
            timeToAnswerList.Add(0);
        }
        else{
            double difference = Mathf.Abs(currentAnswer - correctAnswers[answerCounter]);
            double degreeOffSet = difference * 22.5;
            timeToAnswer /= 1000;

            degreeOffsetList.Add(degreeOffSet);
            timeToAnswerList.Add(timeToAnswer);

        }
        if(answerCounter == 0){
            twTrue = new StreamWriter(filename, true);
            twTrueIsOpen = true;
        }

        int row = answerCounter;

        if((row%5 == 0) & (row != 0)){
                twTrue.WriteLine("Passage Break");
            }
            
        twTrue.WriteLine((row+1) + "," + correctAnswers[row] + "," + answers[row] + "," + timeToAnswerList[row] + "," + degreeOffsetList[row]);
        twTrue.Flush();
        answerCounter++;
        if(answerCounter >= correctAnswers.Count){
            twTrue.Close();
            twTrueIsOpen = false;
        }
	}
    void WriteCSV()
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Sentence Pair, Correct Answer, Given Answer, Time to Answer, Degree Offset");
        tw.Close();

        tw = new StreamWriter(filename, true);
        int row = 0;
        foreach (int answer in answers){
            
            if((row%5 == 0) & (row != 0)){
                tw.WriteLine("Passage Break");
            }
            
            tw.WriteLine((row+1) + "," + correctAnswers[row] + "," + answers[row] + "," + timeToAnswerList[row] + "," + degreeOffsetList[row]);

            row++;
        }
        tw.Close();
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

        private void AnswerListener(int index)
        {
            currentAnswer = index + 1;
            timeToAnswer = stopwatch.ElapsedMilliseconds;
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
        foreach (Button btn in testButtons)
        {
            TestListenerHandler(btn, i);

            i++;
        }
    }

    private void ListenerHandler(Button btn, int index)
        {
            btn.onClick.AddListener(() => { AnswerListener(index); });
        }

    private void TestListenerHandler(Button btn, int index)
        {
            btn.onClick.AddListener(() => { TestSelectionListener(index); });
        }


    private void AddStartListener()
    {
        starter = GameObject.FindGameObjectWithTag("Start");
        testSelector = GameObject.FindGameObjectWithTag("Test Selector");
        Button btn = starter.GetComponentInChildren<Button>();
        btn.onClick.AddListener(BeginTest);
    }
    private void LoadTest(int selectedTest)
    {
        correctAnswers.Clear();
        timeStops.Clear();
        foreach (int answer in ListOfTestAnswers.answersList[selectedTest].theseAnswers){
            correctAnswers.Add(answer);
        }

        foreach (int stopTime in ListOfStopTimes.stopTimesList[selectedTest].theseStopTimes){
            timeStops.Add(stopTime);
        }
        Destroy(testSelector);
        GameObject.FindGameObjectWithTag("Start").transform.localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);

    }

    private void TestSelectionListener(int index)
        {
            int selectedTest = index;
            LoadTest(selectedTest - 1);
        }

}
