using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Button(ButtonStyle.FoldoutButton)]
    public void ShakePosition(float duration, float magnitude)
    {
        StartCoroutine(ShakePositionRoutine(duration, magnitude));

        IEnumerator ShakePositionRoutine(float duration, float magnitude)
        {
            Vector3 originalPosition = cameraTransform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                Vector3 offset = magnitude * new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                cameraTransform.localPosition = originalPosition + offset;
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTransform.localPosition = originalPosition;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void ShakeRotation(float duration, float magnitude)
    {
        StartCoroutine(ShakeRotationRoutine(duration, magnitude));

        IEnumerator ShakeRotationRoutine(float duration, float magnitude)
        {
            Quaternion originalRotation = cameraTransform.localRotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float z = magnitude * Random.Range(-1f, 1f);
                Quaternion offset = Quaternion.Euler(0f, 0f, z);
                cameraTransform.localRotation = originalRotation * offset;
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTransform.localRotation = originalRotation;
        }
    }
}
