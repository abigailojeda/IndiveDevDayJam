using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    [Header("Menu Animations")]
    [SerializeField] private Animator skyAnimator;
    [SerializeField] private Animator moonAnimator;
    [SerializeField] private Animator sunAnimator;
    [SerializeField] private Animator albumButtonAnimator;
    [SerializeField] private Animator logoAnimator;
    void Start()
    {
        albumButtonAnimator.Play("AlbumButton");
        logoAnimator.Play("Logo");
    }

    public void night()
    {
        skyAnimator.Play("sky");
        moonAnimator.Play("Moon");
        sunAnimator.Play("Sun");
        
    }

    
}
