using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clickable))]
public class OutBox : MonoBehaviour
{

    // private MouseBehaviour mouse;
    private Player player;
    private Clickable clicky;

    private List<GameObject> thingsInZone;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        // mouse = FindObjectOfType<MouseBehaviour>();

        clicky = GetComponent<Clickable>();
        clicky.setMouseReleaseSubscriber(onRelease);
        
        thingsInZone = new List<GameObject>();
    }

    void onRelease(Clickable wasReleased)
    {
        //query all the things in the zone, notify them they are disposed
        var toRemove = new List<GameObject>();
        foreach (var thing in thingsInZone)
        {
            var outBoxable = thing.GetComponent<IoutBoxable>();
            if (outBoxable != null)
            {
                if (outBoxable.isSellable())
                {
                    player.giveMoney(outBoxable.sellWorth());
                }
                outBoxable.disposed();
                toRemove.Add(thing);
            }
        }

        foreach (var dead in toRemove)
        {
            thingsInZone.Remove(dead);
        }

        toRemove = null;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (!thingsInZone.Contains(col.gameObject))
        {
            thingsInZone.Add(col.gameObject);
        }
    }    
    
    void OnTriggerExit2D(Collider2D col)
    { 
        thingsInZone.Remove(col.gameObject);
    }
}

interface IoutBoxable
{
    bool isSellable();
    int sellWorth();

    void disposed();

}
