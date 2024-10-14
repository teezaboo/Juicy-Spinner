using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target; // ออปเจคที่ต้องการติดตาม

    void Update()
    {
        if (target != null)
        {
            // ติดตามตำแหน่ง
            transform.position = target.position;

            // ติดตามการหมุน
            transform.rotation = target.rotation;
        }
    }
}
