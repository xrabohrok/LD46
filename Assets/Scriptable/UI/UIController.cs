using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int critterCost = 5;

    public Button buyButton;
    public Button foodButton;

    private SpreadSpawner spawner;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        buyButton.onClick.AddListener(onBuyCritter);

        spawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<SpreadSpawner>();
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
