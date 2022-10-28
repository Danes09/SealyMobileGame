using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerInfiniteLevel : MonoBehaviour
{
    GameObject box;

    private void Start()
    {
        setCorrectTimeScale();
        
    }


    void setCorrectTimeScale()
    {
        Time.timeScale = 1f;
    }


}
