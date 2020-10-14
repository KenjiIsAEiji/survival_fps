using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public Vector2 moveVec2 { get; set; }
    public Vector2 lookVec2 { get; set; }

    [Header("- player move setting and component-")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float targetVelocity = 10;
    
    [Header("- camera Aiming -")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float sensitivity = 10f;
    private float yaw, pitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity;
        rb.AddRelativeForce(v * rb.mass * rb.drag / (1f - rb.drag * Time.fixedDeltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        yaw += lookVec2.x * sensitivity * Time.deltaTime;
        pitch -= lookVec2.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80, 60);

        camTransform.localEulerAngles = new Vector3(pitch, 0, 0);
        this.transform.eulerAngles = new Vector3(0, yaw, 0);

        Debug.Log("move: "+ moveVec2 + " look: " + lookVec2);
    }
}
