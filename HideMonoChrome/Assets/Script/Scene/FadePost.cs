using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class FadePost : MonoBehaviour
{
    PostProcessVolume _postVol;
    PostProcessProfile _postProf;
    Vignette _vig;
    [SerializeField]SpriteRenderer _sprite;
    float _intens;
    float _lim = 0.25f;
    float _alpha = 1f;
    Color _color;
    void Start()
    {
        _postVol = GetComponent<PostProcessVolume>();
        _postProf = _postVol.profile;
        _vig = _postProf.GetSetting<Vignette>();
        _intens = 1f;
        _vig.intensity.Override(_lim);
        _color = _sprite.material.color;
    }

    public void OnButtonClick()
    {
        ChangeRandomColor();
    }

    private void ChangeRandomColor()
    {
        SceneManager.LoadScene("Stage1");
    }

    void Update()
    {
        _alpha -= 0.003f;
        _color.a = _alpha;
        _sprite.material.color = _color;
        if (_lim < _intens)
        {
            _intens -= 0.003f;
            _vig.intensity.Override(_intens);
        }
    }
}
