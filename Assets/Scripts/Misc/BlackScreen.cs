using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
﻿using DG.Tweening;
using UnityEngine;
using TMPro;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] GameObject blackBackgroundGO;
	[SerializeField] Image blackBackground;
    [SerializeField] TextMeshProUGUI text;
	Tween fadeTween, textFadeTween;
    private bool alreadyIn = false;

	public void FadeIn(float durationToFade)
    {
        if (alreadyIn || blackBackground == null || text == null) return;

        Fade(1f, durationToFade, () =>
        {
            StopAllCoroutines();
            alreadyIn = true;
        });
    }

    public void FadeOut(float duration)
    {
        if (blackBackground == null || text == null || blackBackgroundGO == null) return;

        Fade(0f, duration, () =>
        {
            alreadyIn = false;
            if (blackBackgroundGO != null){
                blackBackgroundGO.SetActive(false);
            }
            
        });
    }

    public void FadeZero()
    {
        if (blackBackground == null || text == null || blackBackgroundGO == null) return;

        blackBackgroundGO.SetActive(true);
        Fade(0f, 0, () =>
        {
            alreadyIn = false;
        });
    }

    public void SetFadeInAndOut(float fadeDuration)
    {
        if (blackBackgroundGO == null)
        {
            Debug.LogWarning("blackBackgroundGO is not set.");
            return;
        }

        FadeZero();
        FadeIn(fadeDuration);
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (blackBackground == null || text == null)
        {
            Debug.LogWarning("blackBackground or text is not set.");
            return;
        }

        fadeTween?.Kill(false);
        textFadeTween?.Kill(false);

        fadeTween = blackBackground.DOFade(endValue, duration);
        textFadeTween = text.DOFade(endValue, duration);

        fadeTween.onComplete += onEnd;
    }
}