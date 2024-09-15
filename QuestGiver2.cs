using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver2 : MonoBehaviour
{
    #region Singleton

    public static QuestGiver2 instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    public Item key02;
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

    public GameObject questFinish;
    public GameObject QuestText;
    public GameObject QuestFlag;
    public bool dinoActive = false;
    public GameObject buildingParent;
    public Button DinoButton;
    public Sprite dinoImg;
    float currentTime = 0f;

    Vector3 tempLocation;
    bool enable = true;
    float defaultTime;
    int keyNum = 0;
    int randomBuildingIndex;
    int randomBuildingIndex2;

    void Start()
    {
        int childCount = buildingParent.transform.childCount;
        randomBuildingIndex = Random.Range(0, childCount);
        randomBuildingIndex2 = Random.Range(0, childCount);
    }


    private void Update()
    {
        if (QuestText.activeSelf && quest.isActive)
        {
            
            QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Find Key: " + keyNum + " / 1";
            if (Inventory.instance.HasItem(key02))
            {
                keyNum = 1;
                //game win
                QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Find Key: " + keyNum + " / 1 (Completed)";
                questUI(questFinish);

                quest.isActive = false;

                QuestCompletedTextDis();
            }
        }

        
        
    }



    public void OpenQuestWindow()
    {
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //pause the time
        Time.timeScale = 0f;
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
        buildingParent.SetActive(true);
        portalKey.transform.position = new Vector3(buildingParent.transform.GetChild(7).position.x, 0f, buildingParent.transform.GetChild(7).position.z);

        //portalKey.transform.position = new Vector3(buildingParent.transform.GetChild(randomBuildingIndex).position.x, 0f, buildingParent.transform.GetChild(randomBuildingIndex).position.z);
        rewardOBJ.transform.position = new Vector3(buildingParent.transform.GetChild(11).position.x, 0f, buildingParent.transform.GetChild(11).position.z);

        //rewardOBJ.transform.position = new Vector3(buildingParent.transform.GetChild(randomBuildingIndex2).position.x, 0f, buildingParent.transform.GetChild(randomBuildingIndex2).position.z);
    }

    

    public void AcceptQuest()
    {
        //resume the time
        Time.timeScale = 1f;
        cameraBGObj.SetActive(false);
        tempLocation = player.transform.position;
        enable = false;
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dinoActive = true;
        questWindow.SetActive(false);
        quest.isActive = true;
        //defaultTime = startingTime;
        QuestText.SetActive(true);
        QuestText.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = quest.title;
        DinoButton.GetComponent<Image>().sprite = dinoImg;
        DinoButton.onClick.Invoke();
        MindScript.instance.dinoON();
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
        buildingParent.SetActive(false);

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
