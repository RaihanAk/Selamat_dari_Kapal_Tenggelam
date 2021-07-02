using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSequencer : MonoBehaviour
{
    public GameObject[] panels;
    public int sequenceNum = 0;

    public bool isStartWhenLoaded = true;

    void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);

            //if (isStartWhenLoaded)
            //{
            //    if (i != sequenceNum)
            //        panels[i].SetActive(false);
            //    else
            //        panels[i].SetActive(true);
            //}
            //else
            //    panels[i].SetActive(false);
        }
    }

    public void StartPanelSequencer()
    {
        panels[0].SetActive(true);
    }

    public void NextPanelSequence()
    {
        ClosePanel(panels[sequenceNum]);

        panels[++sequenceNum].SetActive(true);
    }

    public void ClosePanel(GameObject Obj)
    {
        LTDescr descr;
        descr = LeanTween.scale(Obj.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0.3f);
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
