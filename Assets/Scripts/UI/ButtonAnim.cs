using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    public void ClosePanel(GameObject Obj)
    {
        LTDescr descr;
        descr = LeanTween.scale(Obj.GetComponent<RectTransform>(), new Vector3(.001f ,.001f, .001f), 0.3f);
        descr.setEase(LeanTweenType.easeOutQuint);

        StartCoroutine(DisableObjDelayed(Obj, 0.5f));
    }

    IEnumerator DisableObjDelayed(GameObject Obj, float secs)
    {
        yield return new WaitForSeconds(secs);
        Obj.SetActive(false);
        yield return null;
    }
}
