using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay;
    public int difficultyMultiplier;

    private float maxSpawnDelay;
    private int secondNumber;
    private int firstNumber;

    private GameObject topLeft;
    private GameObject bottomRight;

    void Start()
    {
        maxSpawnDelay = spawnDelay;
        firstNumber = 1;
        secondNumber = 0;
        topLeft = GameObject.FindWithTag("TopLeftBound");
        bottomRight = GameObject.FindWithTag("BottomRightBound");
    }

    void Update()
    {
        spawnDelay -= Time.deltaTime;
        if(spawnDelay > 0)
            return;

        int numberToSpawn = firstNumber + secondNumber;
        secondNumber = firstNumber;
        firstNumber = numberToSpawn;

        Vector3 position;
        for(int i = 0; i < numberToSpawn * difficultyMultiplier; i++)
        {
            switch(i % 4)
            {
                case 0:
                    position = new Vector3(Random.Range(topLeft.transform.position.x, bottomRight.transform.position.x), topLeft.transform.position.y, 0);
                    break;
                case 1:
                    position = new Vector3(bottomRight.transform.position.x, Random.Range(topLeft.transform.position.y, bottomRight.transform.position.y), 0);
                    break;
                case 2:
                    position = new Vector3(Random.Range(topLeft.transform.position.x, bottomRight.transform.position.x), bottomRight.transform.position.y, 0);
                    break;
                default:
                    position = new Vector3(topLeft.transform.position.x, Random.Range(topLeft.transform.position.y, bottomRight.transform.position.y), 0);
                    break;
            }
            Instantiate(objectToSpawn, position, new Quaternion(0,0,0,1));
        }

        spawnDelay = maxSpawnDelay;       
    }
}
