using UnityEngine;

[CreateAssetMenu(fileName = "InteractableObject", menuName = "Scriptable Objects/InteractableObject")]
public class InteractableObjectScriptable : ScriptableObject
{
    public Transform prefab;
    public string objectName;
}
