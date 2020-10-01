using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using System;

//enum Color
//{
//    Red,Green,Blue,Max
//}

// 光源を動かすスクリプト
// 主にマウスでプレイヤーが移動させる
// プレイヤー本体が動いてる時と、動いてない時で
// 揺れや光の強弱にメリハリをつける
public class LightMoving : MonoBehaviour
{
    Light _light;
    Vector2 _lightPos;
    float _posZ;
    Color _color;
    int _rgb;

    void Start()
    {
        _light = this.GetComponent<Light>();
        _posZ = transform.position.z;
        _color = new Color(1f, 0, 0);
        _rgb = 0;
    }

    void Update()
    {
        SetLightColor();
        SetLightPosition();
        LightShake();
    }

    // ゆらゆら揺らすための処理
    private void LightShake()
    {

    }

    private void SetLightColor()
    {
        if (Input.GetButtonDown("Decision"))
        {
            _rgb++;
            _rgb %= 3;
            Light2D point = GetComponent<Light2D>();
            point.color = new Color();
            var c = point.color;
            for(int i = 0;i < 3; i++)
            {
                c[i] = i == _rgb ? 1f : 0;
            }
            point.color = c;
        }
    }

    void SetLightPosition()
    {
        // マウス座標とスクリーンのサイズを考慮して
        // ライトの座標を設定する
        _lightPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        transform.position = new Vector3(_lightPos.x,_lightPos.y,_posZ);
    }
}
