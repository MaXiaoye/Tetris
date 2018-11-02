using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    [SerializeField]
    private int type;

    //private Transform[] childCubes;
    private ChildCube[] childCubes;

    private int count;

    private int mapXZ;

    private int cubeSize;

    private int mapHeight;

    private int mapWidth;

    public int Type
    {
        get
        {
            return type;
        }
    }

    public int Count
    {
        get
        {
            return count;
        }

        set
        {
            count = value;
        }
    }

    // Use this for initialization
    void Start () {

        
        //childCubes = new game
        
    }

    private void Awake()
    {
        //childCubes = GetComponentsInChildren<Transform>().CopyTo();
        
        //GetComponentsInChildren<Transform>().CopyTo(childCubes, 2);
        //Debug.Log(GetComponentsInChildren<Transform>().Length);
        //Debug.Log(childCubes[0].name);
        //GetChilds();
        count = transform.childCount;
        //Debug.Log(count);
        //childCubes = new Transform[count];
        childCubes = GetComponentsInChildren<ChildCube>();
        //GetChilds();
        cubeSize = GameManager.Instance.GetCubeSize();
        mapHeight = GameManager.Instance.GetMapHeight();
        mapXZ = GameManager.Instance.GetMapXZ();
        mapWidth = GameManager.Instance.GetMapWidth();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void GetChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //childCubes[i] = transform.GetChild(i);
        }
        //childCubes = GetComponentsInChildren<Transform>();
        /*childCubes[0] = transform.GetChild(0);
        childCubes[1] = transform.GetChild(1);
        childCubes[2] = transform.GetChild(2);
        childCubes[3] = transform.GetChild(3);*/
    }

    public void Initialize()
    {
        MoveCubeTo(new Vector3((mapXZ - 1) / 2, mapHeight / cubeSize - 2, (mapXZ - 1) / 2));

        foreach (ChildCube cc in childCubes)
        {
            cc.InitialFlags();
        }
    }

    private void MoveCubeTo(Vector3 tar)
    {
        /*Vector3 diff = tar - childCubes[0].localPosition;

        foreach (Transform sub in childCubes)
        {
            sub.localPosition += diff;
        }*/
        Vector3 diff = tar - childCubes[0].LocalPos;

        foreach (ChildCube cc in childCubes)
        {
            cc.LocalPos += diff;
        }
    }

    private void MoveCube(Vector3 dis)
    {

        if (!CheckMove(dis)) return;

        /*foreach (Transform sub in childCubes)
        {
            GameManager.Instance.cubeTable[sub.localPosition] = null;
        }

        switch (type)
        {
            case 1:

                foreach (Transform sub in childCubes)
                {
                    sub.localPosition += dis;
                }

                break;
        }

        foreach (Transform sub in childCubes)
        {
            GameManager.Instance.cubeTable[sub.localPosition] = sub;
        }*/

        foreach (ChildCube cc in childCubes)
        {
            cc.LocalPos += dis;
        }
    }

    private bool CheckMove(Vector3 dis)
    {
        /*foreach (Transform sub in childCubes)
        {
            if (!GameManager.Instance.cubeTable.ContainsKey(sub.localPosition + dis))
            {
                return false;
            } else if(GameManager.Instance.cubeTable[sub.localPosition + dis] != null && GameManager.Instance.cubeTable[sub.localPosition + dis].parent.name == "LandPool")
            {
                return false;
            }
        }*/

        foreach (ChildCube cc in childCubes)
        {
            if (!GameManager.Instance.cubeTable.ContainsKey(cc.LocalPos + dis))
            {
                return false;
            }
            else if (GameManager.Instance.cubeTable[cc.LocalPos + dis] != null && GameManager.Instance.cubeTable[cc.LocalPos + dis].transform.parent.name == "LandPool")
            {
                return false;
            }
        }

        return true;
    }

    private void MoveXPositive()
    {
        MoveCube(new Vector3(1, 0, 0));
    }

    private void MoveXNegative()
    {
        MoveCube(new Vector3(-1, 0, 0));
    }

    private void MoveZPositive()
    {
        MoveCube(new Vector3(0, 0, 1));
    }

    private void MoveZNegative()
    {
        MoveCube(new Vector3(0, 0, -1));
    }

    public void MoveControl(int dir, int view)
    {
        switch (view)
        {
            case 1:

                if (dir == 1)
                {
                    MoveXPositive();
                } else if (dir == -1)
                {
                    MoveXNegative();
                } else if (dir == 3)
                {
                    MoveZPositive();
                } else if (dir == -3)
                {
                    MoveZNegative();
                }

                break;

            case 2:

                if (dir == 1)
                {
                    MoveZNegative();
                }
                else if (dir == -1)
                {
                    MoveZPositive();                  
                }
                else if (dir == 3)
                {
                    MoveXPositive();                    
                }
                else if (dir == -3)
                {
                    MoveXNegative();
                }

                break;

            case 3:

                if (dir == 1)
                {
                    MoveXNegative();
                }
                else if (dir == -1)
                {
                    MoveXPositive();
                }
                else if (dir == 3)
                {
                    MoveZNegative();
                }
                else if (dir == -3)
                {
                    MoveZPositive();
                }

                break;

            case 4:

                if (dir == 1)
                {
                    MoveZPositive();
                }
                else if (dir == -1)
                {
                    MoveZNegative();
                }
                else if (dir == 3)
                {
                    MoveXNegative();
                }
                else if (dir == -3)
                {
                    MoveXPositive();
                }

                break;
        }
    }

    private Vector3 GetRotatePivot()
    {
        Vector3 pivot = new Vector3();
        /*switch (type)
        {
            case 1:

                pivot = childCubes[0].localPosition;

                break;
        }*/

        switch (type)
        {
            case 0:

                pivot = childCubes[0].LocalPos;

                break;

            case 1:

                pivot = childCubes[0].LocalPos;

                break;

            case 2:

                pivot = childCubes[0].LocalPos;

                break;

            case 3:

                pivot = childCubes[0].LocalPos;

                break;

            case 4:

                pivot = childCubes[0].LocalPos;

                break;
        }

        return pivot;
    }

    private Vector3 GetGeoPivot()
    {
        float x = (childCubes[0].LocalPos.x + childCubes[1].LocalPos.x + childCubes[2].LocalPos.x + childCubes[3].LocalPos.x) / 4;
        float y = (childCubes[0].LocalPos.y + childCubes[1].LocalPos.y + childCubes[2].LocalPos.y + childCubes[3].LocalPos.y) / 4;
        float z = (childCubes[0].LocalPos.z + childCubes[1].LocalPos.z + childCubes[2].LocalPos.z + childCubes[3].LocalPos.z) / 4;

        return new Vector3(x,y,z);
    }

    private bool CheckRotate(Vector3[] afterRotatePos)
    {
        foreach (Vector3 pos in afterRotatePos)
        {
            if (!GameManager.Instance.cubeTable.ContainsKey(pos))
            {
                return false;
            } else if (GameManager.Instance.cubeTable[pos] != null && GameManager.Instance.cubeTable[pos].transform.parent.name == "LandPool") 
            {
                return false;
            }
        }
        return true;
    }

    private void RotateCubeX()
    {
        Vector3[] afterRotate = new Vector3[count];
        /*for (int i = 0;i < count;i++)
        {
            afterRotate[i] = new Vector3(childCubes[i].localPosition.x, GetRotatePivot().y + childCubes[i].localPosition.z - GetRotatePivot().z, GetRotatePivot().z - childCubes[i].localPosition.y + GetRotatePivot().y);
        }

        if (CheckRotate(afterRotate))
        {

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = null;
            }

            for (int i = 0; i < count; i++)
            {
                childCubes[i].localPosition = afterRotate[i];        
            }

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = sub;
            }
        }*/

        for (int i = 0; i < count; i++)
        {
            afterRotate[i] = new Vector3(childCubes[i].LocalPos.x, GetRotatePivot().y + childCubes[i].LocalPos.z - GetRotatePivot().z, GetRotatePivot().z - childCubes[i].LocalPos.y + GetRotatePivot().y);
        }

        if (CheckRotate(afterRotate))
        {

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.LocalPos] = null;
            }*/

            for (int i = 0; i < count; i++)
            {
                childCubes[i].LocalPos = afterRotate[i];
            }

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.LocalPos] = cc;
            }*/
        }

    }

    private void RotateCubeY()
    {
        Vector3[] afterRotate = new Vector3[count];
        /*for (int i = 0; i < count; i++)
        {
            afterRotate[i] = new Vector3(GetRotatePivot().x + GetRotatePivot().z - childCubes[i].localPosition.z, childCubes[i].localPosition.y, GetRotatePivot().z + childCubes[i].localPosition.x - GetRotatePivot().x);
            //Mathf.Ceil();
        }

        if (CheckRotate(afterRotate))
        {

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = null;
            }

            for (int i = 0; i < count; i++)
            {
                childCubes[i].localPosition = afterRotate[i];
                //Mathf.Ceil();         
            }

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = sub;
            }
        }*/

        for (int i = 0; i < count; i++)
        {
            //afterRotate[i] = new Vector3(GetRotatePivot().x + GetRotatePivot().z - childCubes[i].LocalPos.z, childCubes[i].LocalPos.y, GetRotatePivot().z + childCubes[i].LocalPos.x - GetRotatePivot().x);
            afterRotate[i] = new Vector3((int) (GetRotatePivot().x + GetRotatePivot().z - childCubes[i].LocalPos.z), childCubes[i].LocalPos.y, (int)(GetRotatePivot().z + childCubes[i].LocalPos.x - GetRotatePivot().x));
            //Mathf.Ceil();
        }

        if (CheckRotate(afterRotate))
        {

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.LocalPos] = null;
            }*/

            for (int i = 0; i < count; i++)
            {
                childCubes[i].LocalPos = afterRotate[i];
                //Mathf.Ceil();         
            }

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.LocalPos] = cc;
            }*/
        }

    }

    private void RotateCubeZ()
    {
        /*Vector3[] afterRotate = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            afterRotate[i] = new Vector3(GetRotatePivot().x - childCubes[i].localPosition.y + GetRotatePivot().y, GetRotatePivot().y + childCubes[i].localPosition.x - GetRotatePivot().x, childCubes[i].localPosition.z);
            //Mathf.Ceil();
        }

        if (CheckRotate(afterRotate))
        {

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = null;
            }

            for (int i = 0; i < count; i++)
            {
                childCubes[i].localPosition = afterRotate[i];
                //Mathf.Ceil();         
            }

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = sub;
            }
        }*/

        Vector3[] afterRotate = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            afterRotate[i] = new Vector3(GetRotatePivot().x - childCubes[i].LocalPos.y + GetRotatePivot().y, GetRotatePivot().y + childCubes[i].LocalPos.x - GetRotatePivot().x, childCubes[i].LocalPos.z);
            //Mathf.Ceil();
        }

        if (CheckRotate(afterRotate))
        {

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.transform.localPosition] = null;
            }*/

            for (int i = 0; i < count; i++)
            {
                childCubes[i].LocalPos = afterRotate[i];
                //Mathf.Ceil();         
            }

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.transform.localPosition] = cc;
            }*/
        }

    }

    public void RotateControl(int dir, int view)
    {
        if (dir == 2)
        {
            RotateCubeY();
        } else if (dir == 1)
        {
            if (view == 1 || view == 3)
            {
                RotateCubeX();
            } else
            {
                RotateCubeZ();
            }
        } else if (dir == 3)
        {
            if (view == 1 || view == 3)
            {
                RotateCubeZ();
            }
            else
            {
                RotateCubeX();
            }
        }

        
    }

    public void FallCube()
    {
        if (CheckDown())
        {
            //Update cube table before and after move
            /*foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = null;
            }

            MoveCube(new Vector3(0, -1, 0));

            foreach (Transform sub in childCubes)
            {
                GameManager.Instance.cubeTable[sub.localPosition] = sub;
            }*/

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.transform.localPosition] = null;
            }*/

            MoveCube(new Vector3(0, -1, 0));

            /*foreach (ChildCube cc in childCubes)
            {
                GameManager.Instance.cubeTable[cc.transform.localPosition] = cc;
            }*/

            //UpdateCubeTable();
        } else
        {
            AfterDown();
        }
    }

    private bool CheckDown()
    {
        /*foreach (Transform sub in childCubes)
        {
            //if it reaches the land or some cubes existing under it
            if (sub.localPosition.y == 0)
            {
                return false;

            } else if (GameManager.Instance.cubeTable[sub.localPosition + new Vector3(0, -1, 0)] != null && GameManager.Instance.cubeTable[sub.localPosition + new Vector3(0, -1, 0)].parent.name == "LandPool")
            {
                return false;
            }                
        }*/

        foreach (ChildCube cc in childCubes)
        {
            //if it reaches the land or some cubes existing under it
            if (cc.LocalPos.y == 0)
            {
                return false;

            }
            else if (GameManager.Instance.cubeTable[cc.LocalPos + new Vector3(0, -1, 0)] != null && GameManager.Instance.cubeTable[cc.LocalPos + new Vector3(0, -1, 0)].transform.parent.name == "LandPool")
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateCubeTable()
    {
        /*foreach (Transform sub in childCubes)
        {
            GameManager.Instance.cubeTable[sub.localPosition] = sub;
        }*/
    }

    public void UpdateAllFlags()
    {
        foreach (ChildCube cc in childCubes)
        {
            cc.UpdateFlags();
        }
    }

    private void AfterDown()
    {
        /*foreach (Transform sub in childCubes)
        {
            sub.SetParent(GameManager.Instance.landPool.transform);
            Debug.Log(sub.localPosition);
            Debug.Log(GameManager.Instance.cubeTable[sub.localPosition].parent.name);
        }*/

        foreach (ChildCube cc in childCubes)
        {
            cc.transform.SetParent(GameManager.Instance.landPool.transform);
            cc.DestroyFlags();
            //Debug.Log(sub.localPosition);
            //Debug.Log(GameManager.Instance.cubeTable[sub.localPosition].parent.name);
        }

        GameManager.Instance.currentCube = null;

        Destroy(gameObject);
    }
}
