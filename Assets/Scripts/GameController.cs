
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject target1, target2, target3;

    public int Targets;

   //public GameInspector shotsControlInspec;
    public GameObject shotControl;
    private ShotControl shotControlInspec;

    public float time = 120f;
    private float timeLeft;
    public Text startText;

    private int numberOfTargets;
    private float winnerTime;

    private bool getTimeOnce = true;

    public GameObject camera1, camera2;
    public GameObject miniCamera;

    private bool gameIsRunning = false;

    private GameObject clientObject;
    private TCPServer client;

    private string input;

    void Awake()
    {
        clientObject = GameObject.FindWithTag("Server");
        shotControlInspec = shotControl.GetComponent<ShotControl>();
        client = clientObject.GetComponent<TCPServer>();
    }



    private int CountTargets()
    {
         GameObject[] thingyToFind = GameObject.FindGameObjectsWithTag("Target");
         return thingyToFind.Length;
    }


    private void Update()
    {
        numberOfTargets = CountTargets();

        CheckCameraChange();
        ToggleMiniMap();

        if (gameIsRunning)
        {
            timeLeft -= Time.deltaTime;

            if (gameIsRunning)
                startText.text = "Time: " + (timeLeft).ToString("F1");

            if (timeLeft < 0)
            {
                timeLeft = 0;
                EndGame();
                startText.text = "Time's Up! " + numberOfTargets + " targets left!";                
            }

            if (numberOfTargets == 0)
            {
                if (getTimeOnce)
                    winnerTime = timeLeft;
                getTimeOnce = false;

                EndGame();
                startText.text = "Targets Destroyed! Time left: " + winnerTime.ToString("F1");                
            }
        }
    }

    private void InstantiateTargets()
    {


        int prefab;
        for (int i = 0; i < Targets; i++)
        {
            prefab = Random.Range(1, 4);
            if (prefab == 1)
                Instantiate(target1);
            if (prefab == 2)
                Instantiate(target2);
            if (prefab == 3)
                Instantiate(target3);
        }
    }

    void EndGame()
    {
        camera1.transform.parent = null;
        player.SetActive(false);

        GameObject []targets;
        targets = GameObject.FindGameObjectsWithTag("Target");

        for (int i = 0; i < targets.Length; i++)
        {
            Destroy(targets[i]);
        }

        shotControlInspec.shotsFired = 0;
        getTimeOnce = true; //able to record finish time again
        gameIsRunning = false;

        startText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        startText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        startText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    void CheckCameraChange()
    {
        if (client.getClientMessage()[0] == 'c')
        {
            camera1.SetActive(!camera1.activeSelf);
            camera2.SetActive(!camera1.activeSelf);
            client.setClientMessage("0");
        }
    }

    void ToggleMiniMap()
    {
        if (client.getClientMessage()[0] == 'm')
        {
            miniCamera.SetActive(!miniCamera.activeSelf);
            client.setClientMessage("0");
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Start Game"))
        {
            if (!gameIsRunning)
            {
                InstantiateTargets();
                player.gameObject.SetActive(true);
                camera1.transform.parent = player.transform;

                startText.rectTransform.anchorMin = new Vector2(1f, 1f);
                startText.rectTransform.anchorMax = new Vector2(1f, 1f);
                startText.rectTransform.pivot = new Vector2(0.5f, 0.5f);

                timeLeft = time;
            }

            gameIsRunning = true;
        }

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
        GUILayout.Label("Shots Fired: " + shotControlInspec.shotsFired);
        GUILayout.Label("  Targets Left: " + numberOfTargets + "/" + Targets);
        GUILayout.EndHorizontal();
        GUILayout.Label("");
        GUILayout.Label(client.getClientMessage());
    }
}
