[System.Serializable]
public class Actividad1Resultado
{
    public string actividadId;
    public string childId;
    public string respuestaSeleccionada;
    public bool esCorrecta;
    public float duracionSegundos;
    public int vecesEscuchoInstruccion;
    public string timestamp;
    public string imagenBase64;

    public Actividad1Resultado()
    {
        actividadId = "Actividad1"; // ¡Esto asegura que se envía!
    }
}
