using UnityEngine;
using UnityEditor;
using Core.UI;

[CustomEditor(typeof(TouchUIElement))]
public class TouchUIElementEditor : JoystickEditor
{
    private SerializedProperty siblings;
    protected override void OnEnable()
    {
        base.OnEnable();
        siblings = serializedObject.FindProperty("_siblingElements");
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(siblings, new GUIContent("Sibling elements", "The directions of sibling elements."));
    }
}
