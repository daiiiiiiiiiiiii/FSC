using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

enum BlockType
{
    Black,
    White,
    Gray,
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
    private Vector2Int _cellnum;            // 升目の数
    float _offset = 0.5f;
    private Array _array;                   // ステージの縦横サイズ
    private BlockType[,] _blockType;        // ブロックの種類
    private GameObject _blocks;             // 全ブロックの親オブジェクト
    [SerializeField]
    private GameObject _black;              // 黒ブロック
    [SerializeField]
    private GameObject _white;              // 白ブロック
    [SerializeField]
    private GameObject _gray;               // 灰ブロック
    [SerializeField]
    private GameObject[] _warp;             // ワープ
    Dictionary<BlockType, GameObject> 
        _blockTable;                        // ブロックテーブル
    [SerializeField]
    private AudioClip[] _sound;             // ブロック切り替え用サウンド
    private bool _switch = false;           // サウンド判断用フラグ

    private GameObject _player;             // プレイヤーの情報
    private GameObject _enemyParent;        // 敵キャラクターの親オブジェクト
    public Vector3 _keyPos{ get; set; }     // キーの初期座標
    public Vector3 _goalPos { get; set; }   // キーの初期座標

    // 初期演出判定
    private bool _isFall = true;            // ブロック落下中
    private float _time = 0;                // 落下時間

    // キー関連
    private bool _isDecision;               // 決定を押したか

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
        SetInit();
    }

    private void CreateBlockTable()
    {
        _blockTable = new Dictionary<BlockType, GameObject>();
        _blockTable.Add(BlockType.Black, _black);
        _blockTable.Add(BlockType.White, _white);
        _blockTable.Add(BlockType.Gray, _gray);
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
            x = (int)UnityEngine.Random.Range(1, _cellnum.x - 1);
            y = (int)UnityEngine.Random.Range(1, _cellnum.y - 1);
            _blockType[x, y] = BlockType.Gray;
        }
        for (int i = 0; i < 2; i++)
        {
            x = i * 10 + (int)UnityEngine.Random.Range(1, 5);
            y = (int)UnityEngine.Random.Range(1, _cellnum.y);
            _warp[i].transform.position = new Vector3(x + _offset, y + _offset);
            _blockType[x, y] = BlockType.Black;
        }
        _goalPos = new Vector3(_cellnum.x - 1 + _offset, _cellnum.y - 1 + _offset);
        _enemyParent.transform.GetChild(1).transform.position = _goalPos;
        _blockType[_cellnum.x - 1, _cellnum.y - 1] = BlockType.Black;
        _blockType[0, 0] = BlockType.Black;
    }

    void SetInit()
    {
        // 子オブジェクトの登録
        for (int p = 0; p < _cellnum.x; p++)
        {
            for (int i = 0; i < _cellnum.y; i++)
            {
                Vector3 pos = new Vector3(p + _offset, 15);
                GameObject obj = (GameObject)Instantiate(_blockTable[_blockType[p, i]], pos, Quaternion.identity);
                obj.transform.parent = _blocks.transform;
            }
        }
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

        if (_isFall)
        {
            Time.timeScale = 1f;
            _isFall = SetBlockPostion();
            if (_isFall)
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            KeyInputAction();
            if (_isDecision)
            {
                int num = 0;
                if (_switch)
                {
                    num = 1;
                }
                GetComponent<AudioSource>().PlayOneShot(_sound[num]);
                SetBlocks();
            }
        }
    }

    private bool SetBlockPostion()
    {
        _time++;       
        bool flag = false;
        for (int y = 0; y < _cellnum.y; y++)
        {
            for (int x = 0; x < _cellnum.x; x++)
            {
                Vector3 pos = new Vector3(x + _offset, _offset + y);
                var obj = _blocks.transform.GetChild(x * _cellnum.y + y);
                obj.position = Vector3.Lerp(new Vector3(x + _offset, 15), pos, _time / 200);
                if(obj.position != pos)
                {
                    flag = true;
                }
            }
        }
        return flag;
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
                 || (p == key.x && i == key.y)
                 || (p == _goalPos.x && i == _goalPos.y)))
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
            _blockType[(int)_goalPos.x, (int)_goalPos.y] = BlockType.Black;
        }
        for (int i = 0; i < 2; i++)
        {
            Array ar = new Array((int)_warp[i].transform.position.x, (int)_warp[i].transform.position.y);
            _blockType[ar.x, ar.y] = BlockType.Black;
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
}
