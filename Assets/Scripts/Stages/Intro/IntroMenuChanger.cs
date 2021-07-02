using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroMenuChanger : MonoBehaviour
{
    public GameObject[] menus;

    public void ChangeFocus(int index)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i != index)
            {
                menus[i].GetComponent<RectTransform>().transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                menus[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                menus[i].GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);
                menus[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ExitFocus()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);
            menus[i].GetComponent<Button>().interactable = true;
        }
    }
}
