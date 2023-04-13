using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFlash : MonoBehaviour 
{

    public Sprite HitMaterial;
    public float EffectTime = 0.2f;
    public Image PBImage;

    public Sprite BaseMaterial;

    public UnityEvent OnFlashAction;

    public void OnEnable()
    {
        PBImage.sprite = BaseMaterial;
    }

    IEnumerator HitEffect()
    {
        PBImage.sprite = HitMaterial;
        Color clr = PBImage.color;
        clr.a = 1f;
        float timer = 0f;
        PBImage.color = clr;
        while (clr.a > 0f)
        {
            yield return null;
            clr.a = Mathf.Lerp(1f, 0f, timer / EffectTime);
            timer += Time.deltaTime;
            PBImage.color = clr;
        }

        PBImage.sprite = BaseMaterial;
    }

    public void ApplyHitEffect()
    {
        if (!PBImage || !HitMaterial || !gameObject.activeSelf)
            return;
        StartCoroutine(HitEffect());
        OnFlashAction.Invoke();
    }
}
