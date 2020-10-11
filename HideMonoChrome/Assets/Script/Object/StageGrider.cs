using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

enum BlockType
{
    Black,
    White,
    Gray,
    Warp,
    Max
}

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
    [SerializeField]
    private Vector2Int _cellnum;         // 升目の数
    float _offset = 0.5f;
    private Array _array;               // ステージの縦横サイズ
    private BlockType[,] _blockType;    // ブロックの種類
    private GameObject _blocks;         // 全ブロックの親オブジェクト
    [SerializeField]
    private GameObject _black;          // 黒ブロック
    [SerializeField]
    private GameObject _white;          // 白ブロック
    [SerializeField]
    private GameObject _gray;           // 灰ブロック
    Dictionary<BlockType, GameObject> 
        _blockTable;                    // ブロックテーブル

    private GameObject _player;         // プレイヤーの情報
    private GameObject _enemyParent;    // 敵キャラクターの親オブジェクト
    public Vector3 _keyPos{ get; set; } // キーの初期座標

    // キー関連
    private bool _isDecision;           // 決定を押したか

    void Start()
    {
        _array = new Array(_cellnum);
        _blocks = new GameObject("_blocks");
        _blocks.transform.parent = transform;
        _blockType = new BlockType[_cellnum.x, _cellnum.y];
        CreateBlockTable();

        _enemyParent = GameObject.Find("Enemy");
        _player = GameObject.Find("player");
        gameObject.AddComponent<SortingGroup>().sortingLayerName = "object";
        InitBlocks();
        SetImage();
    }

    private void CreateBlockTable()
    {
        _blockTable = new Dictionary<BlockType, GameObject>();
        _blockTable.Add(BlockType.Black, _black);
        _blockTable.Add(BlockType.White, _white);
        _blockTable.Add(BlockType.Gray, _gray);
        // _blockTable.Add(BlockType.Warp, _gray);
    }

    // 初期のブロック配置を決定
    private void InitBlocks()
    {
        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                _blockType[p, i] = (BlockType)(UnityEngine.Random.Range(0, 100f) % 2);
            }
        }
        var x = (int)UnityEngine.Random.Range(1, _cellnum.x);
        var y = (int)UnityEngine.Random.Range(1, _cellnum.y);
        _keyPos = new Vector3(x + _offset, y + _offset);
        _blockType[x, y] = BlockType.Black;
        _enemyParent.transform.GetChild(0).transform.position = _keyPos;
        for (int i = 0; i < 10; i++)
        {
            x = (int)UnityEngine.Random.Range(1, _cellnum.x);
            y = (int)UnityEngine.Random.Range(1, _cellnum.y);
            _blockType[x, y] = BlockType.Gray;
        }
        _blockType[0, 0] = BlockType.Black;
    }

    void SetImage()
    {
        // 子オブジェクトの登録
        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                Vector3 pos = new Vector3(p + _offset, i + _offset);
                GameObject obj = (GameObject)Instantiate(_blockTable[_blockType[p, i]], pos, Quaternion.identity);
                obj.transform.parent = _blocks.transform;             
            }
        }
    }

    private void Update()
    {
        KeyInputAction();
        DrawGridLine();
        if (_isDecision)
        {
            SetBlocks();
        }
    }

    private void SetBlocks()
    {
        var tmp = _blockType;
        Destroy(_blocks);
        _blocks = new GameObject("_blocks");
        _blocks.transform.parent = transform;
        var pos = GetCurrentCharaPos(_player);
        var key = GetCurrentCharaPos(_enemyParent.transform.GetChild(0).gameObject);
        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                if (!((p == pos.x && i == pos.y)
                 || (p == key.x && i == key.y)))
                {
                    if(tmp[p,i] == BlockType.Black)
                    {
                        _blockType[p, i] = BlockType.White;
                    }
                    else if(tmp[p, i] == BlockType.White)
                    {
                        _blockType[p, i] = BlockType.Black;
                    }
                    else
                    {
                        _blockType[p, i] = tmp[p, i];
                    }
                }
            }
        }
        SetImage();
    }

    private void KeyInputAction()
    {
        _isDecision = Input.GetButtonDown("Decision");
    }

    private Vector2Int GetCurrentCharaPos(GameObject chara)
    {
        Vector2Int pos = new Vector2Int((int)chara.transform.position.x, (int)chara.transform.position.y);
        return pos;
    }

    void DrawGridLine()
    {
        // 横のライン
        for(int y = 0;y <= _array.y; y++)
        {
            Debug.DrawLine(new Vector3(0, y), new Vector3(_cellnum.x, y));
        }
        // 縦のライン
        for (int x = 0; x <= _array.x; x++)
        {
            Debug.DrawLine(new Vector3(x,0), new Vector3(x,_cellnum.y));
        }
    }
}
