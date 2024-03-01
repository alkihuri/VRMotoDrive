using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFader : MonoSinglethon<UIFader>
{
    [SerializeField] Image _fadeImage;
    [SerializeField] Canvas _canvas;
    private void Start()
    {
        FadeIn();
    }

    private void FadeIn()
    {
        if (!_canvas.gameObject.activeInHierarchy)
        {
            _canvas.gameObject.SetActive(true);
        }
        _fadeImage.DOFade(1, 0);
        _fadeImage.DOFade(0, 1).OnComplete(() => _canvas.gameObject.SetActive(false));
    }

    public void FadeOut(Action action)
    {
        if (!_canvas.gameObject.activeInHierarchy)
        {
            _canvas.gameObject.SetActive(true);
        }
        _fadeImage.DOFade(0, 0);
        _fadeImage.DOFade(1, 1).OnComplete(() => action.Invoke());
    }
}
