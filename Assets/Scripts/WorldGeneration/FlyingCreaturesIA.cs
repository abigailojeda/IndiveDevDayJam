using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingCreaturesIA : MonoBehaviour
{
    Transform startTransform;
    Transform endTransform;
    public float speed = 3f;

    public void MoveObjectBetweenTransforms(Transform start, Transform end)
    {
        startTransform = start;
        endTransform = end;
        float distance = Vector3.Distance(start.position, end.position);
        float duration = distance / speed;
        // Get a reference to the object's transform
        Transform objectTransform = transform;
        objectTransform.DOMove(end.position, duration).SetLoops(-1, LoopType.Yoyo);

        /* // Calculate the path for the tween
        Vector3[] path = new Vector3[] { startTransform.position, endTransform.position, startTransform.position };

        // Create a DOTween sequence for the back and forth movement
        Sequence sequence = DOTween.Sequence();

        // Add movement to the sequence
        sequence.Append(objectTransform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear));

        // Play the sequence
        sequence.SetLoops(-1); // Loops infinitely (back and forth) */
    }
}
