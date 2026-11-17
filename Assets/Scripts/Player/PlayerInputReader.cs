using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputReader : MonoBehaviour
{
    //入力判定や入力の値だけを渡す
    
    public Vector2 MoveInput { get; private set;}
    //入力が有効かどうか
    public bool InputEnabled { get; private set;} = true;

    public bool attackPressed { get; private set;} = false;
    public bool doggePressed { get; private set;} = false;
    public bool parryPressed { get; private set;} = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        if(!InputEnabled)
        {
            MoveInput = Vector2.zero;
            return;
        }

        MoveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input:{MoveInput}");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(InputEnabled && context.performed)
        {
            attackPressed = true;
        }
    }
    public bool ConsumeAttackPressed()
    {
        //イベントが呼ばれたら入力判定を消す
        bool value = attackPressed;
        attackPressed = false;

        return value;
    }

    //プレイヤー操作は無効にするが入力を受け付ける場合はあるため自前で無効にする方法を持たせる
    public void SetInputEnabled(bool enabled)
    {
        InputEnabled = enabled;

        //有効なら無効処理をしない
        if(enabled)
        {
            return;
        }

        //入力を全てなかったことにする
        MoveInput = Vector2.zero;
        attackPressed = false;
    }

}
