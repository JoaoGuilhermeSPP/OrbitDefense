using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuiManager : MonoBehaviour
{
    public void Reiniciar()
    {
        SceneManager.LoadScene(2);
    }

    public void VoltaMenu()
    {
        SceneManager.LoadScene(0);
    }

  
}