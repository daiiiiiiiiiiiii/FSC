using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Fade _fade;
    void Start()
    {
       // _fade.FadeOut(1f);
    }

    void Update()
    {
        if (Input.anyKey)
        {
            _fade.FadeIn(1f, () =>
            {
                SceneManager.LoadScene("Stage1");
            });
           
        }
    }
}
