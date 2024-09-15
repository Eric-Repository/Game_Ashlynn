using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InGameMenu : MonoBehaviour
{
    //default game is Not paused 
    public static bool GameIsPaused = false;
    //assign pause menu UI
    public GameObject pauseMenuUI;

    //assign setting menu UI
    public GameObject settingMenuUI;

    //assign setting menu UI
    public GameObject controlMenuUI;

    //default map is Not showed 
    public static bool MapIsShow = false;
    //assign pause manu UI
    public GameObject mapUI;

    public GameObject mask;

    //default Inventory is Not showed 
    public static bool InvIsHidden = false;

    //top down camera
    public Camera topdownCamera;
    public Scrollbar scrollbar;

    public float cameraMax = 50.0f;
    public float cameraMin = 13.0f;

    public GameObject miniIcon;

    public Sprite human;
    public Sprite bird;
    public Sprite dolphin;
    public Sprite dinosaur;

    public Sprite Onsprite;
    public Sprite Offsprite;
    public Image img;

    void Start()
    {
        
        miniIcon.GetComponent<SpriteRenderer>().sprite = human;
        StartCoroutine(FadeImage());

    }

    // Update is called once per frame
    void Update()
    {
        //pause game
        //if press key 
        if (Input.GetKeyDown(KeyCode.Escape) && MapIsShow == false && IngameCutScene.instance.isVideoPlaying == false)
        {
            //Cursor.visible = true;
            //if the game is paused
            if (GameIsPaused)
            {
                
                Resume();
            }
            //if the game is not paused
            else
            {
                
                Pause();
            }
        }

        //map k
        //if press key 
        if (Input.GetKeyDown(KeyCode.M) && !pauseMenuUI.activeSelf)
        {
            //if the game is paused
            if (MapIsShow)
            {
                HiddenMap();
            }
            //if the game is not paused
            else
            {
                AppearMap();
            }
        }

        //map mini icon for human
        //if press key 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            miniIcon.GetComponent<SpriteRenderer>().sprite = human;
        }

        //map mini icon for bird
        //if press key 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            miniIcon.GetComponent<SpriteRenderer>().sprite = bird;
        }

        //map mini icon for dolphin
        //if press key 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            miniIcon.GetComponent<SpriteRenderer>().sprite = dolphin;
        }

        //map mini icon for dinosaur
        //if press key 
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            miniIcon.GetComponent<SpriteRenderer>().sprite = dinosaur;
        }



    }

    public void Resume()
        {
            //hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //set the pause menu ui to disable
            pauseMenuUI.SetActive(false);
            //resume the time
            Time.timeScale = 1f;
            //change the status to false
            GameIsPaused = false;

    }

    public void Pause()
        {
             //show cursor
             Cursor.lockState = CursorLockMode.None;
             Cursor.visible = true;
             //active the pause menu ui
             pauseMenuUI.SetActive(true);
            //pause the time
            Time.timeScale = 0f;
            //change the status to true
            GameIsPaused = true;
        }

    public void Home()
    {
        //set the pause menu ui to disable
        pauseMenuUI.SetActive(false);
        //resume the time
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        //change the status to false
        GameIsPaused = false;
    }


    public void HiddenMap()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //set the pause menu ui to disable
        mapUI.SetActive(false);
        //resume the time
        Time.timeScale = 1f;
        //change the status to false
        MapIsShow = false;
    }

    void AppearMap()
    {
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //active the pause menu ui
        mapUI.SetActive(true);
        //pause the time
        Time.timeScale = 0f;
        //change the status to true
        MapIsShow = true;
    }

   

    public void LoadSetting()
    {
        //hide menu panel
        pauseMenuUI.SetActive(false);
        //display setting panel
        settingMenuUI.SetActive(true);
    }

    public void HideSetting()
    {
        //show menu panel
        pauseMenuUI.SetActive(true);
        //hide setting panel
        settingMenuUI.SetActive(false);
    }

    public void LoadControl()
    {
        //hide menu panel
        pauseMenuUI.SetActive(false);
        //display setting panel
        controlMenuUI.SetActive(true);
    }

    public void HideControl()
    {
        //show menu panel
        pauseMenuUI.SetActive(true);
        //hide setting panel
        controlMenuUI.SetActive(false);
    }

    public void toggleButton()
    {
        var toggles = GameObject.FindGameObjectsWithTag("toggle");

        foreach (var t in toggles)
        {
            if (t.GetComponent<Slider>().value == 0)
            {
                //t.transform.GetChild(0).GetComponent<Image>().sourceimage = Offsprite;
                t.transform.GetChild(0).GetComponent<Image>().sprite = Offsprite;

            }
            else
            {
                //t.transform.GetChild(0).GetComponent<Image>().sourceimage = Onsprite;
                t.transform.GetChild(0).GetComponent<Image>().sprite = Onsprite;

            }
        }

    }

    public void cameraSize()
    {
        float oldRange = 1.0f;
        float newRange = cameraMax - cameraMin;
        float finalValue = (scrollbar.GetComponent<Scrollbar>().value) * newRange / oldRange + cameraMin;
        topdownCamera.GetComponent<Camera>().orthographicSize = finalValue;

    }


    public void BackTitle()
    {

        Time.timeScale = 1f;
        Resume();
        SceneManager.LoadScene(0);
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }


    IEnumerator FadeImage()
    {
        // fade from opaque to transparent
       
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        img.color = new Color(1, 1, 1, 0);
    }


}
