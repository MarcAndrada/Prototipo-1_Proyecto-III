using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "ModulesConfiguration", menuName = "Scriptable Objects/Modules Configuration")]
public class ModulesConfiguration : ScriptableObject
{
    [field: SerializeField, Tooltip("Cantidad de modulos que habra a lo ancho")]
    public int Width {  get; private set; }
    [field: SerializeField, Tooltip("Cantidad de modulos que habra a lo alto")]
    public int Height { get; private set; }

    [field: SerializeField, Tooltip("Este sera la distancia entre cada modulo, sera igual tanto por X como por Y")]
    public float ModuleOffset { get; private set; }

    [Space, SerializedDictionary("Position", "Object"), Tooltip("Esto relaciona la posicion del modulo con el objeto que habra en la casilla")]
    public SerializedDictionary<Vector2Int, GameObject> ModulesPositions;
    
    public void ExpandGridRight()
    {
        Width++;
    }

    public void ExpandGridLeft()
    {
        // Desplazar todos los módulos existentes una columna hacia la derecha
        var newPositions = new SerializedDictionary<Vector2Int, GameObject>();

        foreach (var entry in ModulesPositions)
        {
            Vector2Int newPosition = new Vector2Int(entry.Key.x + 1, entry.Key.y);
            newPositions[newPosition] = entry.Value;
        }

        ModulesPositions = newPositions;
        Width++;
    }
}
