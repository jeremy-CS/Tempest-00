//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//
using UnityEditor;
using UnityEngine;
namespace Tools.PrefabPainter
{
    public class BrushToolWindow : EditorWindow
    {
        //List of Prefab to paint
        private GameObject prefabToPaint;

        [MenuItem("Tools/Brush Tool")]
        public static void ShowWindow()
        {
            GetWindow<BrushToolWindow>("Brush Tool");
        }

        private void OnGUI()
        {
            var settings = BrushToolSettings.instance;

            GUILayout.Label("Brush Tool", EditorStyles.boldLabel);

            //Creating Inspector Fields for Brush Tool Settings
            using (new GUILayout.VerticalScope("box"))
            {
                settings.Enabled = EditorGUILayout.ToggleLeft("Enable Painting", settings.Enabled);

                settings.PrefabToPaint = (GameObject)EditorGUILayout.ObjectField(
                    "Prefab to Paint",
                    settings.PrefabToPaint,
                    typeof(GameObject),
                    false
                    );

                settings.Mode = (BrushToolSettings.PlacementMode)EditorGUILayout.EnumPopup("Placement Mode", settings.Mode);

                if (settings.Mode == BrushToolSettings.PlacementMode.Grid)
                {
                    settings.GridSize = Mathf.Max(0.0001f, EditorGUILayout.FloatField("Grid Size", settings.GridSize));
                    settings.GridOrigin = EditorGUILayout.Vector3Field("Grid Origin", settings.GridOrigin);
                }

                EditorGUILayout.Space(6);

                settings.Use2D = EditorGUILayout.Toggle("Force 2D Placement", settings.Use2D);

                settings.AlignToSurfaceNormal = EditorGUILayout.Toggle("Align To Surface Normal (3D)", settings.AlignToSurfaceNormal);
                if (settings.AlignToSurfaceNormal)
                    settings.NormalOffset = EditorGUILayout.FloatField("Normal Offset", settings.NormalOffset);

                settings.RandomYaw = EditorGUILayout.Toggle("Random Yaw (3D)", settings.RandomYaw);

                settings.Parent = (Transform)EditorGUILayout.ObjectField("Parent (optional)", settings.Parent, typeof(Transform), true);

                EditorGUILayout.Space(6);

                settings.LayerMask3D = LayerMaskField("3D Layer Mask", settings.LayerMask3D);
                settings.LayerMask2D = LayerMaskField("2D Layer Mask", settings.LayerMask2D);
            }

            if (!settings.Enabled)
            {
                EditorGUILayout.HelpBox("Enable Painting to paint in the Scene view.", MessageType.Info);
            }
            else if (settings.PrefabToPaint == null)
            {
                EditorGUILayout.HelpBox("Assing a Prefab to paint.", MessageType.Warning);
            }

            // Persist settings edits
            if (GUI.changed) settings.SaveSettings();
        }

        private static LayerMask LayerMaskField(string label, LayerMask selected)
        {
            //Unity doesn't have a perfect built-in LayerMask field, this is a common approach:
            var layers = UnityEditorInternal.InternalEditorUtility.layers;
            int mask = selected.value;

            mask = EditorGUILayout.MaskField(label, mask, layers);
            selected.value = mask;
            return selected;
        }
    }
}