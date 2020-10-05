using UnityEngine;

public enum Dir
{
    Right,
    Left,
    Up,
    Down, 
    Stop,
    Max
}

enum EnemyType
{
    Key,
    Pac,
    Max
}

// 敵キャラの基底クラス
// 速度、向きなど最低限必要な情報を持っている
interface IEnemy
{
    void Move();
    bool IsHitObject(Vector3 pos);
}
