using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;
	public bool withinTriggerRange = false;
	public bool withinQuestRange = false;
	public bool withinPuzzleRange = false;

	public GameObject otherObject;
	public PortalKey otherScript;
	public QuestGiver questScript;
	public QuestGiver2 questScript2;
	public QuestGiver3 questScript3;
	public QuestGiver4 questScript4;


	public bool colliderWithQuest1 = false;
	public bool colliderWithQuest2 = false;
	public bool colliderWithQuest3 = false;
	public bool colliderWithQuest4 = false;


	// hint text
	public GameObject hint;

	public Item key01;
	public Item key02;
	public Item key03;
	public Item reward4;
	public Item reward5;
	public Item reward6;


	public Interactable focus;  // Our current focus: Item, Enemy etc.
	bool firstTime = true;
	bool firstDolphinTime = true;
	bool firstBirdTime = true;

	public PortalManager portalManager;

	//public LayerMask interactionMask;   // Everything we can interact with



	// Update is called once per frame
	void Update()
	{
		// if (EventSystem.current.IsPointerOverGameObject())
		//    return;


	}


	private void OnTriggerEnter(Collider other)
	{

		//Debug.Log("enter");
		if (other.tag == "PortalKey")
		{


			withinTriggerRange = true;
			otherObject = other.gameObject.transform.parent.gameObject;
			otherScript = otherObject.GetComponent<PortalKey>();



			if (otherObject.name == "Key")
			{
				//for Tutorial world
				if (otherScript.isThePortalOn)
				{
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
				}
				else
				{
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
				}

			}
			else if (otherObject.name == "DinoKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key01))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}
			else if (otherObject.name == "DolphinKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key02))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}
			else if (otherObject.name == "BirdKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key03))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}




			hint.SetActive(true);

		}


		if (other.tag == "ChallengeKey" && !QuestGiver.instance.quest.isActive && other.name == "Cube" && !Inventory.instance.HasItem(key01))
		{
			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Interact";

			hint.SetActive(true);
			colliderWithQuest1 = true;
			otherScript = null;
			withinQuestRange = true;
			questScript = other.gameObject.transform.parent.gameObject.GetComponent<QuestGiver>();


		}

		if (other.tag == "ChallengeKey" && !QuestGiver2.instance.quest.isActive && other.name == "Cube2" && !Inventory.instance.HasItem(key02))
		{
			hint.SetActive(true);

			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Interact";

			otherScript = null;
			withinQuestRange = true;
			colliderWithQuest2 = true;
			questScript2 = other.gameObject.transform.parent.gameObject.GetComponent<QuestGiver2>();

		}

		if (other.tag == "ChallengeKey" && !QuestGiver3.instance.quest.isActive && other.name == "Cube3" && !Inventory.instance.HasItem(key03))
		{
			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Interact";

			hint.SetActive(true);

			otherScript = null;
			withinQuestRange = true;
			colliderWithQuest3 = true;
			questScript3 = other.gameObject.transform.parent.gameObject.GetComponent<QuestGiver3>();

		}


		if (other.tag == "ChallengeKey" && !QuestGiver4.instance.quest.isActive && other.name == "Cube4")
		{
			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Interact";

			hint.SetActive(true);

			otherScript = null;
			withinQuestRange = true;
			colliderWithQuest4 = true;
			questScript4 = other.gameObject.transform.parent.gameObject.GetComponent<QuestGiver4>();

		}


		if (other.tag == "GoalPoint" && QuestGiver.instance.startingTime >= 0f && QuestGiver.instance.quest.isActive)
		{
			QuestGiver.instance.QuestText.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Completed";
			QuestGiver.instance.quest.Complete();
			QuestGiver.instance.countdownText.gameObject.SetActive(false);
			QuestGiver.instance.questUI(QuestGiver.instance.questFinish);
			QuestGiver.instance.QuestCompletedTextDis();
			QuestGiver.instance.RewardAppear();
		}
		if (other.tag == "Item")
		{
			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Collect";
			hint.SetActive(true);

			SetFocus(other.GetComponent<Interactable>());
		}

		if (other.name == "Checker" && Inventory.instance.HasItem(key01))
		{
			portalManager.openTutorialWorld();
		}


		if (other.name == "EnterDinoWorldChecker" && firstTime)
		{
			IngameCutScene.instance.enterDinoworld = true;
			IngameCutScene.instance.PlayVideo();
			firstTime = false;
		} else if (other.name == "EnterDinoWorldChecker" && !firstTime && !QuestGiver2.instance.quest.isActive)
		{
			IngameCutScene.instance.enterDinoworld = false;

		} else if (other.name == "EnterDinoWorldChecker" && Inventory.instance.HasItem(key02))
		{
			portalManager.openDinoWorld();

		}


		if (other.name == "UnderwaterChecker" && firstDolphinTime)
		{
			IngameCutScene.instance.enterUnderWaterworld = true;
			IngameCutScene.instance.PlayVideo();
			firstDolphinTime = false;
		} else if (other.name == "UnderwaterChecker" && !firstDolphinTime && !QuestGiver3.instance.quest.isActive)
		{
			IngameCutScene.instance.enterUnderWaterworld = false;

		} else if (other.name == "UnderwaterChecker" && Inventory.instance.HasItem(key03))
		{
			portalManager.openDolphinWorld();
		}

		if (other.name == "birdWorldChecker" && firstBirdTime)
		{
			IngameCutScene.instance.enterFloatingIslandworld = true;
			IngameCutScene.instance.PlayVideo();
			firstBirdTime = false;
		}
		else if (other.name == "birdWorldChecker" && !firstDolphinTime && !QuestGiver4.instance.quest.isActive)
		{
			IngameCutScene.instance.enterFloatingIslandworld = false;

		}
		else if (other.name == "birdWorldChecker" && (Inventory.instance.HasItem(reward4)|| Inventory.instance.HasItem(reward5)|| Inventory.instance.HasItem(reward6)))
		{
			portalManager.openBirdWorld();
		}


		if (other.tag == "CirclePoint" && QuestGiver4.instance.quest.isActive)
		{
			QuestGiver4.instance.quest.goal.currentAmount++;

		}


		if (other.tag == "puzzle")
		{
			hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Interact";
			hint.SetActive(true);
			withinPuzzleRange = true;
		}

	}

	private void OnTriggerExit(Collider other)
    {
		if (other.tag == "PortalKey")
        {
			hint.SetActive(false);
			withinTriggerRange = false;
			otherObject = null;
			otherScript = null;
        }
		//Debug.Log("exit");

		if (other.tag == "ChallengeKey" || QuestGiver.instance.quest.isActive && other.name == "Cube")
		{
			hint.SetActive(false);
			withinQuestRange = false;
			questScript = null;
			colliderWithQuest1 = false;
		}

		if ((other.tag == "ChallengeKey" || QuestGiver2.instance.quest.isActive) && other.name == "Cube2")
		{
			hint.SetActive(false);
			withinQuestRange = false;
			questScript2 = null;
			colliderWithQuest2 = false;
		}

		if ((other.tag == "ChallengeKey" || QuestGiver3.instance.quest.isActive) && other.name == "Cube3")
		{
			hint.SetActive(false);
			withinQuestRange = false;
			questScript3 = null;
			colliderWithQuest3 = false;
		}

		if ((other.tag == "ChallengeKey" || QuestGiver4.instance.quest.isActive) && other.name == "Cube4")
		{
			hint.SetActive(false);
			withinQuestRange = false;
			questScript4 = null;
			colliderWithQuest4 = false;
		}

		if (other.tag == "Item")
		{
			hint.SetActive(false);
		}

		if (other.tag == "CirclePoint")
		{
			Destroy(other.transform.parent.gameObject);
		}


		if (other.tag == "puzzle")
		{
			hint.SetActive(false);
			withinPuzzleRange = false;
		}

	}

    private void OnTriggerStay(Collider other)
    {
		if (other.tag == "ChallengeKey" && QuestGiver.instance.quest.isActive)
		{
			hint.SetActive(false);
			

		}

		if (other.tag == "ChallengeKey" && QuestGiver2.instance.quest.isActive)
		{
			hint.SetActive(false);

		}

		if (other.tag == "ChallengeKey" && QuestGiver3.instance.quest.isActive)
		{
			hint.SetActive(false);

		}

		if (other.tag == "ChallengeKey" && QuestGiver4.instance.quest.isActive)
		{
			hint.SetActive(false);

		}

		if (other.tag == "Item")
		{
			hint.SetActive(true);

			SetFocus(other.GetComponent<Interactable>());
		}

        if (other.tag == "PortalKey")
        {
			withinTriggerRange = true;
			otherObject = other.gameObject.transform.parent.gameObject;
			otherScript = otherObject.GetComponent<PortalKey>();
			if (otherObject.name == "Key")
			{
				//for Tutorial world
				if (otherScript.isThePortalOn)
				{
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
				}
				else
				{
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
				}

			}
			else if (otherObject.name == "DinoKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key01))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}
			else if (otherObject.name == "DolphinKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key02))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}
			else if (otherObject.name == "BirdKey")
			{
				//for dino world portal key
				if (!Inventory.instance.HasItem(key03))
				{
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, 50);
					hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Missing Key";

				}
				else
				{
					if (otherScript.isThePortalOn)
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Close";
					}
					else
					{
						hint.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Open";
					}
					hint.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(140, 50);
				}

			}


			hint.SetActive(true);
		}

		if (other.tag == "puzzle")
		{
			hint.SetActive(true);
			withinPuzzleRange = true;
		}

	}




    // Set our focus to a new focus
    void SetFocus(Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (focus != newFocus && focus != null)
		{
			// Let our previous focus know that it's no longer being focused
			focus.OnDefocused();

		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		focus = newFocus;

		if (focus != null)
		{
			// Let our focus know that it's being focused
			focus.OnFocused(transform);
		}

	}
}
