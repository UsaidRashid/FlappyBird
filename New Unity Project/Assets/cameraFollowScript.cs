using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform target; // Reference to the bird's transform

    private Vector3 offset; // Offset between the camera and the bird

    void Start()
    {
        // Calculate the initial offset between the camera and the bird
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void Update()
    {
        // Check if the bird's transform is assigned
        if (target != null)
        {
            // Update the camera's position, only following the bird's x-axis movement
            Vector3 newPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
