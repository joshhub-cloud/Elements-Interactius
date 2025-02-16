using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_n_Rotate : MonoBehaviour
{ 
  public float scaleTime = 2f;
private float scale = 1f;
public float rotateAmount = 2f;
public float scaleAmount = 0.005f;
private void Start()
{
StartCoroutine(ScaleObject());
}

private void Update()
{
     
transform. Rotate(0, rotateAmount, 0);
transform. localScale += new Vector3(1, 1f, 1f) * scale * scaleAmount;
    
}
private IEnumerator ScaleObject()
{
yield return new WaitForSeconds(scaleTime);
scale = -scale;
yield return ScaleObject();
}

}
