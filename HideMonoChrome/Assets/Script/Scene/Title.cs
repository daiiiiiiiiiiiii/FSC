using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Title : MonoBehaviour
{
    [SerializeField]
    Button[] _btn;
    AudioSource _audio;             // 音を鳴らす為のインターフェース
    [SerializeField]
    private AudioClip _sound;       // 効果音

    private SpriteRenderer _sprite; // フェード用の画像
    float _lim = 0.25f;
    float _alpha = 1f;
    Color _color;

    Vignette _vig;
    PostProcessProfile _prof;
    string[] _scene;

    IEnumerator Start()
    {
        _scene = new string[2]
        {
            "Stage1",
            "Stage1"
        };
        _audio = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
        _prof = GetComponent<PostProcessVolume>().profile;
        _color = _sprite.material.color;
        _vig = _prof.GetSetting<Vignette>();
        while (FadeIn())
        {
            yield return null;
        }
    }

    void Update()
    {
    }

    public void OnButton(int i)
    {
        _audio.PlayOneShot(_sound);
        _btn[i].interactable = false;
        StartCoroutine("OnButtonClick", i);
    }

    public IEnumerator OnButtonClick(int i)
    {      
        while (FadeOut())
        {
            yield return null;
        }
        SceneManager.LoadScene(_scene[i]);
    }

    private bool FadeIn()
    {
        _alpha -= 0.003f;
        _color.a = _alpha;
        _sprite.material.color = _color;
        if (_alpha > _lim)
        {
            _vig.intensity.Override(_alpha);
            return true;
        }
        StopCoroutine("FadeIn");
        return false;
    }

    private bool FadeOut()
    {
        _alpha += 0.003f;
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
