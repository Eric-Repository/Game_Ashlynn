
using UnityEngine;

public class Interactable : MonoBehaviour
{
	//how close the player need to be to get interact with the object
	public float radius = 3f;
	bool isFocus = false;
	Transform player;
	public Transform interactionTransform;
	//public string InteractText = "Press F to pickup the item";


	bool hasInteracted = false; // Have we already interacted with the object?

	void Update()
	{
		if (isFocus)    // If currently being focused
		{
			float distance = Vector3.Distance(player.position, interactionTransform.position);
            // If we haven't already interacted and the player is close enough and press F to pick up
            //if (!hasInteracted && distance <= radius&& Input.GetKeyDown(KeyCode.F))
            //{
            //	// Interact with the object
            //	hasInteracted = true;
            //	Interact();
            //}

            if (!hasInteracted && distance <= radius)
            {
                // Interact with the object
                hasInteracted = true;
                //GameManager.instance.helpText.text = "Press F";
            }

            if (hasInteracted && Input.GetKeyDown(KeyCode.F))
            {
                Interact();

            }
        }
	}

    // Called when the object starts being focused
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        hasInteracted = false;
        player = playerTransform;
    }

    // Called when the object is no longer focused
    public void OnDefocused()
    {
        isFocus = false;
        hasInteracted = false;
        player = null;
    }

    // This method is meant to be overwritten
    public virtual void Interact()
	{
		Debug.Log("interacting with " + transform.name);
	}


	void OnDrawGizmosSelected()
	{

		if (interactionTransform == null)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}