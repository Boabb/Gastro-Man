using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraController : MonoBehaviour
{
    // Declare a Player variable called "player"
    Player player;

    void Start()
    {
        // Find and assign the Player object to the "player" variable
        player = FindObjectOfType<Player>();
    }

    void LateUpdate()
    {
        // Update the position of the camera to match the player's position, but maintain the same z position as the camera
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}