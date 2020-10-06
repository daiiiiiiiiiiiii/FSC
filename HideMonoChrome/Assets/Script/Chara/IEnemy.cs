public enum MoveType
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
    Pac_W,
    Pac_B,
    Max
}

interface IEnemy
{
    EnemyType GetEnemyType();
}
