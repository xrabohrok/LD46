using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSortSlave : MonoBehaviour
{
    public float height = 0;

    private SpriteRenderer render;

    private static ZSorterMaster master;

    private Vector3 defaultScale;

	// Use this for initialization
	void Start () {

	    if (master == null)
	    {
	        master = GameObject.FindObjectOfType<ZSorterMaster>();
	    }

	    master.pledgeFealty(this);

	    render = this.GetComponent<SpriteRenderer>();

	    defaultScale = this.transform.localScale;
	}

    public void SetZDepth(float minY, float maxY, int maxSortOrder)
    {
        if (render != null)
        {
            render.sortingOrder = Mathf.FloorToInt(( 1 -(( this.transform.position.y - height - minY) / ( maxY - minY))) * maxSortOrder);
        }
    }

    public void OnDestroy()
    {
        master.registerDeath(this);
    }

    public void jiggerScale(float topBound, float bottomBound, float backScaleFactor, float frontScaleFactor)
    {
        var percent = (this.transform.position.y - topBound) / (bottomBound - topBound);
        this.transform.localScale = defaultScale * Mathf.Lerp(backScaleFactor, frontScaleFactor, percent);
    }
}
