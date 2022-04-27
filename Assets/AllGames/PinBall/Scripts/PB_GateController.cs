using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GateController : MonoBehaviour
{
    public static PB_GateController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject leftMainGate, rightMainGate;
    [SerializeField] private GameObject leftHinge, rightHinge;
    [SerializeField] private GameObject leftGate1, leftGate2, leftGate3, leftGate4;
    [SerializeField] private GameObject rightGate1, rightGate2, rightGate3, rightGate4;

    public IEnumerator OpenLeftGate()
    {
        leftMainGate.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2f);
        leftHinge.GetComponent<Animator>().SetTrigger("Open");
        rightHinge.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2f);
        leftMainGate.GetComponent<Animator>().SetTrigger("Close");
        leftHinge.GetComponent<Animator>().SetTrigger("Close");
        rightHinge.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(2f);
        leftGate1.SetActive(false);
        yield return new WaitForSeconds(1f);
        leftGate1.SetActive(true);
        leftGate2.SetActive(false);
        yield return new WaitForSeconds(1f);
        leftGate2.SetActive(true);
        leftGate3.SetActive(false);
        yield return new WaitForSeconds(1f);
        leftGate3.SetActive(true);
        leftGate4.SetActive(false);
        yield return new WaitForSeconds(1f);
        leftGate4.SetActive(true);
    }
    public IEnumerator OpenRightGate()
    {
        rightMainGate.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2f);
        leftHinge.GetComponent<Animator>().SetTrigger("Open");
        rightHinge.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2f);
        rightMainGate.GetComponent<Animator>().SetTrigger("Close");
        leftHinge.GetComponent<Animator>().SetTrigger("Close");
        rightHinge.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(2f);
        rightGate1.SetActive(false);
        yield return new WaitForSeconds(1f);
        rightGate1.SetActive(true);
        rightGate2.SetActive(false);
        yield return new WaitForSeconds(1f);
        rightGate2.SetActive(true);
        rightGate3.SetActive(false);
        yield return new WaitForSeconds(1f);
        rightGate3.SetActive(true);
        rightGate4.SetActive(false);
        yield return new WaitForSeconds(1f);
        rightGate4.SetActive(true);
    }
}
