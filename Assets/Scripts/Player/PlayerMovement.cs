using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField, Min(0f)]
    private float gravity = 20f;

    private CharacterController characterController;
    private PlayerController player;

    private Vector2 moveInput;
    private float verticalVelocity;
    private bool movementEnabled = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(!movementEnabled)
        {
            return;
        }

        Vector3 worldDir = ToCameraRelative(moveInput);
        //本実装時はステータスから取ってくる
        float moveSpeed = player.Stats.MoveSpeed;

        //重力を与える
        if(characterController.isGrounded && verticalVelocity < 0f)
        {
            //地面に押し付けるための重力
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVelocity = worldDir * moveSpeed;
        moveVelocity.y = verticalVelocity;

        characterController.Move(
            moveVelocity * Time.deltaTime
        );

        //キャラクターに進行方向を向かせる
        if(worldDir.sqrMagnitude > 0.001f)
        {
            transform.forward = worldDir;
        }
    }
     //入力された情報を移動用に変換。NormalStateの入口
        public void SetMoveInput(Vector2 input)
    {
        moveInput = Vector2.ClampMagnitude(input, 1f);   
    }

    public Vector3 ToCameraRelative(Vector2 input)
    {
        Vector3 forward =
            cameraTransform.forward;
        Vector3 right =
            cameraTransform.right;

        //y軸の傾きは無視する
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return(
            forward * input.y
            + right * input.x
        ).normalized;
    }

    public void Stop()
    {
        moveInput = Vector2.zero;
    }

    public void SetEnabled(bool enabled)
{
    movementEnabled = enabled;

    if (!enabled)
    {
        Stop();
    }
}
}
