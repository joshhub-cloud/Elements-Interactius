using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    // Start is called before the first frame update
    public void OnCollected()
    {
        GameManager.gameManager.CoinCollected(1);
        Destroy(gameObject);
    }
    
}
