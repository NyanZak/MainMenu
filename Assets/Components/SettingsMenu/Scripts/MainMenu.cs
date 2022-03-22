using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string APP_PC_LINK = "INSERT LINK HERE";
    private string twitternameParameter = "Just played GAME TITLE, GAME DESCRIPTION";
    private string twitterdescriptionParam = "";
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWITTER_LANGUAGE = "en";

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Promote()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(twitternameParameter + "\n" + twitterdescriptionParam + "\n" + APP_PC_LINK));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
