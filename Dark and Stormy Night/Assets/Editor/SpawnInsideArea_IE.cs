#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;



[CustomEditor(typeof(SpawnInsideArea))]
public class SpawnInsideAreaScriptEditor : Editor
{

    override public void OnInspectorGUI()
    {
        //SpawnInsideArea
        var SIA = target as SpawnInsideArea;

        SIA.areaType = (SpawnInsideArea.area)EditorGUILayout.EnumPopup("Spawn Area Type", SIA.areaType);

        switch (SIA.areaType)
        {
            case SpawnInsideArea.area.box:
                EditorGUI.indentLevel++;
                SIA.pointMin = EditorGUILayout.Vector3Field("Min Vector 3", SIA.pointMin);
                SIA.pointMax = EditorGUILayout.Vector3Field("Max Vector 3", SIA.pointMax);
                EditorGUI.indentLevel--;
                break;
            case SpawnInsideArea.area.sphere:
                EditorGUI.indentLevel++;
                SIA.point = EditorGUILayout.ObjectField("Origin Point", SIA.point, typeof(Transform), true) as Transform;
                SIA.radius = EditorGUILayout.FloatField("Radius", SIA.radius);
                EditorGUI.indentLevel--;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }

        EditorGUILayout.Space();

        SIA.spawnRateMax = EditorGUILayout.Vector2Field("Spawn Rate Bounds", SIA.spawnRateMax);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawn Objects");
        EditorGUI.indentLevel++;
        var list = SIA.spawnObjects;
        int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Amount", list.Count));
        while (newCount < list.Count)
            list.RemoveAt(list.Count - 1);
        while (newCount > list.Count)
            list.Add(null);

        for (int i = 0; i < list.Count; i++)
        {
            EditorGUI.indentLevel++;
            list[i] = (GameObject)EditorGUILayout.ObjectField(list[i], typeof(GameObject), false);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }
}

#endif