using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    
    // Start is called before the first frame update
    AudioSource audioSource;
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        //get specific audio source from children
        audioSource = GetComponentsInChildren<AudioSource>()[1]; // Assuming the first AudioSource child is the one we want
    }
    public void buttonStartGame()
    {
        StartCoroutine(PlayAudioAndLoadScene());
    }

    private IEnumerator PlayAudioAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f); // Wait for the audio to finish playing plus a small buffer
        SceneManager.LoadScene("World");
    }
}
