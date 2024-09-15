using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MindScript : MonoBehaviour
{
    #region Singleton

    public static MindScript instance;

    private void Awake()
    {
        instance = this;
        cameraSetting = freeLookCamera.GetComponent<CinemachineFreeLook>();
    }

    #endregion

    private GameObject[] charactersList;

    private GameObject currentCharacter;

    public GameObject freeLookCamera;
     CinemachineFreeLook cameraSetting;

    private int index;



    

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        //add all characters to an array
        charactersList = new GameObject[transform.childCount];
            for(int i  = 0; i < transform.childCount; i++)
            {
                charactersList[i] = transform.GetChild(i).gameObject;
            }

            //turn off their renderer
            foreach(GameObject go in charactersList)
            {
                go.SetActive(false);
            }

        //turn on the first index
        if (charactersList[index])
        {
            charactersList[index].SetActive(true);
        }

        
  
    }



    void Update()
    {
        //turn on main character
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            humanON();
        }
        //turn on bird
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            birdON();

        }
        //turn on dolphin
        if (Input.GetKeyDown(KeyCode.Alpha3) && QuestGiver3.instance.dolphinActive)
        {
            dolphinON();
        }
        //turn on dino
        if (Input.GetKeyDown(KeyCode.Alpha4) && QuestGiver2.instance.dinoActive)
        {
            dinoON();
        }
    }

    //change Characters
    public void ChangeCharacter(int index)
    {
        charactersList[index].SetActive(true);
    }

    public void birdON()
    {
        charactersList[index].SetActive(false);
        index = 1;
        ChangeCharacter(index);
    }


    public void humanON()
    {
        charactersList[index].SetActive(false);
        index = 0;
        ChangeCharacter(index);
        //this is top rig
        cameraSetting.m_Orbits[0].m_Height = 0.26f;
        cameraSetting.m_Orbits[0].m_Radius = 1.75f;
        //this is mid rig
        cameraSetting.m_Orbits[1].m_Height = 0.01f;
        cameraSetting.m_Orbits[1].m_Radius = 3f;
        //this is bot rig
        cameraSetting.m_Orbits[2].m_Height = 0f;
        cameraSetting.m_Orbits[2].m_Radius = 1.3f;

        cameraSetting.m_XAxis.m_MaxSpeed = 350f;
        cameraSetting.m_YAxis.m_MaxSpeed = 4f;
    }

    public void dolphinON()
    {
        charactersList[index].SetActive(false);
        index = 2;
        ChangeCharacter(index);

        //this is top rig
        cameraSetting.m_Orbits[0].m_Height = 2.1f;
        cameraSetting.m_Orbits[0].m_Radius = 0.41f;
        //this is mid rig
        cameraSetting.m_Orbits[1].m_Height = 0.16f;
        cameraSetting.m_Orbits[1].m_Radius = 2.31f;
        //this is bot rig
        cameraSetting.m_Orbits[2].m_Height = -1.04f;
        cameraSetting.m_Orbits[2].m_Radius = 0.55f;
        cameraSetting.m_XAxis.m_MaxSpeed = 350f;
        cameraSetting.m_YAxis.m_MaxSpeed = 2f;
    }


    public void dinoON() {
        charactersList[index].SetActive(false);
        index = 3;
        ChangeCharacter(index);

        //this is top rig
        cameraSetting.m_Orbits[0].m_Height = 3.28f;
        cameraSetting.m_Orbits[0].m_Radius = 2.37f;
        //this is mid rig
        cameraSetting.m_Orbits[1].m_Height = 2.28f;
        cameraSetting.m_Orbits[1].m_Radius = 5.51f;
        //this is bot rig
        cameraSetting.m_Orbits[2].m_Height = 0f;
        cameraSetting.m_Orbits[2].m_Radius = 1.3f;


        cameraSetting.m_XAxis.m_MaxSpeed = 200f;
        cameraSetting.m_YAxis.m_MaxSpeed = 0.5f;
    }

}
