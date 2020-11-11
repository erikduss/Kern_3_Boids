using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    [SerializeField] private GameObject birdPrefab;
    public static int flyingSpaceSize = 7; //The size of flying area for the agents (in this case between -7 and 7 on the x and the y)

    private static int numberOfBirds = 20;
    public static GameObject[] allBirds = new GameObject[numberOfBirds];

    public static Vector3 goalPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< numberOfBirds; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-flyingSpaceSize, flyingSpaceSize), Random.Range(-flyingSpaceSize, flyingSpaceSize), Random.Range(-flyingSpaceSize, flyingSpaceSize));

            allBirds[i] = (GameObject)Instantiate(birdPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move the goal position of the flock to a new random position
        if(Random.Range(0,10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-flyingSpaceSize, flyingSpaceSize), Random.Range(-flyingSpaceSize, flyingSpaceSize), Random.Range(-flyingSpaceSize, flyingSpaceSize));

        }
    }
}
