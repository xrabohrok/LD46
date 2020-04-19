using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadSpawner : MonoBehaviour
{

    public Transform Spawnee;

    // public int maxCount;
    // private int count;
    public float radius;

    // public float secondsPerSpawn = 1;
    // private float timer;

    public List<string> colliderLayers;
    private int layerMask;

    // Use this for initialization
    void Start()
    {
        buildLayerMasks();
    }

    private void buildLayerMasks()
    {
        layerMask = 0;
        foreach (var layer in colliderLayers)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layer);
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
    //
    //     if (Spawnee != null  && (count < maxCount || maxCount == 0))
    //     {
    //         if (timer > secondsPerSpawn)
    //         {
    //             spawnItem();
    //             timer = 0;
    //         }
    //         else
    //         {
    //             timer += Time.deltaTime;
    //         }
    //     }
    // }

    public void spawnItem()
    {
        bool goodSpot = false;
        var spot = Random.insideUnitCircle;
        spot *= radius;
        var spot3d = new Vector3(spot.x, spot.y) + transform.position;

        while (!goodSpot)
        {
            goodSpot = Physics2D.OverlapPoint(spot3d, layerMask) == null;

            if(!goodSpot)
            {
                spot = Random.insideUnitCircle * radius;
                spot3d = new Vector3(spot.x, spot.y) + transform.position;
            }
        }

        var item = GameObject.Instantiate(Spawnee, spot3d, Quaternion.identity).gameObject;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, radius);
    }
}
