using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCams : MonoBehaviour
{
    public GameObject[] cams;
    public void InIt()
    {
        EnableRandomCamera();
    }

    void EnableRandomCamera()
    {
        int r = Random.Range(0, cams.Length);
        cams[r].SetActive(true);
    }

}
