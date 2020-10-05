using UnityEngine;

// プレイヤーに追跡するカメラ
public class TrackingCamera : MonoBehaviour
{
    private GameObject _player;
    private Camera _camera;
    [SerializeField]
    private EdgeCollider2D _outLine;
    private Vector3 _width;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.Find("player");
        var z = _camera.ViewportToWorldPoint(Vector2.zero);
        var o = _camera.ViewportToWorldPoint(Vector2.one);
        _width = new Vector3(o.x - z.x,o.y - z.y) / 2;
    }

    void Update()
    {
        var pos = _player.transform.position;
        pos = Clamp(pos);
        pos.z = transform.position.z;
        transform.position = pos;
    }
    Vector3 Clamp(Vector3 pos)
    {
        var min = _outLine.points[0];
        var max = _outLine.points[2];
        pos.x = Mathf.Clamp(pos.x, min.x + _width.x, max.x - _width.x);
        pos.y = Mathf.Clamp(pos.y, min.y + _width.y, max.y - _width.y);
        // Debug.Log("min :"+min + "max:" + max + "pos :" + pos + "幅:" + _width);
        return pos;
    }
}
