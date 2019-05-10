#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;



[CustomEditor(typeof(FadeToScene))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as FadeToScene;

        myScript.tarFogStrength = EditorGUILayout.FloatField("Target Fog Strength", myScript.tarFogStrength);
        myScript.fadeSpeed = EditorGUILayout.FloatField("Fog Speed", myScript.fadeSpeed);

        myScript.fadeTrue = EditorGUILayout.Toggle("Play On Awake", myScript.fadeTrue);

        myScript.restartScene = EditorGUILayout.Toggle("Restart Scene", myScript.restartScene);

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.restartScene)))
        {
            if (group.visible == false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Target Scene Number");
                myScript.tarSceneNumber = EditorGUILayout.IntField(myScript.tarSceneNumber);
                EditorGUI.indentLevel--;
            }
        }
    }
}
#endif