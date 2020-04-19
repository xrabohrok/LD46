using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D), typeof(Rigidbody2D))]
public class MouseBehaviour : MonoBehaviour
{
    public Vector2 snapLoc = Vector2.down;

    private HingeJoint2D spring;
    private Rigidbody2D rigid;

    private bool attached = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spring = GetComponent<HingeJoint2D>();
    }

    public void Attach(Rigidbody2D obj)
    {
        Debug.Log("ATTACHED");
        this.enabled = true;
        this.attached = true;
        stickToMouse();
        spring.connectedBody = obj;
        obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y) + new Vector3(snapLoc.x, snapLoc.y);

    }

    private void Detach()
    {
        Debug.Log("REMOVED");

        spring.connectedBody = null;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attached)
        {
            stickToMouse();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Detach();
        }
    }

    private void stickToMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(mousePos.x, mousePos.y);
    }
}
