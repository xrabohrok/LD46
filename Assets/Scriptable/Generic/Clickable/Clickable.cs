using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Clickable : MonoBehaviour
{

    public int priority;
    public delegate void WasClicked(Clickable thing);

    private static ClickMaster masterClicker; 
    private Collider2D attachedCollider;
    private WasClicked wasClickedDownSubscribers;
    private WasClicked wasClickedUpSubscribers;

    private WasClicked mouseUppedSubscriber;

    public void setClickDownCallback(WasClicked thing)
    {
        if (wasClickedDownSubscribers == null)
        {
            wasClickedDownSubscribers = thing;
            return;
        }
        wasClickedDownSubscribers += thing;
    }

    public void setClickReleaseCallback(WasClicked thing)
    {
        if (wasClickedUpSubscribers == null)
        {
            wasClickedUpSubscribers = thing;
            return;
        }
        wasClickedUpSubscribers += thing;
    }    
    
    public void setMouseReleaseSubscriber(WasClicked thing)
    {
        if (mouseUppedSubscriber == null)
        {
            mouseUppedSubscriber = thing;
            return;
        }
        mouseUppedSubscriber += thing;
    }

    public Collider2D AttachedCollider
    {
        get { return attachedCollider; }
    }

    // Use this for initialization
    void Start ()
	{
	    attachedCollider = GetComponent<Collider2D>();
        if(masterClicker == null)
        {
            masterClicker = FindObjectOfType<ClickMaster>();
        }

	    masterClicker.register(this);
	}

    void OnDestroy()
    {
        masterClicker.deRegister(this);
    }

    public void ReportMouseDown()
    {

        wasClickedDownSubscribers?.Invoke(this);
    }

    //This requires it was clicked first, slightly different from "release"
    public void ReportMouseUp()
    {

        wasClickedUpSubscribers?.Invoke(this);
    }

    public void ReportMouseRelease()
    {
        mouseUppedSubscriber?.Invoke(this);
    }
}
