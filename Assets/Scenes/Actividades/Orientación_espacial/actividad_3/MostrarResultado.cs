using UnityEngine;
using TMPro;

public class MostrarResultado : MonoBehaviour
{
    public TextMeshProUGUI textoResultado;

    void Start()
    {
        textoResultado.text = "Resultado: " + ResultadoEvaluacion.ZonaFinal + ResultadoEvaluacion.DetallesEvaluacion;
    }
}
