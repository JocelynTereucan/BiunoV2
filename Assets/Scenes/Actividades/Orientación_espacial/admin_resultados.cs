using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ResultadoActividad
{
    public string actividad;
    public string resultado;
}

[System.Serializable]
public class ResultadosAlumno
{
    public string nombreAlumno;
    public List<ResultadoActividad> actividades = new List<ResultadoActividad>();
}

public class ResultadosManager : MonoBehaviour
{
    public static ResultadosManager instancia;
    public string nombreAlumno = "Alumno1";
    private ResultadosAlumno resultadosAlumno;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(this.gameObject);
            resultadosAlumno = new ResultadosAlumno();
            resultadosAlumno.nombreAlumno = nombreAlumno;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarResultado(string actividad, string resultado)
    {
        resultadosAlumno.actividades.Add(new ResultadoActividad
        {
            actividad = actividad,
            resultado = resultado
        });
    }

    public void FinalizarYGuardar()
    {
        string json = JsonUtility.ToJson(resultadosAlumno, true);
        string path = Path.Combine(Application.persistentDataPath, "resultados_" + nombreAlumno + ".json");
        File.WriteAllText(path, json);
        Debug.Log("Resultados guardados en: " + path);
    }
}
