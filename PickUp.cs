using UnityEngine;

public class PickUp : Interactable
{

	public Item item;   // Item to put in the inventory if picked up
	public GameObject hint;

    // When the player interacts with the item
    public override void Interact()
	{
		base.Interact();
		Pickup();
	}


    // Pick up the item
    void Pickup()
	{
			Debug.Log("Picking up " + item.name);

		hint.SetActive(false);
			Inventory.instance.Add(item);   // Add to inventory
			Destroy(gameObject);    // Destroy item from scene
		
	}

}
