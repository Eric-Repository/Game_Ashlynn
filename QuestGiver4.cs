using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver4 : MonoBehaviour
{
    #region Singleton

    public static QuestGiver4 instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public Quest quest;
    public Player player;
    public GameObject playerObject;
    public GameObject rewardOBJ;
    public GameObject rewardOBJ2;
    public GameObject rewardOBJ3;

    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public GameObject cameraBGObj;
    public CameraBG cg;
    public GameObject rewardsPrefabs;
    public Transform rewardsParent;
    public bool birdActive = false;
    public GameObject questFail;
    public GameObject questFinish;

    public GameObject QuestText;
    public GameObject QuestFlag;
    public GameObject timerObject;
    public Text countdownText;
    public Button birdButton;
    public Button humanButton;

    public Sprite birdImg;
    public GameObject circlePrefeb;

    float currentTime = 0f;
    public float startingTime = 60f;
    public GameObject world4;
    public GameObject tempPlace;

    Vector3 tempLocation;
    bool enable = true;
    float defaultTime;
    bool gameWin = false;
    GameObject tempCircleHolder;

    void Start()
    {
        currentTime = startingTime;
        quest.goal.currentAmount = 0;
    }


    private void Update()
    {
        if (QuestText.activeSelf && quest.isActive)
        {
            
            QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Points gain: " + quest.goal.currentAmount + " / " + quest.goal.requiredAmount;
            if (quest.goal.currentAmount >= quest.goal.requiredAmount)
            {
                //game win
                gameWin = true;
                rewardOBJ.SetActive(true);
                rewardOBJ2.SetActive(true);
                questUI(questFinish);
                quest.isActive = false;
                countdownText.gameObject.SetActive(false);
            }
        }

        
        
    }



    public void OpenQuestWindow()
    {
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //pause the time
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

    

    public void AcceptQuest()
    {
        //resume the time
        Time.timeScale = 1f;
        cameraBGObj.SetActive(false);
        birdActive = true;
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
        QuestText.SetActive(true);
        QuestText.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = quest.title;
        rewardOBJ3.SetActive(true);
        player.quest = quest;
        birdButton.GetComponent<Image>().sprite = birdImg;
        birdButton.onClick.Invoke();
        MindScript.instance.birdON();
        defaultTime = startingTime;
        QuestText.SetActive(true);
        QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Points gain: " + quest.goal.currentAmount + " / " + quest.goal.requiredAmount;
        tempCircleHolder = Instantiate(circlePrefeb);
        tempCircleHolder.transform.SetParent(world4.transform, false);
        tempCircleHolder.transform.position = tempPlace.transform.position;
        startCounting();

    }

    public void RewardAppear()
    {
        rewardOBJ.SetActive(true);
    }

   
    


    public void interaction()
    {
        if (this.tag == "ChallengeKey" && enable == true)
        {
            OpenQuestWindow();
            cameraBGObj.SetActive(true);
            cameraBGObj.GetComponent<CameraBG>().UpdateCameraPosition();
            //cg.UpdateCameraPosition();
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
        QuestFlag.SetActive(false);
        QuestText.SetActive(false);
    }


   


   



    public void startCounting()
    {
        countdownText.color = Color.white;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (startingTime >= 0)
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
        if ((quest.isActive && quest.goal.currentAmount < quest.goal.requiredAmount)|| !gameWin)
        {
            countdownText.gameObject.SetActive(false);

            StartCoroutine(Fail());
        }


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

        //change back to human
        humanButton.onClick.Invoke();
        MindScript.instance.humanON();

        Destroy(tempCircleHolder);
        quest.goal.currentAmount = 0;
        QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Points gain: " + quest.goal.currentAmount + " / " + quest.goal.requiredAmount;
        QuestText.SetActive(false);
    }
}
