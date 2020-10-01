using UnityEngine;

public enum State
{
    Idle,
    Run,
    Jump,
    Dead,
    Max
}

public enum ObjectType
{
    None,
    Ground,
    Needle,
    FloarUD,
    FloarLR,
    Goal,
    Max
}

public interface IHitObject
{
    Vector2 SetPosition(Rigidbody2D rb);
}
