using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingCreaturesIA : MonoBehaviour
{
    Transform startTransform;
    Transform endTransform;
    public float speed = 3f;
    SpriteRenderer sprite;
    Transform front;
    Transform back;
    private Tweener tweener;

    /* private void Start() {
        previousPosition = transform.position;
        sprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        front = transform.GetChild(0).GetChild(1);
        back = transform.GetChild(0).GetChild(2);
    } */

    public void MoveObjectBetweenTransforms(Transform start, Transform end)
    {
        sprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        front = transform.Find("front");
        back = transform.Find("back");

        if (transform != null)
        {
            startTransform = start;
            endTransform = end;
            float distance = Vector3.Distance(start.position, end.position);
            float duration = distance / speed;
            // Get a reference to the object's transform
            startRotation();
            Transform objectTransform = transform;
            if (objectTransform != null) {
                tweener = objectTransform.DOMove(end.position, duration).SetLoops(-1, LoopType.Yoyo).OnStepComplete(() =>
                {
                    rotateSprite();
                });;
            }
        }
    }
    void rotateSprite()
    {
        if (sprite != null)
        {
            sprite.flipX = !sprite.flipX;
        }
    }
    void startRotation()
    {
        float frontDistance = Vector3.Distance(front.position, endTransform.position);
        float backDistance = Vector3.Distance(back.position, endTransform.position);

        if (frontDistance > backDistance)
        {
            rotateSprite();
        }
    }

    private void OnDestroy()
    {
        // Check if the tween is not null and if it's playing
        if (tweener != null && tweener.IsActive())
        {
            // Kill the tween when the object is destroyed
            tweener.Kill();
        }
    }

    private void OnDisable()
    {
        if (tweener != null && tweener.IsActive())
        {
            tweener.Kill();
        }
    }
}
