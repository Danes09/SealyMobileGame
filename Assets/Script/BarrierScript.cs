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
        if (other.gameObject.tag == TagManager.DESTRUCTIBLE_TAG || other.gameObject.tag == TagManager.ITEM_TAG)
        {
            Destroy(other.gameObject);
        }
        

    }
}
    /*
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (player_Died) { return; } //return in a void function basically exits the function without executing the rest of the code

        if (target.tag == TagManager.EXTRA_PUSH_TAG)
        {
            if (!initial_Push)
            {
                initial_Push = true;

                myBody.velocity = new Vector2(myBody.velocity.x, 18f);
                target.gameObject.SetActive(false);

                SoundManager.instance.jumpSoundFX();


                return; //This return exits from OnTriggerEnter2D because of the initial push.  
            } //initial push


        }
        
*/


