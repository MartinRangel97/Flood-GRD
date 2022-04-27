using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Dropdown levels;
    public void loadLevel()
    {
        string[] level = levels.options[levels.value].text.Split(' ');
        WorldManager.SetLevel(int.Parse(level[1]));
        SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
    }
}
