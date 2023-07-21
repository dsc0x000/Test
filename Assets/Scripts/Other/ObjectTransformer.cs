using System.Collections;
using UnityEngine;

public class ObjectTransformer : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationTime;

    public void PerformMovement(Vector3 targetPosition) => StartCoroutine(MoveToRoutine(targetPosition));
    public void PerformRotation(Quaternion targetRotation) => StartCoroutine(RotateRoutine(targetRotation));

    private IEnumerator MoveToRoutine(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < animationTime)
        {
            float process = animationCurve.Evaluate(elapsedTime / animationTime);
            transform.position = Vector3.Lerp(startPosition, targetPosition, process);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator RotateRoutine(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < animationTime)
        {
            float process = animationCurve.Evaluate(elapsedTime / animationTime);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, process);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
