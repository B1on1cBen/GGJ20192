using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class WinningScript : MonoBehaviour {

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip winningSound;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}

    public void winCheck()
    {
        foreach(Furniture furniture in GameManager.manager.furniture)
        {
            if (furniture.WinningPosition.gameObject != furniture.OriginSquare)
            {
                return;
            }
        }

       StartCoroutine(Completed());
    }

    IEnumerator Completed()
    {
        source.PlayOneShot(winningSound);
        yield return new WaitWhile(() => source.isPlaying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
