#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;



[CustomEditor(typeof(FadeToScene))]
public class FadeToSceneScriptEditor : Editor
{

    override public void OnInspectorGUI()
    {
        //FadeToScene
        var FTS = target as FadeToScene;

        FTS.tarFogStrength = EditorGUILayout.FloatField("Target Fog Strength", FTS.tarFogStrength);
        FTS.fadeSpeed = EditorGUILayout.FloatField("Fog Speed", FTS.fadeSpeed);

        FTS.fadeTrue = EditorGUILayout.Toggle("Play On Awake", FTS.fadeTrue);

        FTS.restartScene = EditorGUILayout.Toggle("Restart Scene", FTS.restartScene);

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(FTS.restartScene)))
        {
            if (group.visible == false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Target Scene Number");
                FTS.tarSceneNumber = EditorGUILayout.IntField(FTS.tarSceneNumber);
                EditorGUI.indentLevel--;
            }
        }
    }
}
#endif