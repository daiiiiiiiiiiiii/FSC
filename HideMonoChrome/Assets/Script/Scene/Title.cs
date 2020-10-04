using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Fade _fade;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            _fade.FadeIn(2f, () =>
            {
                SceneManager.LoadScene("Stage1");
                _fade.FadeOut(3f);
            });
        }       
    }
}
