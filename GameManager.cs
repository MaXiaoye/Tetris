using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    private int CUBE_SIZE = 2;
    private int MAP_HEIGHT_NUM = 11;

    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject[] cubes;

    [SerializeField]
    Sprite[] cubeImages;

    [SerializeField]
    Image nextCubeHint;

    [SerializeField]
    private int mapXZ;

    //[SerializeField]
    private float timeGap;

    [SerializeField]
    private float TIME_GAP_EASY;

    [SerializeField]
    private float TIME_GAP_NORMAL;

    [SerializeField]
    private float TIME_GAP_HARD;

    [SerializeField]
    private float TIME_GAP_INSANE;

    private float difficulty;

    private float timePass;

    private int score;

    private int combo;

    [SerializeField]
    private Text time;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject leftPanel;

    [SerializeField]
    private GameObject rightPanel;

    [SerializeField]
    private ToggleGroup difficultyTG;

    //Store all cubes that already down
    [SerializeField]
    public GameObject landPool;

    private Vector3 localInitialPoint;

    public Cube currentCube;

    private int nextCubeIndex;

    //public Dictionary<Vector3, Transform> cubeTable;
    public Dictionary<Vector3, ChildCube> cubeTable;

    private bool pause;

    private void Awake()
    {
        //cubeTable = new Dictionary<Vector3, Transform>();

        //Initialize cube table
        cubeTable = new Dictionary<Vector3, ChildCube>();

        for (int y = 0; y <= MAP_HEIGHT_NUM - 1; y++)
        {
            for (int x = 0; x <= mapXZ - 1; x++)
            {
                for(int z = 0; z <= mapXZ - 1; z++)
                {
                    cubeTable.Add(new Vector3(x,y,z), null);
                }
            }
        }

        localInitialPoint = new Vector3((mapXZ - 1) / 2, MAP_HEIGHT_NUM - 2, (mapXZ - 1) / 2);
    }

    // Use this for initialization
    void Start () {

        //currentCube = null;
        timePass = 0;
        pause = true;
        nextCubeIndex = Random.Range(0, cubes.Length);
        //nextCubeIndex = 3;
        //nextCubeHint.sprite = cubeImages[nextCubeIndex];
        //newCube();

    }
	
	// Update is called once per frame
	void Update () {

        ShowTime();
        RenderEachRound();
        ControlKeyboard();
    }

    public void MainMenu()
    {
        if (difficultyTG.AnyTogglesOn())
        {
            IEnumerable<Toggle> toggles = difficultyTG.ActiveToggles();

            foreach (Toggle t in toggles)
            {
                if (t.isOn)
                {
                    if (t.name == "Easy")
                    {
                        timeGap = TIME_GAP_EASY;
                        pause = false;
                    }
                    else if (t.name == "Normal")
                    {
                        timeGap = TIME_GAP_NORMAL;
                        pause = false;
                    }
                    else if (t.name == "Hard")
                    {
                        timeGap = TIME_GAP_HARD;
                        pause = false;
                    }
                    else if (t.name == "Insane")
                    {
                        timeGap = TIME_GAP_INSANE;
                        pause = false;
                    }

                    difficulty = timeGap;
                    mainMenu.SetActive(false);
                    leftPanel.SetActive(true);
                    rightPanel.SetActive(true);

                    break;
                }
            }
        }
    }

    public void ShowTime()
    {
        int min = (int)(Time.fixedTime / 60);
        int sec = (int)(Time.fixedTime);
        time.text = min.ToString() + " : " + sec.ToString();
    }

    public int GetCubeSize()
    {
        return CUBE_SIZE;
    }

    public int GetMapY()
    {
        return MAP_HEIGHT_NUM;
    }

    public int GetMapHeight()
    {
        return CUBE_SIZE * MAP_HEIGHT_NUM;
    }

    public int GetMapXZ()
    {
        return mapXZ;
    }

    public int GetMapWidth()
    {
        return mapXZ * CUBE_SIZE;
    }

    private void newCube()
    {   
        //currentCube = Instantiate(cubes[0], new Vector3(1,1,1) ,Quaternion.identity).GetComponent<Cube>();
        currentCube = Instantiate(cubes[nextCubeIndex], new Vector3(1, 1, 1), Quaternion.identity).GetComponent<Cube>();
        currentCube.Initialize();
        nextCubeIndex = Random.Range(0, cubes.Length);
        nextCubeHint.sprite = cubeImages[nextCubeIndex];
    }

    private void RenderEachRound()
    {

        if (pause) return;

        timePass += Time.fixedDeltaTime;

        if (timePass >= timeGap)
        {
            //Debug.Log("2!");
            if (currentCube != null)
            {
                //Debug.Log("2!");
                currentCube.FallCube();
            }

            timePass = 0;

            /*foreach (Vector3 pos in cubeTable.Keys)
            {
                if (cubeTable[pos] != null)
                {
                    //Debug.Log(cc.LocalPos);
                    Debug.Log(cubeTable[pos].name);
                    Debug.Log(pos);
                }


            }*/
        }

        if (currentCube == null)
        {
            if(CheckOver()) return;

            RemoveLayer();
            timeGap = difficulty;
            timePass = 0;
            newCube();
        }
    }

    private bool CheckOver()
    {
        int initialLayer = MAP_HEIGHT_NUM - 2;

        for (int x = 0; x < mapXZ; x++)
        {
            for (int z = 0; z < mapXZ; z++)
            {
                if (cubeTable[new Vector3(x, initialLayer, z)] != null)
                {
                    //Destroy(cubeTable[new Vector3(x, layer, z)]);

                    pause = true;
                    leftPanel.SetActive(false);
                    rightPanel.SetActive(false);
                    gameOverMenu.SetActive(true);
                    gameOverMenu.transform.Find("FinalScore").GetComponent<Text>().text = "  Your score: " + score;
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveLayer()
    {

        HashSet<int> layers = new HashSet<int>();


        for (int i=0; i< MAP_HEIGHT_NUM - 1; i++)
        {
            layers.Add(i);
        }

        foreach (Vector3 pos in cubeTable.Keys)
        {
            if (cubeTable[pos] == null)
            {
                layers.Remove((int)pos.y);

                if(layers.Count == 0)
                {
                    return;
                }
            }
        }

        //Destroy cubes in a full layers
        foreach (int layer in layers)
        {
            for (int x=0; x< mapXZ;x++)
            {
                for (int z=0;z<mapXZ;z++)
                {
                    if (cubeTable[new Vector3(x, layer, z)] != null)
                    {
                        //Destroy(cubeTable[new Vector3(x, layer, z)]);

                        ChildCube cc = cubeTable[new Vector3(x, layer, z)];
                        cubeTable[new Vector3(x, layer, z)] = null;
                        cc.DestroyWhenRemove();                        
                    }                        
                }
            }

            for (int i = 1; i<=layers.Count; i++)
            {
                score += 100 * i;
            }

            scoreText.text = "Score: " + score.ToString();

            //Move down all cubes above the full layers
            for (int y = layer + 1; y < MAP_HEIGHT_NUM; y++)
            {
                for (int x = 0; x < mapXZ; x++)
                {
                    for (int z = 0; z < mapXZ; z++)
                    {
                        if (cubeTable[new Vector3(x, y, z)] != null && cubeTable[new Vector3(x, y, z)].transform.parent.name == "LandPool")
                        {
                            cubeTable[new Vector3(x, y, z)].LocalPos += new Vector3(0, -1, 0);
                        }                        
                    }
                }
            }
        }      
    }

    private void RemoveTest()
    {
        for (int x = 0; x < mapXZ; x++)
        {
            for (int z = 0; z < mapXZ; z++)
            {
                if (cubeTable[new Vector3(x, 0, z)] != null)
                {
                    //DestroyImmediate(cubeTable[new Vector3(x, 0, z)]);
                    //Destroy(cubeTable[new Vector3(x, 0, z)]);
                    ChildCube cc = cubeTable[new Vector3(x, 0, z)];
                    cubeTable[new Vector3(x, 0, z)] = null;
                    cc.DestroyWhenRemove();

                    //Debug.Log(cubeTable[new Vector3(x, 0, z)].name);
                }
            }
        }

        for (int y = 1; y < MAP_HEIGHT_NUM; y++)
        {
            for (int x = 0; x < mapXZ; x++)
            {
                for (int z = 0; z < mapXZ; z++)
                {
                    if (cubeTable[new Vector3(x, y, z)] != null && cubeTable[new Vector3(x, y, z)].transform.parent.name == "LandPool")
                    {
                        cubeTable[new Vector3(x, y, z)].LocalPos += new Vector3(0, -1, 0);
                    }
                }
            }
        }
    }

    private void ControlKeyboard()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {
            //Debug.Log("W");
            //currentCube.MoveCube(new Vector3(-1,0,0));
            currentCube.MoveControl(-1, cam.GetComponent<DrawFlagLine>().GetViewLocation());          

        } else if (Input.GetKeyUp(KeyCode.S))
        {
            currentCube.MoveControl(3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
            //currentCube.MoveCube(new Vector3(0, 0, 1));
        } else if (Input.GetKeyUp(KeyCode.Q))
        {

            //currentCube.MoveCube(new Vector3(0, 0, -1));
            currentCube.MoveControl(-3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
        } else if (Input.GetKeyUp(KeyCode.A))
        {
            //currentCube.MoveCube(new Vector3(1, 0, 0));
            currentCube.MoveControl(1, cam.GetComponent<DrawFlagLine>().GetViewLocation());
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            timeGap = 0.1f;
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            currentCube.RotateControl(1, cam.GetComponent<DrawFlagLine>().GetViewLocation());
        } else if (Input.GetKeyUp(KeyCode.F))
        {
            currentCube.RotateControl(3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
        } else if (Input.GetKeyUp(KeyCode.R))
        {
            currentCube.RotateControl(2, cam.GetComponent<DrawFlagLine>().GetViewLocation());
        } else if (Input.GetKeyUp(KeyCode.T))
        {
            newGame();
        }
    }

    public int GetViewLocation()
    {
        return cam.GetComponent<DrawFlagLine>().GetViewLocation();
    }
    
    public void ButtonDetect(Button btn)
    {
        if (pause) return;

        switch (btn.name)
        {
            case "ZNega":
                ControlMoveZNega();
                break;

            case "XNega":
                ControlMoveXNega();
                break;

            case "XPosi":
                ControlMoveXPosi();
                break;

            case "ZPosi":
                ControlMoveZPosi();
                break;

            case "FallDown":
                ControlDownCube();
                break;

            case "Y":
                ControlRotateY();
                break;

            case "X":
                ControlRotateX();
                break;

            case "Z":
                ControlRotateZ();
                break;

            case "Up":
                cam.GetComponent<DrawFlagLine>().ControlViewRotateUp();
                break;

            case "Clockwise":
                cam.GetComponent<DrawFlagLine>().ControlViewRotateClock();
                break;

            case "Counterclockwise":
                cam.GetComponent<DrawFlagLine>().ControlViewRotateCounterClock();
                break;

            case "Down":
                cam.GetComponent<DrawFlagLine>().ControlViewRotateDown();
                break;
        }
    }

    public void ControlMoveXPosi()
    {
        currentCube.MoveControl(1, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlMoveXNega()
    {
        currentCube.MoveControl(-1, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlMoveZPosi()
    {
        currentCube.MoveControl(3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlMoveZNega()
    {
        currentCube.MoveControl(-3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlDownCube()
    {
        timeGap = 0.1f;
    }

    public void ControlRotateX()
    {
        currentCube.RotateControl(1, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlRotateY()
    {
        currentCube.RotateControl(2, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void ControlRotateZ()
    {
        currentCube.RotateControl(3, cam.GetComponent<DrawFlagLine>().GetViewLocation());
    }

    public void newGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause(Button btn)
    {
        if(pause)
        {
            btn.GetComponentInChildren<Text>().text = "Pause";
            pause = false;

        }else
        {
            btn.GetComponentInChildren<Text>().text = "Resume";
            pause = true;
        }
    }
}
