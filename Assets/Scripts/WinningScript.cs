using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class WinningScript : MonoBehaviour {

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip winningSound;

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
	}

    public void winCheck()
    {
        foreach(Furniture furniture in gameManager.furniture)
        {
            if (furniture.WinningPosition.gameObject != furniture.OriginSquare)
            {
                return;
            }
        }

        print("winning");
       StartCoroutine(Completed());
    }

    IEnumerator Completed()
    {
        source.PlayOneShot(winningSound);

        while (source.isPlaying)
        {         
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
