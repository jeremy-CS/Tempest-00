//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//
using UnityEditor;
using UnityEngine;
namespace Tools.PrefabPainter
{
    //[FilePath("ProjectSettings/BrushToolSettings.asset", FilePathAttribute.Location.ProjectFolder)] //For tool settings per project editor (shared in repo)
    [FilePath("UserSettings/BrushToolSettings.asset", FilePathAttribute.Location.ProjectFolder)] //For tool settings per developper machine (not shared in repo)
    public sealed class BrushToolSettings : ScriptableSingleton<BrushToolSettings>
    {
        public enum PlacementMode { Free, Grid }

        //Basic fields
        [SerializeField] private bool enabled = false;
        [SerializeField] private GameObject prefabToPaint;
        //Placement mode
        [SerializeField] private PlacementMode mode = PlacementMode.Free;
        //Grid info
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private Vector3 gridOrigin = Vector3.zero;
        //Environment info
        [SerializeField] private bool use2D = false;
        //Align info
        [SerializeField] private bool alignToSurfaceNormal = false;
        [SerializeField] private float normalOffset = 0f;
        //Randomness info
        [SerializeField] private bool randomYaw = false;
        //LayerMask info
        [SerializeField] private LayerMask layerMask3D = ~0; //Everything
        [SerializeField] private LayerMask layerMask2D = ~0; //Everything
        
        [SerializeField] private Transform parent;

        //----- FIELD GETTERS/SETTERS
        public bool Enabled { get => enabled; set => enabled = value; }
        public GameObject PrefabToPaint { get => prefabToPaint; set => prefabToPaint = value; }

        public PlacementMode Mode { get => mode; set => mode = value; }

        public float GridSize { get => gridSize; set => gridSize = value; }
        public Vector3 GridOrigin { get => gridOrigin; set => gridOrigin = value; }

        public bool Use2D { get => use2D; set => use2D = value; }

        public bool AlignToSurfaceNormal { get => alignToSurfaceNormal; set => alignToSurfaceNormal = value; }
        public float NormalOffset { get => normalOffset; set => normalOffset = value; }

        public bool RandomYaw { get => randomYaw; set => randomYaw = value; }

        public LayerMask LayerMask3D { get => layerMask3D; set => layerMask3D = value; }
        public LayerMask LayerMask2D { get => layerMask2D; set => layerMask2D = value; }

        public Transform Parent { get => parent; set => parent = value; }

        //----- SAVE
        public void SaveSettings()
        {
            Save(true);
        }
    }
}