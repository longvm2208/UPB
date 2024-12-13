using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum Mode
    {
        LookAt,
        LookAtInverted,
    }

    [SerializeField] private Transform myTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Mode mode = Mode.LookAt;

    private Vector3 direction;

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                myTransform.LookAt(cameraTransform);
                break;
            case Mode.LookAtInverted:
                direction = myTransform.position - cameraTransform.position;
                myTransform.LookAt(myTransform.position + direction);
                break;
        }
    }
}
