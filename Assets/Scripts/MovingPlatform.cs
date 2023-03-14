using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float platformSpeed = 1f;
    private int index = 0;
    public bool movingPlatform = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the initial position of the platform to the position of the first point
        transform.position = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the distance between the platform and the current point is near 0
        if (Vector2.Distance(transform.position, points[index].position) < 0.01f)
        {
            index++; // increase index (points index to the next point)
            if(index == points.Length) // check if the new point exceeds the bounds of the array 
            {
                index = 0; // if so, reset index (index now points to the first point)
            }
        }

        //moving the platform towards the point indicated by the index variable by a speed amount
        transform.position = Vector2.MoveTowards(transform.position, points[index].position, platformSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
