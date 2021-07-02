using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallerTweener : MonoBehaviour
{
    public GameObject objectToAnimate;

    public enum UIAnimationTypes
    {
        Fade,
        Move,
        Scale,
        RotateAround
    }

    [Header("Tween Val")]
    public UIAnimationTypes animationType;
    public LeanTweenType easeType;
    public float duration;
    public float delay;
    [Tooltip("(0 - n), -1 means infinite")]
    public int repeatVal;

    [Header("Specific Val")]
    public bool startPositionOffset;
    public Vector3 posFrom;
    public Vector3 toFrom;
    public bool invertRotation;

    public bool showOnEnable;

    private LTDescr _tweenObject;

    public void OnEnable()
    {
        if (showOnEnable)
        {
            HandleTween();
        }
    }

    public void HandleTween()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationType)
        {
            case UIAnimationTypes.Fade:
                Fade();
                break;
            case UIAnimationTypes.Move:
                Move();
                break;
            case UIAnimationTypes.Scale:
                Scale();
                break;
            case UIAnimationTypes.RotateAround:
                RotateAround();
                break;
        }

        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);
    }

    public void Fade()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), toFrom.x, duration).setRepeat(repeatVal);

    }
    public void Fade(int target, float duration)
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), target, duration).setRepeat(repeatVal);

    }

    public void Move()
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<Transform>().position = posFrom;
        }
        else posFrom = objectToAnimate.GetComponent<Transform>().position;

        _tweenObject = LeanTween.move(objectToAnimate, toFrom, duration);
    }

    public void Move(GameObject Obj, Vector3 Asal, Vector3 Target, float duration)
    {
        _tweenObject = LeanTween.move(Obj.GetComponent<RectTransform>(), Target, duration).setRepeat(repeatVal);
    }

    public void Scale()
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().localScale = posFrom;
        }

        _tweenObject = LeanTween.scale(objectToAnimate, toFrom, duration).setRepeat(repeatVal);
    }

    public void RotateAround()
    {
        if (!invertRotation)
            _tweenObject = LeanTween.rotateAroundLocal(objectToAnimate, Vector3.forward, 360f, 12f).setRepeat(repeatVal);
        else
            _tweenObject = LeanTween.rotateAroundLocal(objectToAnimate, Vector3.forward, -360f, 12f).setRepeat(repeatVal);
    }
}
