using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour
{

	public GameObject inventoryUI;  // The entire UI
	public Transform itemsParent;   // The parent object of all the items

	Inventory inventory;    // Our current inventory

	void Start()
	{
		inventory = Inventory.instance;
		
		inventory.OnItemChangedCallback += UpdateUI;
	}

	// Check to see if we should open/close the inventory
	void Update()
	{
		//if (Input.GetButtonDown("Inventory"))
		if(Input.GetKeyDown(KeyCode.B))
		{
		

			if (!inventoryUI.activeSelf)
            {
				//show cursor
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				inventoryUI.SetActive(true);
				UpdateUI();
				//pause the time
				Time.timeScale = 0f;
            }
            else
            {
				//hide cursor
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				inventoryUI.SetActive(false);
				UpdateUI();
				//pause the time
				Time.timeScale = 1f;
			}
		}
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI()
	{
		InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}

	public void Close()
    {
        if (inventoryUI.activeSelf)
        {
			//hide cursor
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			inventoryUI.SetActive(false);
			//start the time
			Time.timeScale = 1f;
		}
    }

}