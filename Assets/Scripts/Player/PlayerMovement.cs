using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    //仮置き。本実装時はステータスから読み込ませる。
    [SerializeField]
    private float moveSpeed = 5f;

    private CharacterController characterController;
    private PlayerInputReader inputReader;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        Vector2 input = inputReader.MoveInput;

        Vector3 moveDir = new Vector3(
            input.x,
            0f,
            input.y
        );

        //1以上のベクトルにならないように制限
        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        characterController.Move(
            moveDir * moveSpeed * Time.deltaTime
        );
    }
}