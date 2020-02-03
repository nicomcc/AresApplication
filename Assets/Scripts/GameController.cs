
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject target1, target2, target3;

    public int Targets;

   //public GameInspector shotsControlInspec;
    public GameObject shotControl;
    private ShotControl shotControlInspec;

    public float timeLeft = 120f;
    public Text startText;

    private bool gameEnd = false;

    private int numberOfTargets;
    private float winnerTime;

    private bool getTimeOnce = true;

    public GameObject camera1, camera2;
    public GameObject miniCamera;

    void Start()
    {
        shotControlInspec = shotControl.GetComponent<ShotControl>();

       

        int prefab;
        for(int i = 0; i < Targets; i++)
        {
           prefab = Random.Range(1, 4);
        if(prefab == 1)
           Instantiate(target1);
        if (prefab == 2)
                Instantiate(target2);
        if (prefab == 3)
                Instantiate(target3);
        }

        
    }



    private int CountTargets()
    {
         GameObject[] thingyToFind = GameObject.FindGameObjectsWithTag("Target");
         return thingyToFind.Length;
    }


    private void Update()
    {
        numberOfTargets = CountTargets();

        timeLeft -= Time.deltaTime;
        if (!gameEnd)
            startText.text = (timeLeft).ToString("F1");
        if (timeLeft < 0)
        {
            timeLeft = 0;
            gameEnd = true;
            startText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            startText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            startText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            startText.text = "Time's Up! " + numberOfTargets + " targets left!";
        }

        if (numberOfTargets == 0)
        {
            gameEnd = true;
            
            startText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            startText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            startText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            if (getTimeOnce)
                   winnerTime = timeLeft;
            getTimeOnce = false;
            startText.text = "Targets Destroyed! Time left: " + winnerTime.ToString("F1");
        }

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Change Camera"))
        {
            camera1.SetActive(!camera1.activeSelf);
            camera2.SetActive(!camera1.activeSelf);
        }
        if (GUILayout.Button("MiniCamera"))
        {
            miniCamera.SetActive(!miniCamera.activeSelf);
        }

        GUI.contentColor = Color.black;
        GUILayout.Label("");
        GUILayout.Label("Shots Fired: " + shotControlInspec.shotsFired);
        GUILayout.Label("Targets Left: " + numberOfTargets + "/" + Targets);
        
    }
}
