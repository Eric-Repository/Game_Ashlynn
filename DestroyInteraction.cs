using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInteraction : MonoBehaviour
{
    public GameObject building;
    public GameObject destroyedBuilding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroy()
    {
        // Disables the old (regular building) and replaces it with a broken one
        building.SetActive(false);
        destroyedBuilding.SetActive(true);
        // Applies physics force to the broken building
        Rigidbody[] rubble = destroyedBuilding.GetComponentsInChildren<Rigidbody>();
        if (rubble.Length > 0)
        {
            foreach (var piece in rubble)
            {
                piece.AddExplosionForce(5000, destroyedBuilding.transform.position, 0.25f);
            }
        }
        // After 5 seconds the rubble of the broken building will be removed
        Invoke("removeRubble", 1.5f);
    }

    public void removeRubble()
    {
        // Disables/cleans up the rubble left from the broken building
        destroyedBuilding.SetActive(false);
    }
}
