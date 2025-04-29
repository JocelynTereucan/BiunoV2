using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPantallasOrientacion : MonoBehaviour
{
    [SerializeField] private string escenaActividad = "PantallaActividad";
    [SerializeField] private float tiempoPantallaInicial = 10f;

    void Start()
    {
        StartCoroutine(CambiarAPantallaActividad());
    }

    IEnumerator CambiarAPantallaActividad()
    {
        yield return new WaitForSeconds(tiempoPantallaInicial);
        SceneManager.LoadScene(escenaActividad);
    }
}

