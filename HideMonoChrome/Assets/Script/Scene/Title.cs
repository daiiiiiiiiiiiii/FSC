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
        if (Input.anyKey)
        {
            _fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("Menu");
            });
        }       
    }
}
