using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _time;       // 時間のテキスト
    [SerializeField]
    private GameObject _goal;
    Goal _finish;
    private SpriteRenderer _sprite; // フェード用の画像
    float _lim = 0.25f;
    float _alpha = 1f;
    Color _color;
    float _starttime = 0;
    IEnumerator Start()
    {
        _starttime = Time.time;
        _sprite = GetComponent<SpriteRenderer>();
        _color = _sprite.material.color;
        _finish = _goal.GetComponent<Goal>();
        while (FadeIn())
        {
            yield return null;
        }
    }

    void Update()
    {
        var text = _time.GetComponent<Text>();
        var t = Time.time;
        t -= _starttime;
        int m = (int)t / 60;
        int s = (int)t % 60;
        if (s < 10)
        {
            text.text = "Time : " + m + " : 0" + s;
        }
        else
        {
            text.text = "Time : " + m + " : " + s;
        }
        if (_finish.GetGoal())
        {
            StartCoroutine("GoalFade");
        }
    }

    public IEnumerator GoalFade()
    {
        while (FadeOut())
        {
            yield return null;
        }
        SceneManager.LoadScene("Title");
    }

    private bool FadeIn()
    {
        _alpha -= 0.01f;
        _color.a = _alpha;
        _sprite.material.color = _color;
        if (_alpha > _lim)
        {
            return true;
        }
        StopCoroutine("FadeIn");
        return false;
    }

    private bool FadeOut()
    {
        _alpha += 0.01f;
        _color.a = _alpha;
        _sprite.material.color = _color;
        if (_alpha < 1f)
        {
            return true;
        }
        StopCoroutine("FadeOut");
        return false;
    }
}
