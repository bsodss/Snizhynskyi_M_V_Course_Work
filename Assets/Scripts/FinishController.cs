using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private bool isActivated = false;

    public void Activate()
    {
        isActivated = true;
    }
    public void Finishlevel()
    {
        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }
}
