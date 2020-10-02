using UnityEngine;

// プレイヤーに追跡するカメラ
public class TrackingCamera : MonoBehaviour
{
    private GameObject _player;
    void Start()
    {
        _player = GameObject.Find("player");
    }

    void Update()
    {
        var pos = _player.transform.position;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
