using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public void BackToHome() {
        SceneManager.LoadScene("Home");
    }

    public void GoToTerrainTest() {
        SceneManager.LoadScene("UIDistanceTerrain");
    }

    public void GoToMidAirTest() {
        SceneManager.LoadScene("UIDistanceMidAir");
    }
}
