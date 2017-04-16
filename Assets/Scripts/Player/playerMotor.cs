using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMotor : MonoBehaviour {
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 camrotation = Vector3.zero;
    private Rigidbody rb;
    [SerializeField]
    private Camera cam;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _veloctiy)
    {
        velocity = _veloctiy;


    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void CameraRotate(Vector3 _camrotation)
    {
        camrotation = _camrotation;
    }
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    void PerformMovement()
    {
        if (velocity != Vector3.zero) 
        {
            rb.MovePosition(rb.position+velocity*Time.fixedDeltaTime);
        }

    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-camrotation);
        }
        
    }
}
