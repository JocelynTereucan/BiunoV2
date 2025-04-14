using SqliteForUnity3D;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;


public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection _connection;

    void Start()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "mydatabase.db");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection.CreateTable<ZonaEvaluacion>();
        Debug.Log("Tabla creada exitosamente.");

        if (!_connection.Table<ZonaEvaluacion>().Any())
        {
            _connection.Insert(new ZonaEvaluacion
            {
                nombre = "Competente",
                xMin = 98.21f,
                xMax = 681.78f,
                yMin = 237.54f,
                yMax = 400f
            });

            _connection.Insert(new ZonaEvaluacion
            {
                nombre = "Inefectivo",
                xMin = 98.21f,
                xMax = 681.78f,
                yMin = -313.54f,
                yMax = 237.54f
            });

            _connection.Insert(new ZonaEvaluacion
            {
                nombre = "Deficiente",
                xMin = 0f,
                xMax = 1920f,
                yMin = -1080f,
                yMax = -313.54f
            });

            _connection.Insert(new ZonaEvaluacion
            {
                nombre = "Cuestionable",
                xMin = 0f,
                xMax = 1920f,
                yMin = 400f,
                yMax = 1080f
            });

            Debug.Log("Zonas insertadas correctamente.");
        }
        else
        {
            Debug.Log("La tabla ya contiene datos. No se insertaron duplicados.");
            ActualizarZonas();
        }
    }

    public class ZonaEvaluacion
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string nombre { get; set; }
        public float xMin { get; set; }
        public float xMax { get; set; }
        public float yMin { get; set; }
        public float yMax { get; set; }
    }
    public void EvaluarPosicion(GameObject cubo)
    {
        RectTransform rect = cubo.GetComponent<RectTransform>();
        Vector2 pos = rect.anchoredPosition;
        Vector2 size = rect.sizeDelta;

        float left = pos.x - size.x / 2f;
        float right = pos.x + size.x / 2f;
        float top = pos.y + size.y / 2f;
        float bottom = pos.y - size.y / 2f;


        Debug.Log($"[DEBUG] Rectángulo del cubo: L:{left}, R:{right}, T:{top}, B:{bottom}");

        var zonas = _connection.Table<ZonaEvaluacion>().ToList();

        string zonaGanadora = "NINGUNA";
        float mayorSolapamiento = 0;

        string detalles = $"📦 Rectángulo del cubo:\nL: {left}\nR: {right}\nT: {top}\nB: {bottom}\n\n";
        foreach (var zona in zonas)
        {
            float xSolapado = Mathf.Max(0, Mathf.Min(right, zona.xMax) - Mathf.Max(left, zona.xMin));
            float ySolapado = Mathf.Max(0, Mathf.Min(top, zona.yMax) - Mathf.Max(bottom, zona.yMin));
            float areaSolapada = xSolapado * ySolapado;

            Debug.Log($"[DEBUG] Solapamiento con {zona.nombre}: {areaSolapada}");
            detalles += $"📐 Solapamiento con {zona.nombre}: {areaSolapada}\n";
            if (areaSolapada > mayorSolapamiento)
            {
                mayorSolapamiento = areaSolapada;
                zonaGanadora = zona.nombre.ToUpper();
            }
        }
        detalles += $"\n✅ Evaluación final: {zonaGanadora}";
        ResultadoEvaluacion.ZonaFinal = zonaGanadora;
        ResultadoEvaluacion.DetallesEvaluacion = detalles;
        Debug.Log($"✅ Evaluación: {zonaGanadora}");
    }

    private Vector2 ObtenerPosicionEnCanvas(Vector3 posicionMundo)
    {
        Vector2 pantalla = RectTransformUtility.WorldToScreenPoint(Camera.main, posicionMundo);
        return pantalla;
    }
    public class MoverCubo : MonoBehaviour
    {
        public DatabaseManager dbManager;

        private void OnMouseUp() // cuando se suelta el clic del mouse
        {
            dbManager.EvaluarPosicion(this.gameObject);
        }
    }
    void ActualizarZonas()
    {
        var zonas = new List<ZonaEvaluacion>
        {
            new ZonaEvaluacion
            {
                 nombre = "Competente",
                xMin = 98.21f,
                xMax = 681.78f,
                yMin = 237.54f,
                yMax = 400f
            },
            new ZonaEvaluacion
            {
                nombre = "Cuestionable",
                xMin = 0f,
                xMax = 1920f,
                yMin = 400f,
                yMax = 1080f
            },
            new ZonaEvaluacion
            {
                nombre = "Inefectivo",
                xMin = 98.21f,
                xMax = 681.78f,
                yMin = -313.54f,
                yMax = 237.54f
            },
            new ZonaEvaluacion
            {
                xMin = 0f,
                xMax = 1920f,
                yMin = -1080f,
                yMax = -313.54f
            }
        };

        foreach (var zona in zonas)
        {
            var existente = _connection.Table<ZonaEvaluacion>().FirstOrDefault(z => z.nombre == zona.nombre);
            if (existente != null)
            {
                existente.xMin = zona.xMin;
                existente.xMax = zona.xMax;
                existente.yMin = zona.yMin;
                existente.yMax = zona.yMax;
                _connection.Update(existente);
                Debug.Log($"✅ Zona actualizada: {zona.nombre}");
            }
            else
            {
                _connection.Insert(zona);
                Debug.Log($"➕ Zona insertada: {zona.nombre}");
            }
        }
    }

}
