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
    
    public void ExpandWidth()
    {
        Width++;
    }

    public void ExpandHeight()
    {
        Height++;
    }
}
