using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField]
    private GameObject _warp;
    float _start;
    private void OnTriggerExit2D(Collider2D col)
    {
        _start = 0f;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        _start += Time.deltaTime;
        if (_start >= 5f)
        {
            col.transform.position = _warp.transform.position;
        }
    }
}
