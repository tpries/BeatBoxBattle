using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeatBoxer : MonoBehaviour
{
    private Beat kick;
    private Beat hiHat;
    private Beat snare;
    private Beat click;
    private Beat clap;
    private Beat shaker;

    [SerializeField] TextAsset txt_Beat;
    [SerializeField] Transform plotPoint;

    private float animWaitTime = 0.3f;
    [SerializeField] private Transform sourcePoint;

    private Animator animator;

    private float startTime;
    private int index = -1;
    private float difficultyFactor = 0.2f;
    private int totalNumberOfBeats;
    private float difficultIncrement = 0.005f;
    private int everyXthBeat = 3;
    private bool spawn = false;
    private bool success = false;
    private float successTime;

    void Awake()
    {
        GlobalManager.FindAudioManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalNumberOfBeats = 10 + (int)((1f - difficultyFactor) / difficultIncrement); 

        animator = GetComponent<Animator>();

        string[] lines = txt_Beat.text.Split('\n');

        int currentBeat = GlobalManager.GetCurrentLevel();

        startTime = Time.time;

        kick = new Beat(Resources.Load("Prefabs/Beats/Kick") as GameObject, sourcePoint, "Kick", lines[currentBeat]);
        hiHat = new Beat(Resources.Load("Prefabs/Beats/HiHat") as GameObject, sourcePoint, "HiHat", lines[currentBeat]);
        snare = new Beat(Resources.Load("Prefabs/Beats/Snare") as GameObject, sourcePoint, "Snare", lines[currentBeat]);
        click = new Beat(Resources.Load("Prefabs/Beats/Click") as GameObject, sourcePoint, "Click", lines[currentBeat]);
        shaker = new Beat(Resources.Load("Prefabs/Beats/Shaker") as GameObject, sourcePoint, "Shaker", lines[currentBeat]);
        clap = new Beat(Resources.Load("Prefabs/Beats/Clap") as GameObject, sourcePoint, "Clap", lines[currentBeat]);

    }

    // Update is called once per frame
    void Update()
    {
        if (success && Time.time - successTime > 5f)
        {
            GlobalManager.LoadScene("StartScreen");
        }

        GlobalManager.PlayBackgroundMusic();

        if (GlobalManager.GetPlaybackSpeed() < Time.time - startTime)
        {
            startTime = Time.time;
            index += 1;
            GlobalManager.SurvivedBeat();
            difficultyFactor += difficultIncrement;

            GlobalManager.PlotDigits(index, plotPoint, true);

            if (index > (4-everyXthBeat)*(totalNumberOfBeats / 4))
            {
                everyXthBeat -= 1;
                if (everyXthBeat == 1) { everyXthBeat = 2; }
            }

            if ((index+4) % everyXthBeat == 0)
            {
                spawn = true;
            }
            else
            {
                spawn = false;
            }

            GlobalManager.SetPlaybackSpeed(GlobalManager.GetPlaybackSpeed() - 0.0007f);
        }
        else
        {
            return;
        }

        if (shaker.IsReady(index))
        {
            // play anim
            animator.SetTrigger("Shaker");
            StartCoroutine(WaitForAnim(Time.time, shaker, spawn));
        }

        if (clap.IsReady(index))
        {
            // play anim 
            animator.SetTrigger("Clap");
            StartCoroutine(WaitForAnim(Time.time, clap, spawn));
        }

        if (kick.IsReady(index))
        {
            // play anim
            animator.SetTrigger("Kick");
            StartCoroutine(WaitForAnim(Time.time, kick, spawn));
        }

        if (hiHat.IsReady(index))
        {
            // play anim 
            animator.SetTrigger("HiHat");
            StartCoroutine(WaitForAnim(Time.time, hiHat, spawn));
        }

        if (snare.IsReady(index))
        {
            // play anim
            animator.SetTrigger("Snare");
            StartCoroutine(WaitForAnim(Time.time, snare, spawn));
        }

        if (click.IsReady(index))
        {
            // play anim 
            animator.SetTrigger("Click");
            StartCoroutine(WaitForAnim(Time.time, click, spawn));
        }
    }

    IEnumerator WaitForAnim(float startTime, Beat beat, bool spawnBeat)
    {
        while (Time.time - startTime > animWaitTime)
        {
            yield return new WaitForSeconds(0.05f);
        }

        // play sound
        beat.ProduceBeat(spawnBeat);
    }
}

public class Beat
{
    private float nextBeat;
    private int[] beatArray;
    private int index = -1;
    private float betweenBeats;

    private GameObject prefab;

    private string name;

    private float lastBeat;
    private Transform sourcePoint;

    private string abbreviation;

    private List<float> waitingTimes = new List<float>();
    private Vector3 offSet = new Vector3(0f, 0f, 0f);

    public Beat(GameObject p, Transform sP, string n, string txt_BeatArray)
    {
        sourcePoint = sP;
        prefab = p;
        name = n;

        if (name.Equals("Click"))
        {
            abbreviation = "D";
            offSet.y = 0.5f;
        }
        if (name.Equals("Kick"))
        {
            abbreviation = "B";
            offSet.y = 0f;
        }
        if (name.Equals("HiHat"))
        {
            abbreviation = "T";
            offSet.y = -0.5f;
        }
        if (name.Equals("Snare"))
        {
            abbreviation = "K";
            offSet.y = 0.5f;
        }
        if (name.Equals("Clap"))
        {
            abbreviation = "C";
        }
        if (name.Equals("Shaker"))
        {
            abbreviation = "F";
        }

        betweenBeats = GlobalManager.GetPlaybackSpeed();

        ParseTxt(txt_BeatArray);
        GetNewWaitingTime();
    }

    public void ProduceBeat(bool spawn)
    {
        //Debug.Log(name + " produced at: " + Time.time.ToString() + " - 0,5: " + (Time.time - 0.5f).ToString());

        if (UnityEngine.Random.Range(0f,1f) < 0.01f)
        {
            GlobalManager.SpawnHeart(sourcePoint.position + offSet, sourcePoint.rotation);
            lastBeat = Time.time;
            //GetNewWaitingTime();
            GlobalManager.PlaySound(name);
            return;
        }

        lastBeat = Time.time;

        if (spawn)
        {
            GameObject.Instantiate(prefab, sourcePoint.position + new Vector3(UnityEngine.Random.Range(-0.6f, 0.6f), UnityEngine.Random.Range(-0.0f, 0.0f), 0f), sourcePoint.rotation);
        }

        GlobalManager.PlaySound(name);

        //GetNewWaitingTime();
    }

    public bool IsReady(int i)
    {
        index = i % beatArray.Length;

        //Debug.Log(beatArray);

        if (beatArray[index] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

        //return Time.time - lastBeat > nextBeat;
    }

    void ParseTxt(string s)
    {
        beatArray = new int[s.Length];
        string[] singleElements = s.Split("");

        for (int i = 0; i < s.Length; i++)
        {
            if (abbreviation.Contains(s[i])) { beatArray[i] = 1; }
            else { beatArray[i] = 0; }
        }
    }

    void GetNewWaitingTime()
    {
        float waitingTime = betweenBeats;

        for (int i = 1; i < beatArray.Length; i++)
        {
            int neverEndingIndex = (i + index) % beatArray.Length;

            if (beatArray[neverEndingIndex] == 0)
            {
                waitingTime += betweenBeats;
            }
            else if (beatArray[neverEndingIndex] == 1)
            {
                index = neverEndingIndex;
                nextBeat = waitingTime;
                //waitingTimes.Add(waitingTime);
                //PrintList();
                return;
            }
            else
            {
                Debug.Log("Error: Wrong value in beatArray." + beatArray[neverEndingIndex].ToString());
            }
        }

        name = "NeverPlay";
        nextBeat = 99f;
    }

    void PrintList()
    {
        string print = name + ":";

        for (int i = 0; i<waitingTimes.Count; i++)
        {
            print += waitingTimes[i].ToString() + " - ";
        }
        Debug.Log(print);
    }
}
