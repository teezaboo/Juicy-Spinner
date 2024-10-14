using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player != null)
        {
            // Find the desired position for the camera to follow
            Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            // Use SmoothDamp to smoothly interpolate the camera position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);

            // Optionally, you can set a fixed rotation for the camera in a 2D scenario
            transform.rotation = Quaternion.identity;
        }
    }
}
