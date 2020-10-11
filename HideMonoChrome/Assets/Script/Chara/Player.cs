using System;
using UnityEngine;

enum State
{
    Idle,
    Run,
    Jump,
    Max
}

public class Player : MonoBehaviour
{
    private State _state;               // 現在の状態
    Rigidbody2D _rb;                    // プレイヤーのrigidbody
    Animator _animator;                 // アニメーション切り替え用変数
    String[] _animFlagName;             // アニメーションフラグの名前
    int _animNum;                       // アニメーションの種類
    public GameObject _start;           // 開始地点

    /// <summary>
    /// 左右移動用変数
    /// </summary>
    private float _dir;                 // 移動方向(左右)
    private float _runSpeed = 5f;       // 走る速度

    /// <summary>
    /// ジャンプ用変数
    /// </summary>
    private bool _isJumpFlag = false;   // ジャンプしているかどうか
    private float _jumpHeight = 250f;   // ジャンプの高さ

    private bool _isGround = false;     // 地面にいるかどうか
    private bool _dead = false;         // 死亡、リスポーンフラグ

    // 初期化
    void Start()
    {
        // 初期状態の設定
        _state = State.Idle;
        this._rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animFlagName = new String[]{
            "_idle","_run","_jump"
        };
        _animNum = _animFlagName.Length;
        transform.position = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        // キー入力によって移動を行う
        GetInput();
        Move();
        SetState();
    }

    // 何らかの処理に必要なキー入力の取得
    void GetInput()
    {
        _dir = Input.GetAxisRaw("Horizontal");
        _isJumpFlag = Input.GetButtonDown("Jump");
        _dead = Input.GetButtonDown("Resporn");
    }

    void Move()
    {
        // 左右移動
        this._rb.velocity = new Vector2(_runSpeed * _dir, _rb.velocity.y);

        if (_isGround && _dir == 0)
        {
            this._rb.velocity = new Vector2(0, this._rb.velocity.y);
        }

        // ジャンプ
        if (_isGround && _isJumpFlag)
        {
            this._rb.AddForce(transform.up * _jumpHeight);
            _isGround = false;
        }
    }

    void SetState()
    {

        if (_dir == 0)
        {
            _state = State.Idle;
        }
        else
        {
            _state = State.Run;
        }
        if (!_isGround)
        {
            _state = State.Jump;
        }
        SetAnimInfo((int)_state);
        // 画像の向きを変える
        InverPlayerImage();
    }

    void InverPlayerImage()
    {
        // 画像反転
        var scl = this.transform.localScale;
        if (_dir != 0)
        {
            scl.x = _dir;
        }
        this.transform.localScale = scl;
    }

    void SetAnimInfo(int num)
    {
        bool flag = false;
        for (int i = 0; i < _animNum; i++)
        {
            flag = i == num ? true : false;
            _animator.SetBool(_animFlagName[i], flag);
        }
    }

    // 着地判定
    void OnTriggerEnter2D(Collider2D col)
    {
        _isGround = true;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        _isGround = true;
    }
    // 空中判定
    private void OnTriggerExit2D(Collider2D col)
    {
        _isGround = false;
    }
}
