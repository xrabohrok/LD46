using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CreechurBehavior : MonoBehaviour
{

    [PropertyTooltip("Time Spent Wandering")]
    public float wanderRange;

    [PropertyTooltip("time spent waiting")]
    public float waitTimerRange;

    [PropertyRange(0, 1), PropertyTooltip("How Much the other values will fluctuate randomly")]
    public float randomVariance;

    [PropertyTooltip("How fast they will move as a force applied")]
    public float moveForce;

    public goals startState = goals.WANDER;

    public enum goals { ZERO, WANDER, WAIT,
        GRABBED
    }

    private float behaviourTime;
    private goals currGoal;
    private goals lastGoal;
    private goals nextGoal;
    private Vector2 moveDir;

    private Rigidbody2D physical;
    private Clickable clicker;

    private static MouseBehaviour mousey;



    // Start is called before the first frame update
    void Start()
    {
        nextGoal = startState;
        currGoal = goals.ZERO;

        physical = this.GetComponent<Rigidbody2D>();
        clicker = this.GetComponent<Clickable>();

        if (clicker != null)
        {
            clicker.setClickDownCallback(wasMouseDowned);
            clicker.setClickReleaseCallback(wasMouseUpped);
        }

        if (mousey == null)
        {
            mousey = FindObjectOfType<MouseBehaviour>();
        }

    }

    private void wasMouseDowned(Clickable thing)
    {
        Debug.Log("Attached monster");
        nextGoal = goals.GRABBED;
        physical.freezeRotation = false;
        physical.gravityScale = 1;

        mousey.Attach(physical);
    }

    private void wasMouseUpped(Clickable thing)
    {
        Debug.Log("Detached Monster");

        nextGoal = goals.WAIT;
        physical.freezeRotation = true;
        physical.transform.rotation = Quaternion.identity;
        physical.gravityScale = 0;
    }

    private float WeightedRandomRange(float percentWeightThatIsRandom, float maxValue)
    {
        return (1 - percentWeightThatIsRandom) * maxValue + percentWeightThatIsRandom * maxValue * Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        lastGoal = currGoal;
        currGoal = nextGoal;

        if (currGoal == goals.WANDER)
        {
            WanderAction();
        }
        else if (currGoal == goals.WAIT)
        {
            WaitAction();
        }
        else if (currGoal == goals.GRABBED)
        {
            GrabbedAction();
        }

        Debug.Log($"Is on {Enum.GetName(typeof(goals), currGoal)}");

        behaviourTime -= Time.deltaTime;

    }

    private void GrabbedAction()
    {
        //basically do nothing
        //cannot escape this event on their own

        //however it is unlikely that the mouse will be over the creechur when pulled up, so we have to watch for that ourselves
        if (Input.GetMouseButtonUp(0))
        {
            wasMouseUpped(clicker);
        }
    }

    private void WaitAction()
    {
        if (lastGoal != goals.WAIT)
        {
            behaviourTime = WeightedRandomRange(randomVariance, waitTimerRange);
            //play wait anim
        }
        //nothing to do

        nextGoal = behaviourTime < 0 ? goals.WANDER : goals.WAIT;

    }

    private void WanderAction()
    {
        if (lastGoal != goals.WANDER)
        {
            behaviourTime = WeightedRandomRange(randomVariance, wanderRange);
            moveDir = Random.insideUnitCircle;
            //play walk anim
        }
        else
        {
            physical.AddForce(moveForce * moveDir);
            //set play speed
        }

        nextGoal = behaviourTime < 0 ? goals.WAIT : goals.WANDER;
    }
}
