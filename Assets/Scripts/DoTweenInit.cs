using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenInit : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init();
    }
}
