using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startMoney = 5;
    private int money;

    public int Money => money;

    public delegate void moneyChange(int newAmount);

    private moneyChange moneySubscribers;

    // Start is called before the first frame update
    void Start()
    {
        money = startMoney;
    }

    public void giveMoney(int monies)
    {
        money += monies;
        Debug.Log($"Money is : {monies}");
    }

    public bool trySpendMoney(int monies)
    {
        if (money < monies)
        {
            return false;
        }

        money -= monies;
        moneySubscribers?.Invoke(money);
        return true;
    }

    public void registerMoneyChange(moneyChange newThingy)
    {
        if (moneySubscribers == null)
        {
            moneySubscribers = newThingy;
            return;
        }
        moneySubscribers += newThingy;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
