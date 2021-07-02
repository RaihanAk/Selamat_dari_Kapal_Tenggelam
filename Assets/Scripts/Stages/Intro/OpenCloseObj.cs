 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseObj : MonoBehaviour
{
    public GameObject target;

    public void OnClickOpenCloseObjButton(bool state)
    {
        target.gameObject.SetActive(state);
    }
}
