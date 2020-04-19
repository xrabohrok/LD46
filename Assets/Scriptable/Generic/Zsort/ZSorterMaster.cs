using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSorterMaster : MonoBehaviour
{
    public int maxSortOrders = 10;

    public Transform topBound;
    public Transform bottomBound;

    public bool lerpScale;
    public float backScaleFactor = 1;
    public float frontScaleFactor = 1;

    private List<ZSortSlave> allSlaves;

	// Use this for initialization
	void Start () {
	    if (allSlaves == null)
	    {
	        allSlaves = new List<ZSortSlave>();
	    }

	    if (topBound == null)
	    {
	        topBound = this.transform;
	    }

	    if (bottomBound == null)
	    {
	        bottomBound = this.transform;
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    foreach (var slave in allSlaves)
	    {
	        slave.SetZDepth(bottomBound.position.y, topBound.position.y, maxSortOrders);
	    }

	    if (lerpScale)
	    {
	        var top = topBound.position.y;
	        var bot = bottomBound.position.y;

	        foreach (var slave in allSlaves)
	        {
	            if (slave.enabled)
	            {
	                slave.jiggerScale(top, bot, backScaleFactor, frontScaleFactor);
	            }
	        }
	    }
	}

    public void pledgeFealty(ZSortSlave zSortSlave)
    {
        if(allSlaves == null) { allSlaves = new List<ZSortSlave>();}

        allSlaves.Add(zSortSlave);
    }

    public void registerDeath(ZSortSlave zSortSlave)
    {
        allSlaves.Remove(zSortSlave);
    }
}
