using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラで取得した画像をFilterにかける
[ExecuteInEditMode]
public class CameraFilter : MonoBehaviour
{
    [SerializeField] private Material _filter;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _filter);
    }
}
