﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public Vector2 moveVec2 { get; set; }
    public Vector2 lookVec2 { get; set; }
    public bool jump { get; set; }
    public bool crouch { get; set; }
    public bool sprint { get; set; }

    [Header("- player walk settings and component-")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float targetVelocity = 10;
    [SerializeField] private float sprintAmplifi = 1.5f;
    private float defaultDrag;

    [Header("- player jump and ground check settings -")]
    [SerializeField] LayerMask checkLayer;
    private float defaultPlayerHeight;
    public bool isGrounded;
    [SerializeField] float checkHeightOffset = 0;
    [SerializeField] float jumpForce = 1.0f;
    private bool jumpFlag = false;
    private bool crouchInputBuf, crouching;
    
    [Header("- camera Aiming -")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float camHeightOffcet = 0.2f;
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
            Vector3 v;
            if(sprint){
                v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity * sprintAmplifi;
            }else{
                v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity;
            }
            rb.AddRelativeForce(v * rb.mass * rb.drag / (1f - rb.drag * Time.fixedDeltaTime));

            if(jump){
                if(!jumpFlag){
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    crouching = false;
                }
                jumpFlag = true;
            }else{
                jumpFlag = false;
            }
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
        
        // capsuleCollider Center World height (y axis)
        float capsuleCenter = transform.position.y + playerCollider.center.y;

        camTransform.transform.position = new Vector3(
            camTransform.transform.position.x,
            capsuleCenter + (playerCollider.height / 2f) + camHeightOffcet,
            camTransform.transform.position.z
        );

        float checkHeight = capsuleCenter - (playerCollider.height / 2f) + checkHeightOffset;
        isGrounded = Physics.CheckSphere(
            new Vector3(transform.position.x, checkHeight, transform.position.z),
            playerCollider.radius,
            checkLayer
        );

        if(crouch){
            if(!crouchInputBuf) crouching = !crouching;
            crouchInputBuf = true;
        }else{
            crouchInputBuf = false;
        }
        playerCollider.height = crouching ? defaultPlayerHeight / 2f : defaultPlayerHeight;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float height = transform.position.y - (playerCollider.height / 2f) + playerCollider.center.y + checkHeightOffset;
        Gizmos.DrawWireSphere(
            new Vector3(transform.position.x, height, transform.position.z),
            playerCollider.radius
        );
    }
}
