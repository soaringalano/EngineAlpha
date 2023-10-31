using UnityEngine;
using UnityEngine.SceneManagement;

// Object.DontDestroyOnLoad example.
//
// Two scenes call each other. This happens when OnGUI button is clicked.
// scene1 will load scene2; scene2 will load scene1. Both scenes have
// the Menu GameObject with the SceneSwap.cs script attached.
//
// AudioSource plays an AudioClip as the game runs. This is on the
// BackgroundMusic GameObject which has a music tag.  The audio
// starts in AudioSource.playOnAwake. The DontDestroy.cs script
// is attached to BackgroundMusic.

public class SwitchSceneManager : MonoBehaviour
{

    private void Awake()
    {
    }

    private void OnGUI()
    {
        /*int xCenter = (Screen.width / 2);
        int yCenter = (Screen.height / 2);
        int width = 400;
        int height = 120;

        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
        fontSize.fontSize = 32;*/

        if(Input.GetKeyDown(KeyCode.M))
        {
            Scene scene = SceneManager.GetActiveScene();


            if (scene.name == "Level1")
            {
                // Show a button to allow scene2 to be switched to.
                Debug.Log("Now loading level 2");

                SceneManager.LoadScene("Level2");
            }
            else if (scene.name == "Level2")
            {
                Debug.Log("Now loading level 3");
                SceneManager.LoadScene("Level3");
            }
            else
            {
                Debug.Log("No more new levels");
            }

        }

    }
}