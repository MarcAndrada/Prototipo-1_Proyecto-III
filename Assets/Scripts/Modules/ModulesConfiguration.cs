using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "ModulesConfiguration", menuName = "Scriptable Objects/Modules Configuration")]
public class ModulesConfiguration : ScriptableObject
{
    [field: SerializeField]
    public int width {  get; private set; }
    [field: SerializeField]
    public int height { get; private set; }

    [Space, SerializedDictionary("Position", "Object")]
    public SerializedDictionary<Vector2Int, GameObject> modulesPositions;

}
