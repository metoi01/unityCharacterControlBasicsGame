using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }
    void Update()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = newPosition;
    }
}
