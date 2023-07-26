using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleThought : MonoBehaviour
{
    public BaitType type;

    public GameObject hamburguer, smartphone, money;
    // Start is called before the first frame update
    void Start()
    {
        switch (this.type)
        {
            case BaitType.HAMBURGER:
                this.hamburguer.SetActive(true);
                this.smartphone.SetActive(false);
                this.money.SetActive(false);
                break;
            case BaitType.PHONE:
                this.hamburguer.SetActive(false);
                this.smartphone.SetActive(true);
                this.money.SetActive(false);
                break;
            case BaitType.MONEY:
                this.hamburguer.SetActive(false);
                this.smartphone.SetActive(false);
                this.money.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
