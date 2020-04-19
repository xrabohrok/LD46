using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Make sure the execution order is set to do this before any clicable!
public class ClickMaster : MonoBehaviour {

    private Vector3 mouseLoc;
    private List<Clickable> clickables { get; set;  }
    private List<Clickable> hovered;
    private Clickable currClickable;

    void Start()
    {
        if (clickables == null)
        {
            clickables = new List<Clickable>();
        }
        hovered = new List<Clickable>();
    }
	
	void Update ()
	{
        hovered = new List<Clickable>();
        mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);

	    foreach (var clickable in clickables)
	    {
	        if (clickable.AttachedCollider.OverlapPoint(mouseLoc))
	        {
	            hovered.Add(clickable);
            }
        }

	    if (Input.GetMouseButtonDown(0))
	    {
            // gameObject.GetComponent<MassSoundSubscriber>().playSound(gameObject.transform.position, "Click");
            foreach (var clicked in hovered)
	        {
	            if (currClickable == null || clicked.priority > currClickable.priority)
	            {
	                currClickable = clicked;
	            }
	            clicked.ReportMouseDown();
	        }
	    }

	    if (Input.GetMouseButtonUp(0))
	    {
	        currClickable = null;
	        foreach (var clicked in hovered)
	        {
	            clicked.ReportMouseUp();
	        }

            foreach (var curHover in hovered)
            {
                curHover.ReportMouseRelease();
            }
	    }
    }

    public void register(Clickable clickable)
    {
        if (clickables == null)
        {
            clickables = new List<Clickable>();
        }
        clickables.Add(clickable);
    }

    public void deRegister(Clickable clickable)
    {
        if (clickables == null)
        {
            clickables = new List<Clickable>();
        }
        clickables.Remove(clickable);
    }
}
