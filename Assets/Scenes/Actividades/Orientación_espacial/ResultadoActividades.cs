[System.Serializable]
public class ResultadoActividades
{
    public string actividad;
    public string childId;
    public string respuestaSeleccionada;
    public bool esCorrecta;
    public float duracionSegundos;
    public int vecesEscuchoInstruccion;
    public string timestamp;
    public string imagenBase64;

    public ResultadoActividades(string actividad)
    {
        this.actividad = actividad;
    }
}
