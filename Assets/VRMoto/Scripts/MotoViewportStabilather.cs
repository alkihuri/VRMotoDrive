using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MotoViewportStabilather : MonoBehaviour
{
    private readonly float TICK = 0.4f;
    [SerializeField] Vector3 _lastTickAngle;
    [SerializeField] private Vector3 _delta;
    [SerializeField] Vector3 _currentickAngle;
    [SerializeField] Transform _viewPortAnchor;

    public bool IsStabilazing { get; private set; }

    private void Awake()
    {
    }

    private void Update()
    {
        _currentickAngle = _viewPortAnchor.eulerAngles;
    }

    private void LateUpdate()
    {

        if (IsStabilazing)
            return;

        _lastTickAngle = _viewPortAnchor.eulerAngles;
        _delta = _currentickAngle - _lastTickAngle;

        var stabilazedY = (_viewPortAnchor.eulerAngles + _delta).y;

        var newAnlge = new Vector3

            (
                transform.eulerAngles.x,
                stabilazedY,
                transform.eulerAngles.z
            );


        transform.DORotate(newAnlge, TICK)
            .SetEase(Ease.Linear)
                .OnStart(() => IsStabilazing = true)
                    .OnComplete(() => IsStabilazing = false);
    }

}


/*
 IEnumerator Tick()
    {
        while (gameObject.activeInHierarchy)
        {
            // middle point betwean lsat and current postion 
            Vector3 delta = (_currentickPosition - _lastTickPosition);
            transform.DOMove(transform.position - delta, 0).SetEase(Ease.Linear)

                .OnStart(() => IsMove = true)
                .OnComplete(() => IsMove = false)
                ;


            _currentickPosition = _viewPortAnchor.position;

            yield return new WaitWhile(() => IsMove);
            yield return new WaitForSeconds(TICK);
            _lastTickPosition = _viewPortAnchor.position;
        }
    }
*/