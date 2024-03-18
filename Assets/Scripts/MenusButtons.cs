using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusButtons : MonoBehaviour
{
    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
    public void BorraRecord()
    {
        PlayerPrefs.DeleteKey("record");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
