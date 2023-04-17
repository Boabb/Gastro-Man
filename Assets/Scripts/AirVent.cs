using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    public GameObject finish;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!finish.activeInHierarchy)
        {
            collision.attachedRigidbody.gravityScale = -2;
        }
        else
        {

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.attachedRigidbody.gravityScale = 1.5f;
    }
}
