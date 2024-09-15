using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver3 : MonoBehaviour
{
    #region Singleton

    public static QuestGiver3 instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public Item key03;
    public Quest quest;
    public Player player;
    public GameObject playerObject;
    public GameObject rewardOBJ;
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public GameObject cameraBGObj;
    public CameraBG cg;
    public GameObject rewardsPrefabs;
    public Transform rewardsParent;
    public GameObject portalKey;
    public bool dolphinActive = false;
    public GameObject questFinish;

    public GameObject QuestText;
    public GameObject QuestFlag;
    public Button DolphinButton;
    public Sprite dolphinImg;

    float currentTime = 0f;

    Vector3 tempLocation;
    bool enable = true;
    float defaultTime;
    int keyNum = 0;


  


    private void Update()
    {
        if (QuestText.activeSelf && quest.isActive)
        {
            
            QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Find Key: " + keyNum + " / 1";
            if (Inventory.instance.HasItem(key03))
            {
                keyNum = 1;
                //game win
                QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Find Key: " + keyNum + " / 1 (Completed)";
                questUI(questFinish);

                QuestCompletedTextDis();
                quest.isActive = false;

            }
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

    

    public void AcceptQuest()
    {
        //resume the time
        Time.timeScale = 1f;
        cameraBGObj.SetActive(false);
        dolphinActive = true;
        tempLocation = player.transform.position;
        enable = false;
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
     
        questWindow.SetActive(false);
        quest.isActive = true;
        //defaultTime = startingTime;
        QuestText.SetActive(true);
        QuestText.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = quest.title;
        DolphinButton.GetComponent<Image>().sprite = dolphinImg;
        DolphinButton.onClick.Invoke();
        MindScript.instance.dolphinON();
        player.quest = quest;

        
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
