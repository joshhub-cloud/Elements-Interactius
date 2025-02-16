using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour, ICollectable
{
    // Start is called before the first frame update

    public void OnCollected()
    {
        GameManager.gameManager.OrbCollected();
        Destroy(gameObject);
    }
    
}
