using System.Collections;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] _obj;
    private int _nowSelect = 0;
    private bool _isScele = true;
    private Vector3 _small;
    private Vector3 _wide;
    private readonly float _moveH = 0.001f;

    void Start()
    {
        _small = transform.localScale;
        _wide = transform.localScale * 1.3f;
    }

    void Update()
    {
        StartCoroutine("PopButton");
        if (Input.GetButtonDown("Vertical"))
        {
            var pos = transform.position;
            _nowSelect = ((int)Input.GetAxisRaw("Vertical") - 1) / -2;
            var sel = _obj[_nowSelect].transform.position.y;
            pos.y = sel;
            transform.position = pos;
        }
    }

    IEnumerator PopButton()
    {
        var scale = transform.localScale;
        if (_isScele)
        {
            scale = new Vector3(scale.x + _moveH, scale.y + _moveH);
            if (scale.x > _wide.x)
            {
                _isScele = false;
                yield break;
            }
        }
        else
        {
            scale = new Vector3(scale.x - _moveH, scale.y - _moveH);
            if (scale.x < _small.x)
            {
                _isScele = true;
                yield break;
            }
        }
        transform.localScale = scale;       
        yield return null;
    }
}
