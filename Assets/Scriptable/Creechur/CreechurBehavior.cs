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

    public enum goals { ZERO, WANDER, WAIT }

    private float behaviourTime;
    private goals currGoal;
    private goals lastGoal;
    private goals nextGoal;
    private Vector2 moveDir;

    private Rigidbody2D physical;


    // Start is called before the first frame update
    void Start()
    {
        nextGoal = goals.WANDER;
        currGoal = goals.ZERO;

        physical = this.GetComponent<Rigidbody2D>();
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
            Debug.Log("Is Wandering");
            WanderAction();
        }
        else if (currGoal == goals.WAIT)
        {
            Debug.Log("Is Waiting");

            WaitAction();
        }

        behaviourTime -= Time.deltaTime;

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
