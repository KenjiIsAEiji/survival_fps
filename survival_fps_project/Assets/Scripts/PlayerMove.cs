using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public Vector2 moveVec2 { get; set; }
    public Vector2 lookVec2 { get; set; }
    public bool jump { get; set; }
    public bool crouch { get; set; }

    [Header("- player move setting and component-")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float targetVelocity = 10;
    private float defaultDrag;
    private float defaultPlayerHeight;
    [SerializeField] LayerMask rayLayer;
    public bool isGrounded;
    [SerializeField] float groundRange = 1.1f;
    [SerializeField] float jumpForce = 1.0f;
    
    [Header("- camera Aiming -")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float sensitivity = 10f;
    private float yaw, pitch;

    // Start is called before the first frame update
    void Start()
    {
        defaultDrag = rb.drag;
        defaultPlayerHeight = playerCollider.height;
    }

    void FixedUpdate()
    {
        if(isGrounded){
            rb.drag = defaultDrag;
            Vector3 v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity;
            rb.AddRelativeForce(v * rb.mass * rb.drag / (1f - rb.drag * Time.fixedDeltaTime));

            if(jump) rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }else{
            rb.drag = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        yaw += lookVec2.x * sensitivity * Time.deltaTime;
        pitch -= lookVec2.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80, 60);

        camTransform.localEulerAngles = new Vector3(pitch, 0, 0);
        this.transform.eulerAngles = new Vector3(0, yaw, 0);

        isGrounded = Physics.Raycast(transform.position, Vector3.down,groundRange,rayLayer);

        if(crouch){
            playerCollider.height = defaultPlayerHeight / 2f;
        }else{
            playerCollider.height = defaultPlayerHeight;
        }
    }
}
