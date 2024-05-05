using UnityEngine;

[ExecuteInEditMode]
public class SnapPosition : MonoBehaviour
{
    [SerializeField] private Vector3 snap = Vector3.one;

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Snapping();
        }
    }

    private void Snapping()
    {
        if (snap.x != 0)
        {
            float x = Mathf.Round(transform.localPosition.x / snap.x) * snap.x;
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        if (snap.y != 0)
        {
            float y = Mathf.Round(transform.localPosition.y / snap.y) * snap.y;
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        if (snap.z != 0)
        {
            float z = Mathf.Round(transform.localPosition.z / snap.z) * snap.z;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }
    }
}
