using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    #region Singleton

    public static Puzzle instance;

    private void Awake()
    {
        instance = this;

    }

    #endregion

    public float offsetX = 120f;

    public GameObject puzzleParent;
    public bool isPuzzleUp = false;
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;
    public GameObject image5;
    public GameObject image6;
    public GameObject animation1;
    public GameObject animation2;    
    public GameObject animation3;
    public GameObject animation4;
    public GameObject animation5;
    public GameObject animation6;
    public Item[] rewards;
    public GameObject hintPrefeb;
    public GameObject hintParent;


    GameObject parent;
    bool puzzle1Completed = false;
    bool puzzle2Completed = false;
    bool puzzle3Completed = false;
    bool puzzle4Completed = false;
    bool puzzle5Completed = false;
    bool puzzle6Completed = false;


    private void Update()
    {
        if (puzzleParent.activeSelf)
        {
            //show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        if (puzzle1Completed && puzzle2Completed && puzzle3Completed&& puzzle4Completed && puzzle5Completed && puzzle6Completed)
        {
            //show the ending scene
        }
    }

    public void showPuzzle()
    {
        isPuzzleUp = true;
        puzzleParent.SetActive(true);
    }
    public void p1()
    {

        if (Inventory.instance.HasItem(rewards[0]))
        {
            animation1.SetActive(true);
            puzzle1Completed = true;
        }
        else
        {
            //hint missing rewards
            puzzleHint(image1, "Reward 01");
        }

    }

    public void p2()
    {

        if (Inventory.instance.HasItem(rewards[1]))
        {
            animation2.SetActive(true);
            puzzle2Completed = true;

        }
        else
        {
            //hint missing rewards
            puzzleHint(image2, "Reward 02");
        }

    }

    public void p3()
    {
        if (Inventory.instance.HasItem(rewards[2])){
            animation3.SetActive(true);
            puzzle3Completed = true;

        }
        else
        {
            //hint missing rewards
            puzzleHint(image3, "Reward 03");
        }

    }

    public void p4()
    {

        if (Inventory.instance.HasItem(rewards[3]))
        {
            animation4.SetActive(true);
            puzzle4Completed = true;

        }
        else
        {
            //hint missing rewards
            puzzleHint(image4, "Reward 04");
        }

    }

    public void p5()
    {

        if (Inventory.instance.HasItem(rewards[4]))
        {
            animation5.SetActive(true);
            puzzle5Completed = true;

        }
        else
        {
            //hint missing rewards
            puzzleHint(image5, "Reward 05");
        }

    }

    public void p6()
    {

        if (Inventory.instance.HasItem(rewards[5]))
        {
            animation6.SetActive(true);
            puzzle6Completed = true;

        }
        else
        {
            //hint missing rewards
            puzzleHint(image6, "Reward 06");
        }

    }

    public void closePuzzleWindow()
    {
        isPuzzleUp = false;
        puzzleParent.SetActive(false);
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void puzzleHint(GameObject puzzle, string name)
    {
        var clone = Instantiate(hintPrefeb);
        var canvas = GameObject.Find("Canvas");
        clone.transform.SetParent(hintParent.transform, false);
        var rect = canvas.transform as RectTransform;
        clone.transform.position = new Vector3(puzzle.transform.position.x + offsetX * rect.localScale.x, puzzle.transform.position.y, 0);
        clone.transform.GetChild(0).GetComponent<Text>().text = "Missing " + name;
        StartCoroutine(disappear());
    }



    IEnumerator disappear()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform child in hintParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
