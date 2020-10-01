using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLight : MonoBehaviour
{
    float _posZ;
    float _angleZ;
    [SerializeField] GameObject _gameObj;

    void Start()
    {
        _posZ = transform.position.z;
        _angleZ = transform.rotation.z;
    }

    void Update()
    {
        SetLightRotation();
        SetLightPosition();
    }

    private void SetLightRotation()
    {
        var scl = _gameObj.transform.localScale.x;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, scl * -90f);
    }

    private void SetLightPosition()
    {
        // プレイヤーの座標にライトを設定する
        var pos = _gameObj.transform.position;
        transform.position = new Vector3(pos.x + 0.1f * transform.localScale.x,pos.y + 0.6f,_posZ);
    }
}
