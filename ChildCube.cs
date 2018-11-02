using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCube : MonoBehaviour {

    private Cube parentCube;

    private Vector3 localPos;

    private int cubeSize;

    [SerializeField]
    public GameObject[] flagPrefabs;

    private GameObject[] flags;

    public Vector3 LocalPos
    {
        get
        {
            return localPos;
        }

        set
        {
            //***************** PREVENT OVERWRITE OTHER CHILDCUBES BY MISTAKE !!!!!!!!!!!!!!
            if (GameManager.Instance.cubeTable[localPos] == this)
            {
                GameManager.Instance.cubeTable[localPos] = null;
            }

            localPos = value;
            transform.localPosition = localPos;
            UpdateFlags();
           
            GameManager.Instance.cubeTable[localPos] = this;
            //Debug.Log(GameManager.Instance.cubeTable[localPos].name);
            //Debug.Log(name);

            /*foreach (Vector3 pos in GameManager.Instance.cubeTable.Keys)
            {
                if (GameManager.Instance.cubeTable[pos] != null)
                {
                    //Debug.Log(cc.LocalPos);
                    Debug.Log(GameManager.Instance.cubeTable[pos].name);
                    Debug.Log(pos);
                }
            }*/

        }
    }

    private void Awake()
    {
        cubeSize = GameManager.Instance.GetCubeSize();
        localPos = transform.localPosition;
        flags = new GameObject[5];
    }

    // Use this for initialization
    void Start () {   

        parentCube = transform.GetComponentInParent<Cube>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitialFlags()
    {
        for(int i=0; i<flagPrefabs.Length; i++)
        {
            flags[i] = Instantiate(flagPrefabs[i]);
            flags[i].transform.SetParent(this.transform);
            //Debug.Log(flags[i].name);
        }

        UpdateFlags();

    }

    private void GroundFlag()
    {

        if (flags[0] == null)
        {
            return;
        }

        //Store the top layer that has a childcube on the x,z.
        int topLayer = -1;

        //Find the top layer by traverse
        foreach (KeyValuePair<Vector3, ChildCube> pair in GameManager.Instance.cubeTable)
        {
            if (pair.Value != null && pair.Key.x == localPos.x && pair.Key.z == localPos.z && pair.Value.transform.parent.name == "LandPool" && pair.Key.y > topLayer)
            {
                topLayer = (int)pair.Key.y;              
            }
        }

        //Debug.Log(topLayer);

        flags[0].transform.position = new Vector3(transform.position.x, (topLayer + 1) * cubeSize + 0.001f, transform.position.z);
        /*if (topLayer == -1)
        {
            flags[0].transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
        } else
        {
            flags[0].transform.position = new Vector3(transform.position.x, (topLayer+1) * cubeSize + 0.001f, transform.position.z);
        }*/
    }

    private void PosiXFlag()
    {
        if (flags[1] == null)
        {
            return;
        } else if (GameManager.Instance.GetViewLocation() == 2 || GameManager.Instance.GetViewLocation() == 3)
        {
            flags[1].SetActive(false);
        } else
        {
            flags[1].SetActive(true);
        }

        flags[1].transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void PosiZFlag()
    {
        if (flags[2] == null)
        {
            return;

        } else if (GameManager.Instance.GetViewLocation() == 3 || GameManager.Instance.GetViewLocation() == 4)
        {
            flags[2].SetActive(false);
        } else
        {
            flags[2].SetActive(true);
        }

        flags[2].transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    private void NegaXFlag()
    {
        if (flags[3] == null)
        {
            return;

        } else if (GameManager.Instance.GetViewLocation() == 1 || GameManager.Instance.GetViewLocation() == 4)
        {
            flags[3].SetActive(false);
        } else
        {
            flags[3].SetActive(true);
        }

        flags[3].transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.GetMapWidth());
    }

    private void NegaZFlag()
    {
        if (flags[4] == null)
        {
            return;
        } else if (GameManager.Instance.GetViewLocation() == 1 || GameManager.Instance.GetViewLocation() == 2)
        {
            flags[4].SetActive(false);
        } else
        {
            flags[4].SetActive(true);
        }

        flags[4].transform.position = new Vector3(GameManager.Instance.GetMapWidth(), transform.position.y, transform.position.z);
    }

    public void UpdateFlags()
    {
        GroundFlag();
        PosiXFlag();
        PosiZFlag();
        NegaXFlag();
        NegaZFlag();
    }

    public void DestroyFlags()
    {
        for (int i = 0; i < flags.Length; i++)
        {
            Destroy(flags[i]);
            //Debug.Log(flags[i].name);
        }
    }

    public void DestroyWhenRemove()
    {
        Destroy(gameObject);
    }
}
