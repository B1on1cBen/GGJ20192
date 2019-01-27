using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int index;

    public void LoadScene(int index)
    {
        this.index = index;
        Invoke("Delay", .25f);
    }

    void Delay()
    {
        SceneManager.LoadScene(index);
    }
}
