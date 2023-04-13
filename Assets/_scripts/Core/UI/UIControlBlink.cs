using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIControlBlink : MonoBehaviour {

    public Image ObservedObject;

    [Range(0.0f, 1.0f)]
    public float MinAplhaLimit = 0.1f;

    [Range(0.0f, 1.0f)]
    public float MaxAplhaLimit = 0.6f;

    [Range(0.0f, 1.0f)]
    public float BlinkLimit = 0.2f;

    [Range(0.0f, 2.0f)] 
    public float FadeTime = 1.0f;

    public bool ForcedBlink = false;

    //private GameObject _trgGraphics;
    private bool _blinking = false;
    private IEnumerator fadeProc;

    public UnityEvent OnBlinkingOn;
    public UnityEvent OnBlinkingOff;


    void Start()
    {
        if (!ObservedObject)
            return;
        //_trgGraphics = ObservedObject.targetGraphic.gameObject;
    }

    IEnumerator FadeOutAlpha()
    {
        while (ObservedObject.color.a > MinAplhaLimit)
        {
            Color displayColor = ObservedObject.color;
            displayColor.a -= Time.smoothDeltaTime / FadeTime;
            displayColor.a = Mathf.Max(displayColor.a, MinAplhaLimit);
            ObservedObject.color = displayColor;
            yield return null;
        }
        fadeProc = null;
        yield return null;
    }

    IEnumerator FadeInAlpha()
    {
        while (ObservedObject.color.a < MaxAplhaLimit)
        {
            Color displayColor = ObservedObject.color;
            displayColor.a += Time.smoothDeltaTime / FadeTime;
            displayColor.a = Mathf.Min(displayColor.a, MaxAplhaLimit);
            ObservedObject.color = displayColor;
            yield return null;
        }
        fadeProc = null;
        yield return null;
    }


    IEnumerator BlinkCycle()
    {
        _blinking = true;
        while (true)
        {
            Color clr = ObservedObject.color;
            if (clr.a >= MaxAplhaLimit && fadeProc == null)
            {
                fadeProc = FadeOutAlpha();
                StartCoroutine(FadeOutAlpha());
            }

            if (clr.a <= MinAplhaLimit && fadeProc == null)
            {
                fadeProc = FadeInAlpha();
                StartCoroutine(FadeInAlpha());
            }
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
        if (!ObservedObject)// || !_trgGraphics)
            return;
        if (ObservedObject.fillAmount < BlinkLimit || ForcedBlink)
	    {
            if (!_blinking)
            {
                StartCoroutine(BlinkCycle());
                OnBlinkingOn.Invoke();
            }
        }
	    else
            if (_blinking)
	        {
	            _blinking = false;
                StopAllCoroutines();
                StartCoroutine(FadeInAlpha());
                OnBlinkingOff.Invoke();
        }
    }
}
