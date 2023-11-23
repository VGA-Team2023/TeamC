using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Reflection.Emit;

[CustomEditor(typeof(GameManager))]
public class GameManagerPrefabEditor : Editor
{
    GameManager _target;
    SerializedProperty _slowManagerProperty;
    SerializedProperty _timeManagerProperty;
    private void OnEnable()
    {
        _target = target as GameManager;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        using (new GUILayout.VerticalScope(GUILayout.Height(115f)))
        {
            EditorGUILayout.BeginVertical();
            
            Rect r = EditorGUILayout.GetControlRect();
            //値をロードする
            serializedObject.Update();
            //プロパティを取得する
            _slowManagerProperty = serializedObject.FindProperty("_slowManager");
            EditorGUI.BeginProperty(r, GUIContent.none, _slowManagerProperty);
            using (new EditorGUI.PropertyScope(r, GUIContent.none, _slowManagerProperty))
            {
                _slowManagerProperty.isExpanded = EditorGUI.Foldout(r, _slowManagerProperty.isExpanded, new GUIContent("SlowManager"));
                if (_slowManagerProperty.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(r, _slowManagerProperty.FindPropertyRelative("_slowSpeedRate"));
                        r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        if (GUI.Button(r, new GUIContent("PrefabLoad")))
                        {
                            PrefabLoad(_slowManagerProperty);
                        }
                    }
                    
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
            r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUILayout.BeginVertical();
            _timeManagerProperty = serializedObject.FindProperty("_timeManager");
            using (new EditorGUI.PropertyScope(r, GUIContent.none, _timeManagerProperty))
            {
                _timeManagerProperty.isExpanded = EditorGUI.Foldout(r, _timeManagerProperty.isExpanded, new GUIContent("TimeManager"));
                if (_timeManagerProperty.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(r, _timeManagerProperty.FindPropertyRelative("_gamePlayTime"));
                        r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        if (GUI.Button(r, new GUIContent("PrefabLoad")))
                        {
                            PrefabLoad(_timeManagerProperty);
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }

    void PrefabLoad(SerializedProperty property)
    {
        //シーン内でインスタンスされているオブジェクトのPrefabのパスを取得
        string assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_target.gameObject);
        //Prefabのプロパティに反映
        PrefabUtility.ApplyPropertyOverride(property, assetPath, InteractionMode.UserAction);
    }
}
