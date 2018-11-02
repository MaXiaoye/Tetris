using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFlagLine : MonoBehaviour {

    private int MAX_ROTATE_UP_X = 34;
    private int DEF_ROTATE_UP_X = 22;
    private int MIN_ROTATE_UP_X = 14;

    [SerializeField]
    private Material flagLineMat;

    [SerializeField]
    private GameObject[] axisSet;

    private int mapXZ;

    private int cubeSize;

    private int mapHeight;

    private int mapWidth;

    private int ViewLocation = 1;

    private float totalRotation = 0;

    private float rotationDegreesAmount = 0;

    private bool RotateLeft = false;

    private bool RotateRight = false;

    // Use this for initialization
    void Start () {
        cubeSize = GameManager.Instance.GetCubeSize();
        mapHeight = GameManager.Instance.GetMapHeight();
        mapXZ = GameManager.Instance.GetMapXZ();
        mapWidth = GameManager.Instance.GetMapWidth();
    }
	
	// Update is called once per frame
	void Update () {

        DetectInput();
        SmoothRotate();
    }

    private void OnDrawGizmos()
    {
        if (cubeSize != 0)
        {
            DrawFlagGrid();
        }       
    }

    private void OnPostRender()
    {
        DrawFlagGrid();
    }

    private void DrawFlagGrid()
    {
        switch (ViewLocation)
        {
            case 1:
                DrawFlagGridOne();
                DrawFlagGridTwo();
                break;

            case 2:
                DrawFlagGridTwo();
                DrawFlagGridThree();
                break;

            case 3:
                DrawFlagGridThree();
                DrawFlagGridFour();
                break;

            case 4:
                DrawFlagGridOne();
                DrawFlagGridFour();
                break;
        }
    }

    private void DrawFlagGridOne()
    {
        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        for (int i = 0; i <= mapXZ; i++)
        {
            startPoint = new Vector3(i * cubeSize, 0, 0);
            endPoint = new Vector3(i * cubeSize, mapHeight, 0);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }

        for (int i = 1; i <= mapHeight / cubeSize; i++)
        {
            startPoint = new Vector3(0, i * cubeSize, 0);
            endPoint = new Vector3(mapWidth, i * cubeSize, 0);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }
    }

    private void DrawFlagGridTwo()
    {
        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        for (int i = 0; i <= mapXZ; i++)
        {
            startPoint = new Vector3(0, 0, i * cubeSize);
            endPoint = new Vector3(0, mapHeight, i * cubeSize);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }

        for (int i = 1; i <= mapHeight / cubeSize; i++)
        {
            startPoint = new Vector3(0, i * cubeSize, 0);
            endPoint = new Vector3(0, i * cubeSize, mapWidth);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }
    }

    private void DrawFlagGridThree()
    {
        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        for (int i = 0; i <= mapXZ; i++)
        {
            startPoint = new Vector3(i * cubeSize, 0, mapWidth);
            endPoint = new Vector3(i * cubeSize, mapHeight, mapWidth);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }

        for (int i = 1; i <= mapHeight / cubeSize; i++)
        {
            startPoint = new Vector3(0, i * cubeSize, mapWidth);
            endPoint = new Vector3(mapWidth, i * cubeSize, mapWidth);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }
    }

    private void DrawFlagGridFour()
    {
        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        for (int i = 0; i <= mapXZ; i++)
        {
            startPoint = new Vector3(mapWidth, 0, i * cubeSize);
            endPoint = new Vector3(mapWidth, mapHeight, i * cubeSize);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }

        for (int i = 1; i <= mapHeight / cubeSize; i++)
        {
            startPoint = new Vector3(mapWidth, i * cubeSize, 0);
            endPoint = new Vector3(mapWidth, i * cubeSize, mapWidth);

            GL.Begin(GL.LINES);
            flagLineMat.SetPass(0);
            GL.Color(Color.gray);
            GL.Vertex(startPoint);
            GL.Color(Color.gray);
            GL.Vertex(endPoint);
            GL.End();
        }
    }

    private void SmoothRotate()
    {
        if (Mathf.Abs(totalRotation) < Mathf.Abs(rotationDegreesAmount) && RotateLeft)
        {
            //transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, Time.fixedDeltaTime * 135f);
            //totalRotation += Time.fixedDeltaTime * 135f;

            transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, 3);
            totalRotation += 3;

            Mathf.Clamp(0, 90, totalRotation);

            if (totalRotation >= 90)
            {
                totalRotation = 0;
                rotationDegreesAmount = 0;
                RotateLeft = false;

                ViewLocation += 1;

                //Update all flags
                GameManager.Instance.currentCube.UpdateAllFlags();

                if (ViewLocation > 4)
                {
                    ViewLocation = 1;
                }

                //Show corresponding axis and flaggrid
                ShowAxis(ViewLocation);
            }
        } else if (Mathf.Abs(totalRotation) < Mathf.Abs(rotationDegreesAmount) && RotateRight)
        {
            transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, -3);
            totalRotation += 3;

            Mathf.Clamp(0, 90, totalRotation);

            if (totalRotation >= 90)
            {
                totalRotation = 0;
                rotationDegreesAmount = 0;
                RotateRight = false;

                ViewLocation -= 1;

                //Update all flags
                GameManager.Instance.currentCube.UpdateAllFlags();

                if (ViewLocation < 1)
                {
                    ViewLocation = 4;
                }

                //Show corresponding axis and flaggrid
                ShowAxis(ViewLocation);
            }
        }
            
    }

    private void DetectInput()
    {
        //Rotate camera
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !RotateLeft && !RotateRight)
        {
            //float oriAngle = transform.rotation.eulerAngles.y;
            //rotate around center of the panel
            //transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, Mathf.LerpAngle(90,90,Time.deltaTime));
            //transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, Mathf.Clamp(Time.deltaTime,0,90));
            //transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, 90);
            RotateLeft = true;
            rotationDegreesAmount = 90;
            

        } else if (Input.GetKeyDown(KeyCode.RightArrow) && !RotateLeft && !RotateRight)
        {
            //transform.RotateAround(new Vector3(7, 0, 7), Vector3.up, -90);
            RotateRight = true;
            rotationDegreesAmount = -90;
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (transform.rotation.eulerAngles.x < 30)
            {
                //transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1,0,Mathf.Pow(-1,ViewLocation)), -2 * Mathf.Pow(-1, ViewLocation+1));

                if (ViewLocation == 1)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, 1), 2);
                }
                else if (ViewLocation == 2)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, 1), 2);
                }
                else if (ViewLocation == 3)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, -1), 2);
                }
                else if (ViewLocation == 4)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, -1), 2);
                }
                //transform.Rotate(new Vector3(2, 0, 0));
                //Debug.Log(transform.rotation.eulerAngles.x);
            }

        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (transform.rotation.eulerAngles.x > 14)
            {
                if (ViewLocation == 1)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, 1), -2);
                }
                else if (ViewLocation == 2)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, 1), -2);
                }
                else if (ViewLocation == 3)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, -1), -2);
                }
                else if (ViewLocation == 4)
                {
                    transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, -1), -2);
                }
            }
        }
    }

    private void ShowAxis(int view)
    {
        foreach (GameObject axis in axisSet)
        {
            axis.SetActive(false);
        }
        axisSet[view - 1].SetActive(true);
    }

    public int GetViewLocation()
    {
        return ViewLocation;
    }

    public void ControlViewRotateUp()
    {
        if (transform.rotation.eulerAngles.x < MAX_ROTATE_UP_X)
        {
            if (ViewLocation == 1)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, 1), 2);
            }
            else if (ViewLocation == 2)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, 1), 2);
            }
            else if (ViewLocation == 3)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, -1), 2);
            }
            else if (ViewLocation == 4)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, -1), 2);
            }
        }
    }

    public void ControlViewRotateDown()
    {
        if (transform.rotation.eulerAngles.x > MIN_ROTATE_UP_X)
        {
            if (ViewLocation == 1)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, 1), -2);
            }
            else if (ViewLocation == 2)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, 1), -2);
            }
            else if (ViewLocation == 3)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(1, 0, -1), -2);
            }
            else if (ViewLocation == 4)
            {
                transform.RotateAround(new Vector3(7, 0, 7), new Vector3(-1, 0, -1), -2);
            }
        }
    }

    public void ControlViewRotateClock()
    {
        if (!RotateLeft && !RotateRight)
        {
            RotateLeft = true;
            rotationDegreesAmount = 90;
        }
    }

    public void ControlViewRotateCounterClock()
    {
        if (!RotateLeft && !RotateRight)
        {
            RotateRight = true;
            rotationDegreesAmount = -90;
        }
    }
}
