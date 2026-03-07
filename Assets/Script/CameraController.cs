using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    public float PosY = 1f;
    public float LookDownOffset = -2f;

    public float SmoothTime = 0.15f;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (Target == null) return;

        float yOffset = PosY;

        // กด S → มองลง
        if (Input.GetKey(KeyCode.S))
        {
            yOffset = PosY + LookDownOffset;
        }

        Vector3 targetPos = new Vector3(
            Target.position.x,
            Target.position.y + yOffset,
            -10f
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            SmoothTime
        );
    }
}