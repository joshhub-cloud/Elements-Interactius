using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI; // Reference to the Shop UI GameObject

    private void Start()
    {
        // Ensure the Shop UI is invisible at the start
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            // Toggle the Shop UI visibility
            if (shopUI != null)
            {
                shopUI.SetActive(true);
            }
        }
    }
}

