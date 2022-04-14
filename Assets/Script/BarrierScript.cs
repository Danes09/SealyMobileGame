using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == TagManager.DESTRUCTIBLE_TAG || other.gameObject.tag==TagManager.ITEM_TAG)
        {
            Destroy(other.gameObject);
        }



    }



}
