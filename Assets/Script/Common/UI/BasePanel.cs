using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationMethod
{
    Ease,
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    Bouncing,
    BouncingR,
    Custom
}

public enum PanelEventType
{
    AnimationOver,
    OnOpened,
    OnClosed,
}

public enum AnimationMode
{
    Normal,
    Custom,
}

public enum AnimationType
{
    Open,
    Close,
}

[System.Serializable]
public class UIAnimationStep
{
    [Range(0f, 1f)]
    public float percentage = 1;
    public float targetAlpha = 1;
    public Vector2 targetScale = Vector2.one;
    public AnimationMethod method = AnimationMethod.Linear;
    public Vector4 points = Vector4.zero;

    public UIAnimationStep(
        float _percentage,
        float _targetAlpha,
        Vector2 _targetScale,
        AnimationMethod _method,
        Vector4 _points
    )
    {
        percentage = _percentage;
        targetAlpha = _targetAlpha;
        targetScale = _targetScale;
        method = _method;
        points = _points;
    }
}

[RequireComponent(typeof(CanvasGroup))]
public class BasePanel : MonoBehaviour
{

    public AnimationMode animationMode = AnimationMode.Normal;
    public float duration = 0.3f;
    [SerializeField] private List<UIAnimationStep> openSteps = new List<UIAnimationStep>();
    [SerializeField] private List<UIAnimationStep> closeSteps = new List<UIAnimationStep>();
    [SerializeField] private GameObject content;

    [NonSerialized] public UnityEvent<BasePanel> OnOpenEnd = new UnityEvent<BasePanel>();
    [NonSerialized] public UnityEvent<BasePanel> OnCloseEnd = new UnityEvent<BasePanel>();

    public bool canAnimation => !startAnimation;
    public bool IsOpening => canvasGroup.alpha != 0;

    private List<UIAnimationStep> useOpenSteps
    {
        get
        {
            switch (animationMode)
            {
                case AnimationMode.Normal:
                    return new List<UIAnimationStep>(){
                        new UIAnimationStep(1,1,Vector2.one, AnimationMethod.Bouncing, Vector4.zero)
                    };
                default:
                    openSteps.Sort((a, b) => a.percentage - b.percentage > 0 ? 1 : -1);
                    return new List<UIAnimationStep>(openSteps);
            }
        }
    }

    private List<UIAnimationStep> useCloseSteps
    {
        get
        {
            switch (animationMode)
            {
                case AnimationMode.Normal:
                    return new List<UIAnimationStep>(){
                        new UIAnimationStep(1,0,Vector2.zero, AnimationMethod.BouncingR, Vector4.zero)
                    };
                default:
                    closeSteps.Sort((a, b) => a.percentage - b.percentage > 0 ? 1 : -1);
                    return new List<UIAnimationStep>(closeSteps);
            }
        }
    }

    private List<UIAnimationStep> useSteps
    {
        get
        {
            return animationType == AnimationType.Close ? useCloseSteps : useOpenSteps;
        }
    }

    private CanvasGroup canvasGroup => GetComponent<CanvasGroup>();
    private Vector2 scale = Vector2.one;
    private Vector2 targetScale = Vector2.one;
    private AnimationType animationType = AnimationType.Open;
    private int stepIdx = 0;
    private float alpha = 1;
    private float targetAlpha = 1;
    private float timer = 0;
    private float previousStepTime => stepIdx > 0 ? useSteps[stepIdx - 1].percentage * duration : 0;
    private float timerGap => stepIdx > 0 ? useSteps[stepIdx].percentage - useSteps[stepIdx - 1].percentage : useSteps[stepIdx].percentage == 0 ? float.MinValue : useSteps[stepIdx].percentage;
    private float progress => useSteps.Count > 0 ?
        (Time.time - timer - previousStepTime) / (duration * timerGap) : (Time.time - timer) / duration;
    private GameObject target => content != null ? content : gameObject;
    private bool startAnimation = false;

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (startAnimation)
        {
            if (useSteps.Count > 0)
            {
                switch (useSteps[stepIdx].method)
                {
                    case AnimationMethod.Ease:
                        target.transform.localScale = BezierCurve.Ease<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.Ease<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.Linear:
                        target.transform.localScale = BezierCurve.Linear<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.Linear<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.EaseIn:
                        target.transform.localScale = BezierCurve.EaseIn<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.EaseIn<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.EaseOut:
                        target.transform.localScale = BezierCurve.EaseOut<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.EaseOut<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.EaseInOut:
                        target.transform.localScale = BezierCurve.EaseInOut<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.EaseInOut<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.Bouncing:
                        target.transform.localScale = BezierCurve.Bouncing<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.Bouncing<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.BouncingR:
                        target.transform.localScale = BezierCurve.BouncingR<Vector3>(progress) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.BouncingR<float>(progress) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                    case AnimationMethod.Custom:
                        target.transform.localScale = BezierCurve.GetValue(
                            Vector3.one * useSteps[stepIdx].points.x,
                            Vector3.one * useSteps[stepIdx].points.y,
                            Vector3.one * useSteps[stepIdx].points.z,
                            Vector3.one * useSteps[stepIdx].points.w,
                            progress
                        ) * (useSteps[stepIdx].targetScale - scale) + scale;
                        canvasGroup.alpha = BezierCurve.GetValue(
                            useSteps[stepIdx].points.x,
                            useSteps[stepIdx].points.y,
                            useSteps[stepIdx].points.z,
                            useSteps[stepIdx].points.w,
                            progress
                        ) * (useSteps[stepIdx].targetAlpha - alpha) + alpha;
                        break;
                }
                if (Time.time - timer >= duration * useSteps[stepIdx].percentage)
                {
                    scale = useSteps[stepIdx].targetScale;
                    target.transform.localScale = scale;
                    alpha = useSteps[stepIdx].targetAlpha;
                    canvasGroup.alpha = alpha;
                    stepIdx++;
                    if (stepIdx >= useSteps.Count)
                    {
                        startAnimation = false;
                        switch (animationType)
                        {
                            case AnimationType.Open:
                                onOpenEnd();
                                break;
                            case AnimationType.Close:
                                onCloseEnd();
                                break;
                        }
                    }
                }
            }
            else
            {
                target.transform.localScale = BezierCurve.Linear<Vector3>(progress) * useSteps[stepIdx].targetScale;
                canvasGroup.alpha = BezierCurve.Linear<float>(progress) * useSteps[stepIdx].targetAlpha;
                if (Time.time - timer >= duration)
                {
                    canvasGroup.alpha = targetAlpha;
                    target.transform.localScale = targetScale;
                    startAnimation = false;
                    switch (animationType)
                    {
                        case AnimationType.Open:
                            onOpenEnd();
                            break;
                        case AnimationType.Close:
                            onCloseEnd();
                            break;
                    }
                }
            }
        }
    }

    public virtual async Task ShowAsync(params object[] args)
    {
        Open(args);

        while (gameObject.activeSelf)
        {
            await Task.Yield();
        }
    }

    public virtual void Open(params object[] args)
    {
        if (startAnimation) return;
        gameObject.SetActive(true);
        alpha = 0;
        canvasGroup.alpha = alpha;
        scale = Vector2.zero;
        target.transform.localScale = scale;
        ToOpen();
    }

    public virtual void ToOpen()
    {
        if (startAnimation) return;
        gameObject.SetActive(true);
        animationType = AnimationType.Open;
        scale = target.transform.localScale;
        alpha = canvasGroup.alpha;
        targetAlpha = 1;
        stepIdx = 0;
        targetScale = Vector2.one;
        timer = Time.time;
        startAnimation = true;
    }

    public virtual void Close()
    {
        if (startAnimation) return;
        gameObject.SetActive(true);
        alpha = 1;
        canvasGroup.alpha = alpha;
        scale = Vector2.one;
        target.transform.localScale = scale;
        ToClose();
    }

    public virtual void ToClose()
    {
        if (startAnimation) return;
        gameObject.SetActive(true);
        animationType = AnimationType.Close;
        scale = target.transform.localScale;
        alpha = canvasGroup.alpha;
        targetAlpha = 0;
        stepIdx = 0;
        targetScale = Vector2.zero;
        timer = Time.time;
        startAnimation = true;
    }

    protected virtual void onOpenEnd()
    {
        OnOpenEnd?.Invoke(this);
    }

    protected virtual void onCloseEnd()
    {
        gameObject.SetActive(false);
        OnCloseEnd?.Invoke(this);
    }
}
