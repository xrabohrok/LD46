﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    [PropertyTooltip("Upper Limit to hunger")]
    public float maxHunger = 100;

    [PropertyTooltip("Hunger in units per second (already negative)")]
    public float hungerDegredation = 1;

    [PropertyRange(0, "maxHunger")]
    public float feelHungryThreshold = 50;

    public float foodPermanance = 5;

    public float huntCooldownTime = 2;
    
    private float currentHunger;

    private GameObject currSeenFood;
    private float relaxTimer;
    private float huntCooldown = -1;

    public goals startState = goals.WANDER;

    public enum goals { 
        ZERO,
        WANDER,
        WAIT,
        GRABBED,
        HUNTING,
        DIE
    }

    public enum emotes { NEUTRAL, HUNGRY}
    private emotes currEmote;

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

        currentHunger = maxHunger;
        currEmote = emotes.NEUTRAL;

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
        else if (currGoal == goals.HUNTING)
        {
            HuntAction();
        }
        else if (currGoal == goals.DIE)
        {
            DieAction();
        }

        Debug.Log($"Is on {Enum.GetName(typeof(goals), currGoal)}");

        if (currentHunger < feelHungryThreshold)
        {
            Debug.Log("Now Hungry");
            currEmote = emotes.HUNGRY;
        }
        else if (currentHunger >= feelHungryThreshold)
        {
            currEmote = emotes.NEUTRAL;
        }

        behaviourTime -= Time.deltaTime;
        currentHunger -= hungerDegredation * Time.deltaTime;
        huntCooldown -= Time.deltaTime;

        if (relaxTimer > 0)
        {
            relaxTimer -= Time.deltaTime;
        }
        else
        {
            currSeenFood = null;
        }

        if (currentHunger <= 0)
        {
            nextGoal = goals.DIE;
        }

    }

    private void DieAction()
    {
        //play death anim, for now just wink out.
        Destroy(gameObject);
    }

    private void HuntAction()
    {
        //the creechur is going to ask the food if it hasn't been claimed. If it has, then it will go back to waiting, otherwise it will move for it
        //there is a hunt cooldown to prevent spazzing
        huntCooldown = huntCooldownTime;

        var mine = true;
        if (currSeenFood != null)
        {
            var food = currSeenFood.GetComponent<Food>();
            if (food != null)
            {
                mine = food.claim(gameObject);
            }
            else
            {
                mine = false;
            }
        }

        if (mine && currSeenFood != null)
        {
            var dir = currSeenFood.transform.position - gameObject.transform.position;
            dir = new Vector3(dir.x, dir.y);

            physical.AddForce(dir.normalized * moveForce);
        }
        else
        {
            nextGoal = goals.WAIT;
        }


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
        canGoHungry();

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
        canGoHungry();
    }

    private void canGoHungry()
    {
        if (currSeenFood != null && currEmote == emotes.HUNGRY && huntCooldown <= 0)
        {
            nextGoal = goals.HUNTING;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (currSeenFood == null && col.tag == "food")
        {
            currSeenFood = col.gameObject;
            relaxTimer = foodPermanance;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (currSeenFood == col.gameObject)
        {
            currSeenFood = null;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (currGoal == goals.HUNTING)
        {
            var food = currSeenFood.GetComponent<Food>();
            if (food != null && food.claim(gameObject))
            {
                currentHunger = maxHunger;
                //play eat anim
                Destroy(currSeenFood);
                currSeenFood = null;
                nextGoal = goals.WAIT;
            }
        }
    }
}