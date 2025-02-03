using UnityEditor;
using UnityEngine;

namespace Swift_Blade.Audio
{
    [CustomEditor(typeof(AudioSO))]
    public class AudioSODrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            AudioSO audioSO = target as AudioSO;
            DrawDefaultInspector();
            if (audioSO.audioRolloffMode == AudioRolloffMode.Custom)
            {
                EditorGUILayout.LabelField("Animation Field", EditorStyles.boldLabel);
                audioSO.curve = EditorGUILayout.CurveField("Curve", audioSO.curve);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(audioSO);
            }

        }
    }
}
