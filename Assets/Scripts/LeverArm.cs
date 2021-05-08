using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverArm : MonoBehaviour
{
    private FinishController finish;
    private void Start()
    {
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<FinishController>();
    }
    public void ActivateLevelArm()
    {
        finish.Activate();
    }
}
