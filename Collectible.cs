
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Collectible")]
public class Collectible : Item
{
	public int index;


	// This is called when pressed in the inventory
	public override void Use()
	{

		Debug.Log(name + " used");

		RemoveFromInventory();  // Remove the item after use
	}


	
}
