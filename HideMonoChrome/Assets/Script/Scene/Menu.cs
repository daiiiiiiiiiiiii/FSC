using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Fade _fade;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey)
        {
            _fade.FadeOut(1f, () =>
            {
                //SceneManager.LoadScene("Menu");
            });
        }
    }
}
