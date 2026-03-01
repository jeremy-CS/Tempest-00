//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//
using UnityEditor;
using UnityEngine;
namespace Tools.PrefabPainter
{
    [InitializeOnLoad]
    public static class BrushToolHandler
    {
        static BrushToolHandler()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var settings = BrushToolSettings.instance;
            //Sanity check
            if (!settings.Enabled || settings.PrefabToPaint == null) return;

            Event e = Event.current;

            // Don't break scene navigation
            if (e.alt) return;

            // Repaint scene view to accurately show ghost preview
            sceneView.Repaint();

            //Show a subtle hint that tool is armed
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10,10,260,40), GUI.skin.box);
            GUILayout.Label("Brush Tool: ON (LMB to paint)", EditorStyles.miniLabel);
            GUILayout.EndArea();
            Handles.EndGUI();

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            bool validHit = false;
            bool want2D = settings.Use2D || sceneView.in2DMode;
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            if (want2D)
            {
                //Reliable editor-friendly 2D pick
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, settings.LayerMask2D.value);
                if (hit.collider != null)
                {
                    validHit = true;

                    pos = ApplySnappingIfNeeded(hit.point, settings);
                }
            }
            else //want3D
            {
                if (Physics.Raycast(ray, out RaycastHit hit3D, Mathf.Infinity, settings.LayerMask3D.value))
                {
                    validHit = true;

                    pos = ApplySnappingIfNeeded(hit3D.point, settings);

                    if (settings.AlignToSurfaceNormal)
                    {
                        // Align prefab "up" with the surface normal
                        rot = Quaternion.FromToRotation(Vector3.up, hit3D.normal);
                        pos += hit3D.normal * settings.NormalOffset;
                    }

                    if (settings.RandomYaw)
                    {
                        // Random yaw around world up (simple + predictable)
                        rot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * rot;
                    }
                }
            }

            //----- Ghost Preview
            if (validHit)
            {
                var bounds = GetPrefabBounds(settings.PrefabToPaint);
                Handles.color = Color.cyan;

                if (want2D)
                {
                    Draw2DPreviewBounds(bounds, pos);
                }
                else
                {
                    Handles.matrix = Matrix4x4.TRS(pos, rot, Vector3.zero);
                    Handles.DrawWireCube(bounds.center, bounds.size);
                    Handles.matrix = Matrix4x4.identity;
                }
            }

            //----- Prefab Placement
            if (validHit && e.type == EventType.MouseDown && e.button == 0)
            {
                // Prevent default object selection when painting
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                
                //Create and Place prefab in scene
                PlacePrefab(pos, rot, settings);
                e.Use();
            }
        }

        //----- SNAPPING IMPLEMENTATION
        private static Vector3 ApplySnappingIfNeeded(Vector3 pos, BrushToolSettings settings)
        {
            if (settings.Mode != BrushToolSettings.PlacementMode.Grid) return pos;

            float grid = settings.GridSize;
            Vector3 origin = settings.GridOrigin;

            Vector3 snappedPosition = pos - origin;
            snappedPosition.x = Mathf.Round(snappedPosition.x / grid) * grid;
            snappedPosition.y = Mathf.Round(snappedPosition.y / grid) * grid;
            snappedPosition.z = Mathf.Round(snappedPosition.z / grid) * grid;

            return origin + snappedPosition;
        }

        //----- PLACING PREFAB IN SCENE
        private static void PlacePrefab(Vector3 position, Quaternion rotation, BrushToolSettings settings)
        {
            GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(settings.PrefabToPaint);
            Undo.RegisterCreatedObjectUndo(placed, "Brush Paint Prefab");

            if (settings.Parent != null) placed.transform.SetParent(settings.Parent, worldPositionStays: true);

            placed.transform.SetLocalPositionAndRotation(position, rotation);
            Selection.activeGameObject = placed;
        }

        //----- GHOST PREVIEW IMPLEMENTATION
        private static Bounds? _cachedPrefabBounds;
        private static GameObject _cachedPrefab;
        private static Bounds GetPrefabBounds(GameObject prefab)
        {
            // Recompute if prefab changed
            if (_cachedPrefab != prefab || _cachedPrefabBounds == null)
            {
                _cachedPrefab = prefab;

                // Create a temporary instance (not saved) to measure bounds
                var temp = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                temp.hideFlags = HideFlags.HideAndDontSave;

                //Info initialization
                bool hasAny = false;
                bool used2D = false;
                Bounds bounds = new(Vector3.zero, Vector3.zero);

                //2D -> Priority when checking renderers' bounds
                var spriteRenderers = temp.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sr in spriteRenderers)
                {
                    // Convert spriteRenderer.LocalBounds into temp root local space
                    var lb = sr.localBounds;
                    var t = sr.transform;

                    // Center in root-local space
                    Vector3 centerRootLocal = temp.transform.InverseTransformPoint(t.TransformPoint(lb.center));

                    // Size is trickier with rotation, but for 2D sprites, there's usually no rotations on children;
                    // Assume axis-aligned for fast + stable editor preview;
                    Vector3 sizeRootLocal = Vector3.Scale(lb.size, t.lossyScale);

                    var converted = new Bounds(centerRootLocal, sizeRootLocal);
                    if (!hasAny) { bounds = converted; hasAny = true; used2D = true; }
                    else bounds.Encapsulate(converted);
                }

                //3D -> Skipped if Sprite Renderers found on prefab
                if (!hasAny)
                {
                    var renderers = temp.GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        var b = renderer.bounds;
                        if (!hasAny) { bounds = b; hasAny = true; }
                        else bounds.Encapsulate(b);
                    }
                }

                //Fallback: No renderers found on prefab object
                if (!hasAny) bounds = new Bounds(temp.transform.position, Vector3.one * 0.5f);

                Object.DestroyImmediate(temp);

                //Store bounds in prefab local space-ish: use size only, centered at 0
                _cachedPrefabBounds = used2D ? bounds : new Bounds(Vector3.zero, bounds.size);
            }

            return _cachedPrefabBounds.Value;
        }
        private static void Draw2DPreviewBounds(Bounds b, Vector3 pos)
        {
            // Draw a rectangle in XY plane (ignore Z)
            Vector3 c = pos + b.center;

            float halfX = b.size.x * 0.5f;
            float halfY = b.size.y * 0.5f;

            Vector3 p1 = new(c.x - halfX, c.y - halfY, c.z);
            Vector3 p2 = new(c.x - halfX, c.y + halfY, c.z);
            Vector3 p3 = new(c.x + halfX, c.y + halfY, c.z);
            Vector3 p4 = new(c.x + halfX, c.y - halfY, c.z);

            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);
            Handles.DrawLine(p3, p4);
            Handles.DrawLine(p4, p1);
        }
    }
}