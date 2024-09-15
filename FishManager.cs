using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    // The size of the tank that will determine where the fish can spawn
    public static int spawnPosition = 2;
    public int tankSize = 60;
    public int tankHeight = 3;
    public Transform tankPosition;
    public Transform tankCenter;
    public static int numOfFish;

    // The array that will hold all of the fish
    public GameObject[] allFish = new GameObject[numOfFish];

    // The goal position of the flock
    public Vector3 goalPos = Vector3.zero;
    public GameObject goalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // There is a restriction to the position of the goal depending on the tank
        goalPos = new Vector3(Random.Range(-tankSize, tankSize) + tankCenter.position.x,
                                Random.Range(-tankHeight, tankHeight) + tankCenter.position.y,
                                Random.Range(-tankSize, tankSize) + tankCenter.position.z);
        goalPrefab.transform.position = goalPos;
        for (int i = 0; i < numOfFish; i ++)
        {
            // Generates a random position inside of the tank
            Vector3 pos = new Vector3(Random.Range(-spawnPosition, spawnPosition),
                                        Random.Range(-spawnPosition, spawnPosition),
                                        Random.Range(-spawnPosition, spawnPosition));
            // Instantiates a fish at the random position
            allFish[i].transform.position = pos + tankPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Will reset the goalPosition randomly
        // 50/10000 chance
        if (Random.Range(0, 1000) < 50)
        {
            // There is a restriction to the position of the goal depending on the tank
            goalPos = new Vector3(Random.Range(-tankSize, tankSize) + tankCenter.position.x,
                                    Random.Range(-tankHeight, tankHeight) + tankCenter.position.y,
                                    Random.Range(-tankSize, tankSize) + tankCenter.position.z);
            goalPrefab.transform.position = goalPos;
        }
    }
}
