using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int critterCost = 5;

    public Button buyButton;
    public Button foodButton;

    public TextMeshProUGUI moneyStuff;

    private SpreadSpawner spawner;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        buyButton.onClick.AddListener(onBuyCritter);

        spawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<SpreadSpawner>();

        player.registerMoneyChange(newMoneyAmount);
        newMoneyAmount(player.startMoney);
    }

    private void newMoneyAmount(int newAmount)
    {
        if (moneyStuff != null)
        {
            moneyStuff.text = $"${newAmount}";
        }
    }

    private void onBuyCritter()
    {
        if (player.trySpendMoney(critterCost))
        {
            spawner.spawnItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
