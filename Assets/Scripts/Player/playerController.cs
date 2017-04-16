using UnityEngine;
[RequireComponent(typeof(playerMotor))]
public class playerController : MonoBehaviour {

    [SerializeField]
    private float speed = 10f;
    private playerMotor motor;
    [SerializeField]
    private float lookSensitivity=3f;
    Animator anim;
    void Start()
    {
        motor = GetComponent<playerMotor>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Calculate movement velocity as a 3d Vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;
        Vector3 velocity = (movHorizontal + movVertical)
.normalized * speed;
        motor.Move(velocity);
        //animation
        Animating(xMov, yMov);
        //Rotation(turning around)
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f)*lookSensitivity;

        motor.Rotate(rotation);
        //Camera Rotation(turning around)
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 camrotation = new Vector3(xRot, 0f, 0f) * lookSensitivity;

        motor.CameraRotate(camrotation);
    }


    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }
}


