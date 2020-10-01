using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Array
{
    public int x;
    public int y;
    public Array()
    {
        x = y = 0;
    }
    public Array(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
    public Array(Vector2Int v)
    {
        x = v.x;
        y = v.y;
    }
}

// グリッドラインの表示
// マップ情報の管理など
public class StageGrider : MonoBehaviour
{
    public Vector2Int _cellnum;
    float _size = 1;
    private Array _array;
    private bool[,] _blocks;
    [SerializeField]
    private Sprite _black;
    [SerializeField]
    private Sprite _white;

    void Start()
    {
        _array = new Array(_cellnum);
        _blocks = new bool[_cellnum.x, _cellnum.y];

        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                var r = (int)UnityEngine.Random.Range(0, 100f) % 2;
                _blocks[p,i] = r == 0 ? true : false;
            }
        }
        SetImage();
    }

    void SetImage()
    {
        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                var s = gameObject.AddComponent<SpriteRenderer>();
                if (_blocks[i, p])
                {
                    s.sprite = _black;
                    gameObject.AddComponent<SortingGroup>().sortingLayerName = "object";
                }
                else
                {
                    s.sprite = _white;
                    gameObject.AddComponent<SortingGroup>().sortingLayerName = "object";
                }
            }
        }
    }

    private void Update()
    {
        DrawGridLine();
    }

    void DrawGridLine()
    {
        // 横のライン
        for(int y = 0;y <= _array.y; y++)
        {
            var p = y * _size;
            Debug.DrawLine(new Vector3(0, p), new Vector3(_cellnum.x * _size, p));
        }
        // 縦のライン
        for (int x = 0; x <= _array.x; x++)
        {
            var p = x * _size;
            Debug.DrawLine(new Vector3(p,0), new Vector3(p,_cellnum.y * _size));
        }
    }
}
