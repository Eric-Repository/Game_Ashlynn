using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver : MonoBehaviour
{
    #region Singleton

    public static QuestGiver instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public Quest quest;
    public Player player;
    public GameObject playerObject;
    public GameObject rewardOBJ;
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public CameraBG cg;
    public GameObject rewardsPrefabs;
    public Transform rewardsParent;
    public GameObject timerObject;
    public Text countdownText;
    public GameObject questFinish;
    public GameObject questFail;
    public GameObject QuestText;
    public GameObject QuestFlag;
    float currentTime = 0f;
    public float startingTime = 15f;
    Vector3 tempLocation;
    bool enable = true;
    float defaultTime;

    void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        if (QuestText.activeSelf && quest.isActive)
        {
            int distText = distance();
            QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Distance: " + distText + " m";
        }

        
        
    }



    public void OpenQuestWindow()
    {
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //pause the time
        //Time.timeScale = 0f;
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        if (rewardsParent.transform.childCount == 0)
        {
            for (int i = 0; i < quest.rewards.Length; i++)
            {
                var reward = Instantiate(rewardsPrefabs);

                reward.transform.SetParent(rewardsParent);
                reward.GetComponent<Image>().sprite = quest.rewards[i];
                reward.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            }
        }
        else
        {
            foreach(Transform child in rewardsParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < quest.rewards.Length; i++)
            {
                var reward = Instantiate(rewardsPrefabs);

                reward.transform.SetParent(rewardsParent);
                reward.GetComponent<Image>().sprite = quest.rewards[i];
                reward.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            }
        }
    }

    public int distance()
    {
        float dist = Vector3.Distance(player.transform.position, GameObject.FindGameObjectWithTag("GoalPoint").transform.position);
        int myDist = (int)dist;
        return myDist;
    }

    public void AcceptQuest()
    {
        //pause the time
        Time.timeScale = 1f;
        tempLocation = player.transform.position;
        enable = false;
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        timerObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        countdownText.text = startingTime.ToString();
        questWindow.SetActive(false);
        quest.isActive = true;
        defaultTime = startingTime;
        QuestText.SetActive(true);
        QuestText.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = quest.title;
        

        
           
        padAppear();
        startCounting();

        //create quest array of quest class in player class
        player.quest = quest;

        
    }

    public void RewardAppear()
    {
        rewardOBJ.SetActive(true);
    }

    public void padAppear()
    {
        var pads = GameObject.FindGameObjectsWithTag("LilyPad");
     
        foreach(var p in pads)
        {
           foreach(Transform child in p.transform)
            {
                child.gameObject.SetActive(true);
            }

        }
    }

    public void padDisappear()
    {
        var pads = GameObject.FindGameObjectsWithTag("LilyPad");

        foreach (var p in pads)
        {
            foreach (Transform child in p.transform)
            {
                child.gameObject.SetActive(false);
            }

        }
    }



    public void interaction()
    {
        if (this.tag == "ChallengeKey" && enable == true)
        {
            OpenQuestWindow();
            cg.UpdateCameraPosition();
        }
    }


    public void CloseQuestWindow()
    {

        //pause the time
        Time.timeScale = 1f;
        questWindow.SetActive(false);
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        countdownText.gameObject.SetActive(false);

        titleText.text = null;
        descriptionText.text = null;
        foreach(Transform g in rewardsParent)
        {
            GameObject.Destroy(g.gameObject);
        }
    }

    public void startCounting()
    {
        countdownText.color = Color.white;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(startingTime >= 0)
        {
            
            if (startingTime <= 5)
            {
                countdownText.color = Color.red;
            }

           
            countdownText.text = startingTime.ToString("0");
            yield return new WaitForSeconds(1f);
            startingTime--;


        }
        //if timer less than 0 and quest is not finished, show fail and re-active challenge
        //show quest fail ui
        if (quest.isActive)
        {
            padDisappear();
            countdownText.gameObject.SetActive(false);
            StartCoroutine(Fail());
        }

        
    }

    public void questUI(GameObject gameObject)
    {
        
        StartCoroutine(WindowUp(gameObject));
    }

    IEnumerator WindowUp(GameObject obj)
    { 
        yield return new WaitForSeconds(0.2f);
        obj.SetActive(true);
     

        yield return new WaitForSeconds(1.7f);
        obj.SetActive(false);
      
    }


    IEnumerator Fail()
    {
        yield return new WaitForSeconds(0.2f);
      
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        questFail.SetActive(true);
        // pause the time
        Time.timeScale = 0f;
    }


    public void ResetQuest()
    {
        //pause the time
        Time.timeScale = 1f;
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        questFail.SetActive(false);
        //spawn player in front of the starting point
        playerObject.SetActive(false);
        playerObject.transform.position = tempLocation;
        playerObject.SetActive(true);
        startingTime = defaultTime;
        enable = true;
        quest.isActive = false;
        int distText = distance();
        QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Distance: " + distText + " m";
        QuestText.SetActive(false);
    }


    public void QuestCompletedTextDis()
    {
        QuestFlag.SetActive(false);
        StartCoroutine(Dis());
    }

    IEnumerator Dis()
    {

        yield return new WaitForSeconds(10f);
        QuestText.SetActive(false);


    }
}
