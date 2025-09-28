using UnityEngine;
using UnityEngine.InputSystem;

public class FreeFlyCameraNewInput : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 10f;
    [SerializeField]
    public float FastMoveSpeed = 50f;
    [SerializeField]
    public float Sensitivity = 2f;

    private float rotationX;
    private float rotationY;

    void Start()
    {        
        Vector3 euler = transform.rotation.eulerAngles;
        rotationX = euler.y;
        rotationY = euler.x;
    }

    void Update()
    {
        var mouse = Mouse.current;
        var keyboard = Keyboard.current;
                
        if (mouse.rightButton.isPressed)
        {
            rotationX += mouse.delta.x.ReadValue() * Sensitivity * Time.deltaTime;
            rotationY -= mouse.delta.y.ReadValue() * Sensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        float speed = keyboard.leftShiftKey.isPressed ? FastMoveSpeed : MoveSpeed;

        Vector3 direction = Vector3.zero;
        if (keyboard.wKey.isPressed)
        {
            direction += Vector3.forward;
        }
        if (keyboard.sKey.isPressed)
        {
            direction += Vector3.back;
        }
        if (keyboard.aKey.isPressed)
        {
            direction += Vector3.left;
        }
        if (keyboard.dKey.isPressed)
        {
            direction += Vector3.right;
        }
        if (keyboard.eKey.isPressed)
        {
            direction += Vector3.up;
        }
        if (keyboard.qKey.isPressed)
        {
            direction += Vector3.down;
        }

        transform.Translate(direction * speed * Time.deltaTime, Space.Self);
    }
}
