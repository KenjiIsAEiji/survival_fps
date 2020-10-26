using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 移動キー(WASD)入力
    /// </summary>
    /// <value></value>
    public Vector2 moveVec2 { get; set; }

    /// <summary>
    /// マウスの移動距離入力
    /// </summary>
    /// <value></value>
    public Vector2 lookVec2 { get; set; }

    /// <summary>
    /// ジャンプキー入力
    /// </summary>
    /// <value></value>
    public bool jump { get; set; }

    /// <summary>
    /// しゃがみキー入力
    /// </summary>
    /// <value></value>
    public bool crouch { get; set; }

    /// <summary>
    /// ダッシュキー入力
    /// </summary>
    /// <value></value>
    public bool sprint { get; set; }

    [Header("- player walk settings and component-")]
    [SerializeField] private Rigidbody rb;  // プレイヤーの移動実行用RigidBody
    [SerializeField] private CapsuleCollider playerCollider;    // プレイヤー体部分の当たり判定
    [SerializeField] private float targetVelocity = 10; // 移動の終端速度(Dragを考慮)
    [SerializeField] private float sprintAmplifi = 1.5f;    // スプリント時の移動速度倍率
    [SerializeField] private float crouchSpeed = 1.0f;  // しゃがみ時や解除時の高さの反映速度
    private float defaultDrag;  // デフォルトの抵抗力

    [Header("- player jump and ground check settings -")]
    [SerializeField] LayerMask checkLayer;  // 接地判定を行うレイヤーマスク
    private float defaultPlayerHeight;  // 当たり判定のデフォルトの高さ
    public bool isGrounded; // 接地判定フラグ
    [SerializeField] float checkHeightOffset = 0;   // 接地判定位置の調整
    [SerializeField] float jumpForce = 1.0f;    // ジャンプ時の上方向の力
    private bool jumpFlag = false;  // ジャンプ状態の一時記録用フラグ
    
    // 前フレームのしゃがみボタンの入力バッファ｜しゃがみ状態フラグ
    private bool crouchInputBuf, crouching;
    
    [Header("- camera Aiming -")]
    [SerializeField] private Transform camTransform;　// カメラのTransform
    [SerializeField] private float camHeightOffcet = 0.2f;  // カメラの高さ調整
    [SerializeField] private float sensitivity = 10f;   // カメラ操作感度(InputManagerに組み込む可能性あり)
    private float yaw, pitch;   // カメラの縦方向の回転およびプレイヤーの横方向の回転

    // Start is called before the first frame update
    void Start()
    {
        // デフォルト値の記録
        defaultDrag = rb.drag;
        defaultPlayerHeight = playerCollider.height;
    }

    void FixedUpdate()
    {
        // プレイヤーの移動処理　----------
        if(isGrounded){
            rb.drag = defaultDrag;  // 地面に接地している場合は抵抗力が働く
            Vector3 v;
            // 前方に移動し、かつスプリント時に、移動速度倍率を反映
            if(sprint && moveVec2.y > 0){
                v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity * sprintAmplifi;
            }else{
                v = new Vector3(moveVec2.x, 0f, moveVec2.y) * targetVelocity;
            }

            // ローカル座標系で移動実行
            rb.AddRelativeForce(v * rb.mass * rb.drag / (1f - rb.drag * Time.fixedDeltaTime));

            // ジャンプ処理(ボタン押し込み時の1フレームのみ実行) -----------
            if(jump){
                if(!jumpFlag){
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    crouching = false; // ジャンプするとしゃがみ状態を解除
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
        // カメラの回転処理 ------------------------------------------
        yaw += lookVec2.x * sensitivity * Time.deltaTime;
        pitch -= lookVec2.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80, 60);    // 縦方向の回転の制限

        camTransform.localEulerAngles = new Vector3(pitch, 0, 0);
        this.transform.eulerAngles = new Vector3(0, yaw, 0);
        
        // capsuleColliderの高さの中心
        float capsuleCenter = transform.position.y + playerCollider.center.y;

        // カメラの高さ位置を変更
        camTransform.transform.position = new Vector3(
            camTransform.transform.position.x,
            capsuleCenter + (playerCollider.height / 2f) + camHeightOffcet,
            camTransform.transform.position.z
        );

        // 接地判定の位置の調整
        float checkHeight = capsuleCenter - (playerCollider.height / 2f) + checkHeightOffset;
        isGrounded = Physics.CheckSphere(
            new Vector3(transform.position.x, checkHeight, transform.position.z),
            playerCollider.radius,
            checkLayer
        );

        // しゃがみ状態の移行および解除 ----------------------------------
        if(crouch){
            if(!crouchInputBuf) crouching = !crouching;
            crouchInputBuf = true;
        }else{
            crouchInputBuf = false;
        }
        
        // しゃがみ状態から当たり判定に反映
        float targetHeight = crouching ? defaultPlayerHeight / 2f : defaultPlayerHeight;
        playerCollider.height = Mathf.Lerp(playerCollider.height,targetHeight,crouchSpeed * Time.deltaTime);
    }

    // 接地判定の可視化 ------------------------------------------------------------------
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
