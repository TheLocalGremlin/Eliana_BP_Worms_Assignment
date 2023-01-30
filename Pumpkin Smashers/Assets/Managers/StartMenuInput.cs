using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuInput : MonoBehaviour
{

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
