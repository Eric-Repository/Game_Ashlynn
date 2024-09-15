using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Sprite Onsprite;
    public Sprite Offsprite;

    //assign setting menu UI
    public GameObject settingMenuUI;

    public void PlayGame()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
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

    public void LoadSetting()
    {

        //display setting panel
        settingMenuUI.SetActive(true);
    }

    public void Back()
    {

        //hide setting panel
        settingMenuUI.SetActive(false);

    }
}
