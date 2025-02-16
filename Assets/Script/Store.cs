using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Store : MonoBehaviour
{
    public ItemScriptableObject Item1, Item2;
    public Image Image1, Image2;
    public TextMeshProUGUI textItem1, TextItem2;
    public Button Buy1, Buy2;

    // Update is called once per frame
    void OnEnable()
    {
        Image1.sprite = Item1.image;
        Image2.sprite = Item2.image;
        CheckIfCanBuy(Item1, textItem1, Buy1);
        CheckIfCanBuy(Item2, TextItem2, Buy2);
    }
    public void BuyItemBow()
    {
        GameManager.gameManager.ItemCollected(Item1.image, 0);
        GameManager.gameManager.CoinCollected(-Item1.price);
        CheckIfCanBuy(Item1, textItem1, Buy1);
        CheckIfCanBuy(Item2, TextItem2, Buy2);
    }

    public void BuyItemArrow() 
    {
        GameManager.gameManager.ItemCollected(Item2.image, 1);
        GameManager.gameManager.CoinCollected(-Item2.price);
        CheckIfCanBuy(Item1, textItem1, Buy1);
        CheckIfCanBuy(Item2, TextItem2, Buy2);
    }

    private void CheckIfCanBuy(ItemScriptableObject item, TextMeshProUGUI insuCoins, Button buyButton)
    {
        if(GameManager.gameManager.Coins >= item.price) 
        { 
            insuCoins.text = "" + item.price;
            insuCoins.color = Color.yellow;
            buyButton.interactable = true;
        }
        else
        {
            insuCoins.text = "insuficient coins: " + item.price;
            insuCoins.color = Color.red;
            buyButton.interactable = false;
        }
    }    
}
