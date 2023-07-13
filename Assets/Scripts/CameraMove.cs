using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [Space]
    [SerializeField]
    private float speed = 10f;

    private void Update()
    {
        Vector3 delta = Vector3.zero;
        if(Input.GetKey(KeyCode.W))
        {
            delta += Vector3.forward;
        }
        if(Input.GetKey(KeyCode.S))
        {
            delta -= Vector3.forward;
        }
        if(Input.GetKey(KeyCode.A))
        {
            delta += Vector3.left;
        }
        if(Input.GetKey(KeyCode.D))
        {
            delta -= Vector3.left;
        }

        if(!delta.Equals(Vector3.zero))
        {
            cameraTransform.position += Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f) * delta * speed * Time.deltaTime;
        }
    }
}
