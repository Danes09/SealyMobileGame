using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    public static bool Move = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        lastclickpos = transform.position;
    }
    private Animator animator;

    public float speed;
    public Vector2 lastclickpos;
    bool moving;
    private void Update()
    {

        if (Move == true)
        {

            if (Input.GetMouseButtonDown(1))
            {

                lastclickpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                moving = true;
            }
            if (moving && (Vector2)transform.position != lastclickpos)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, lastclickpos, step);
                updateanimation();
            }
            else
            {
                moving = false;
            }
        }


    }

    private void updateanimation()
    {
        float distance = Vector2.Distance(transform.position, lastclickpos);
        animator.SetFloat("distance", distance);
        if (distance > 0.01)
        {
            Vector3 direction = transform.position - new Vector3(lastclickpos.x, lastclickpos.y, transform.position.z);
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            animator.SetFloat("angle", angle);
        }

    }
}
