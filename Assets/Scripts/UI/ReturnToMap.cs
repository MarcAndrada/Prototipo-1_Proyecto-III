using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMap : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("NodeMap");
    }
}
