using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public int space = 15; // Amount of item spaces

    //current list of items in the inventory
   public List<Item> items = new List<Item>();


	// Add a new item if enough room
	public void Add(Item item)
	{
		if (item.showInInventory)
		{
			if (items.Count >= space)
			{
				Debug.Log("Not enough room.");
				return;
			}

			items.Add(item);

			if (OnItemChangedCallback != null)
				OnItemChangedCallback.Invoke();
		}
	}

	// Remove an item
	public void Remove(Item item)
	{
		items.Remove(item);

		if (OnItemChangedCallback != null)
			OnItemChangedCallback.Invoke();
	}

	public bool HasItem(Item item)
	{
		return items.Contains(item);
	}

}
