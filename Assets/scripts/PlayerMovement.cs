using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public float verticalSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Get input from A and D keys
        float moveZ = Input.GetAxis("Vertical"); // Get input from W and S keys
        float moveY = 0; // Initialize Y movement to 0

        if (Input.GetKey(KeyCode.UpArrow)) // Get input from Up arrow key
            moveY = 1;

        if (Input.GetKey(KeyCode.DownArrow)) // Get input from Down arrow key
            moveY = -1;

        // Apply the movement
        Vector3 movement = new Vector3(moveX, moveY, moveZ) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X");

        // Apply rotation based on mouse movement
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime * rotateSpeed);
    }
}
