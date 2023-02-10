using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class GlobalManager
{

    public static void SpawnHeart(Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(Resources.Load("Prefabs/Beats/Heart"), position, rotation);
        PlaySound("Triangle"); // for now
    }

    #region Audio

    private static float playbackSpeed = 0.4f;

    private static float basePlaybackSpeed = 0.4f;

    public static float GetPlaybackSpeed() { return playbackSpeed; }

    public static void SetPlaybackSpeed(float speed)
    {
        if (playbackSpeed < 0.07f)
        {
            playbackSpeed = 0.07f;
        }

        playbackSpeed = speed;
    }

    private static AudioManager audioManager;

    public static void FindAudioManager()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public static void PlayStartSceneBackgroundMusic()
    {
        audioManager.Play("StartScene");
    }

    public static void PlayBackgroundMusic()
    {
        audioManager.Play("BackgroundMusic");
    }

    public static void PlaySound(string name)
    {
        audioManager.PlayOverlapping(name);
    }

    #endregion

    #region GlobalTimer

    private static float startTime;
     
    public static void StartTimer() { startTime = Time.time; }

    //public static int GetBeatIndex() { return (int)((Time.time - startTime)*1000) % int(playbackSpeed*1000); }

    #endregion

    #region LevelSystem

    private static int currentLevel;

    public static bool playing = false;

    private static int beatsSurvived = 0;

    public static void SurvivedBeat() { beatsSurvived += 1; }

    public static int GetCurrentLevel() { return currentLevel; }

    public static int GetCurrentScore() { return beatsSurvived; }

    public static void StartLevel(int level) 
    {
        foreach (var plottedDigit in deleteOnLoad)
        {
            GameObject.Destroy(plottedDigit);
        }

        playbackSpeed = basePlaybackSpeed;
        startTime = Time.time;
        beatsSurvived = 0;
        currentLevel = level;
        playing = true;
    }

    private static int[] bestTimes = new int[3];
    
    public static void ReadHighScores()
    {

        string path = Application.persistentDataPath + "/HighScores.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string[] highScores = reader.ReadToEnd().Split('\n');
        reader.Close();

        Debug.Log(highScores);

        bestTimes[0] = int.Parse(highScores[0]);
        bestTimes[1] = int.Parse(highScores[1]);
        bestTimes[2] = int.Parse(highScores[2]);
    }

    private static void WriteHighScores()
    {
        string path = Application.persistentDataPath + "/HighScores.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(bestTimes[0].ToString());
        writer.WriteLine(bestTimes[1].ToString());
        writer.WriteLine(bestTimes[2].ToString());
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        reader.Close();
    }
    

    public static void CompletedLevel()
    {
        Debug.Log("Completed Level GM");

        if (bestTimes[currentLevel] < beatsSurvived)
        {
            bestTimes[currentLevel] = beatsSurvived;
            WriteHighScores();
        }
    }

    public static int GetBestScore(int index)
    {
        return bestTimes[index];
    }

    #endregion

    public static void LoadScene(string scene)
    {
        if (scene.Equals("StartScreen"))
        {
            playing = false;
        }

        SceneManager.LoadScene(scene);
    }

    private static List<GameObject> plottedDigits = new List<GameObject>();
    private static List<GameObject> deleteOnLoad = new List<GameObject>();

    public static void PlotDigits(int score, Transform transform, bool deleteQuick)
    {
        if (deleteQuick)
        {
            foreach (var pastDigit in plottedDigits)
            {
                GameObject.Destroy(pastDigit);
            }
        }

        string scoreAsString = score.ToString();

        float xPos = - scoreAsString.Length / 4;

        for (int i = 0; i < scoreAsString.Length; i++)
        {
            GameObject digit;

            switch (scoreAsString[i])
            {
                case '0':
                    digit = Resources.Load("Prefabs/Digits/0") as GameObject;
                    break;
                case '1':
                    digit = Resources.Load("Prefabs/Digits/1") as GameObject;
                    break;
                case '2':
                    digit = Resources.Load("Prefabs/Digits/2") as GameObject;
                    break;
                case '3':
                    digit = Resources.Load("Prefabs/Digits/3") as GameObject;
                    break;
                case '4':
                    digit = Resources.Load("Prefabs/Digits/4") as GameObject;
                    break;
                case '5':
                    digit = Resources.Load("Prefabs/Digits/5") as GameObject;
                    break;
                case '6':
                    digit = Resources.Load("Prefabs/Digits/6") as GameObject;
                    break;
                case '7':
                    digit = Resources.Load("Prefabs/Digits/7") as GameObject;
                    break;
                case '8':
                    digit = Resources.Load("Prefabs/Digits/8") as GameObject;
                    break;
                case '9':
                    digit = Resources.Load("Prefabs/Digits/9") as GameObject;
                    break;
                default:
                    digit = null;
                    Debug.Log("Error Bad Argument in PlotDigits");
                    break;
            }

            GameObject spawned = GameObject.Instantiate(digit, transform.position + new Vector3(xPos / 2f, 0f, 0f), transform.rotation);

            if (deleteQuick) { plottedDigits.Add(spawned); }
            else { deleteOnLoad.Add(spawned); } 
            
            xPos += 1;
        }
    }
}
