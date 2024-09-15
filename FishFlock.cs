using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlock : MonoBehaviour
{
    // The move speed of the fish
    public float speed = 0.15f;
    // The speed of the fish's rotation
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 3.5f;
    public FishManager fishManager;
    GameObject[] gos;
    public Vector3 newGoalPos;

    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.25f, 0.75f);
        gos = fishManager.allFish;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!turning)
        {
            newGoalPos = this.transform.position - other.gameObject.transform.position;
        }
        turning = true;
    }

    void OnTriggerExit(Collider other)
    {
        turning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            Vector3 direction = fishManager.tankCenter.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.25f, 0.75f);
        }
        else
        {
            // There is a chance that the fish will not flock
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = fishManager.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;
                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    FishFlock anotherFlock = go.GetComponent<FishFlock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcenter + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                        Quaternion.LookRotation(direction), 
                                                        rotationSpeed * Time.deltaTime);
            }
        }
    }
}
