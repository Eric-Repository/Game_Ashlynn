using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

	public Image icon;
	public GameObject PopUpWindowPrefab;
	public float offsetX = 400f;
	

	Item item;  // Current item in the slot
	//Item item = new Collectible();
	GameObject parent;

    private void Awake()
    {
		parent = GameObject.Find("Inventory");
	}

    // Add item to the slot
    public void AddItem(Item newItem)
	{
		item = newItem;

		icon.sprite = item.icon;
		icon.enabled = true;
	}

	// Clear the slot
	public void ClearSlot()
	{
		item = null;

		icon.sprite = null;
		icon.enabled = false;
	}

	//// If the remove button is pressed, this function will be called.
	//public void RemoveItemFromInventory()
	//{
	//	Inventory.instance.Remove(item);
	//}

	// Use the item
	public void UseItem()
	{
		if (item != null)
		{
			item.Use();
		}
	}

	public void WindowPopUp()
    {
		//if there is item in this slot then window pop up

		if (item != null)
		{

			var clone = Instantiate(PopUpWindowPrefab);
			var canvas = GameObject.Find("Canvas");
			clone.transform.SetParent(parent.transform.GetChild(3).transform, false);
			var rect = canvas.transform as RectTransform;
			clone.transform.position = new Vector3(transform.position.x + offsetX * rect.localScale.x, transform.position.y, 0);
			clone.transform.GetChild(0).GetComponent<Text>().text = item.name;
			clone.transform.GetChild(1).GetComponent<Text>().text = item.description;
			
			

		}
    }

	public void WindowOff()
	{
        foreach (Transform child in parent.transform.GetChild(3).transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}

