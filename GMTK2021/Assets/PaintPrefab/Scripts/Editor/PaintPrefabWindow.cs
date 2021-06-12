using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PaintPrefabWindow : EditorWindow
{
    static bool BrushActive;

    [System.Serializable]
    public class BrushOptions
    {
        public BrushOptions()
        {
            CircleRadius = 1f;
            Density = 1;
            AngleX = 0f;
            AngleY = 0f;
            AngleZ = 0f;
        }

        public BrushOptions(BrushOptions brush)
        {
            Name = brush.Name;
            BrushObject = brush.BrushObject;

            SnapNormal = brush.SnapNormal;
            BuildOnTop = brush.BuildOnTop;
            CircleRadius = brush.CircleRadius;
            Density = brush.Density;
            PositionOffset = brush.PositionOffset;
            RotationOffset = brush.RotationOffset;

            RandomRotation = brush.RandomRotation;
            RandomScale = brush.RandomScale;
            MinScale = brush.MinScale;
            MaxScale = brush.MaxScale;
            AngleX = brush.AngleX;
            AngleY = brush.AngleY;
            AngleZ = brush.AngleZ;
        }

        public override bool Equals(object obj)
        {
            BrushOptions brushOpt = obj as BrushOptions;
            if (brushOpt == null)
                return false;

            if (Name != brushOpt.Name)
                return false;

            if (BrushObject != brushOpt.BrushObject)
                return false;

            if (SnapNormal != brushOpt.SnapNormal)
                return false;

            if (BuildOnTop != brushOpt.BuildOnTop)
                return false;

            if (CircleRadius != brushOpt.CircleRadius)
                return false;

            if (Density != brushOpt.Density)
                return false;

            if (RotationOffset != brushOpt.RotationOffset)
                return false;

            if (PositionOffset != brushOpt.PositionOffset)
                return false;

            if (RandomRotation != brushOpt.RandomRotation)
                return false;

            if (RandomScale != brushOpt.RandomScale)
                return false;

            if (MinScale != brushOpt.MinScale)
                return false;

            if (MaxScale != brushOpt.MaxScale)
                return false;

            if (AngleX != brushOpt.AngleX)
                return false;

            if (AngleY != brushOpt.AngleY)
                return false;

            if (AngleZ != brushOpt.AngleZ)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Name;
        public GameObject BrushObject;

        public bool SnapNormal;
        public bool BuildOnTop;

        public float CircleRadius;
        public int Density;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        public bool RandomRotation;
        public bool RandomScale;
        public float MinScale;
        public float MaxScale;
        public float AngleX;
        public float AngleY;
        public float AngleZ;
    }

    public PaintPrefabWindow()
    {
        activeOptions = new BrushOptions();
        pastOptions = new BrushOptions();
    }

    BrushOptions activeOptions;
    BrushOptions pastOptions;
    string customTagName = "PREFAB_PAINTER_TAG";
    bool isDirty = true;

    [MenuItem("Prefab Painter/Brush Tool")]
    static void Init()
    {
        var window = (PaintPrefabWindow)EditorWindow.GetWindow(typeof(PaintPrefabWindow));
        window.Show();
    }

    void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void OnGUI()
    {
        float margin = 5.0f;
        GUILayoutOption[] guiOptions = new GUILayoutOption[] { GUILayout.ExpandWidth(false) };

        GUILayout.BeginVertical();
        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab Painter", EditorStyles.boldLabel, guiOptions);
        if (isDirty)
        {
            EditorGUILayout.LabelField("(Some changes have not been saved !)", EditorStyles.miniLabel, guiOptions);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(margin * 3f);
        EditorGUILayout.LabelField("============ General Settings ===========", EditorStyles.boldLabel);
        GUILayout.Space(margin * 2f);
        GUILayout.BeginHorizontal();
        activeOptions.Name = EditorGUILayout.TextField("Brush Name", activeOptions.Name);
        GUILayout.EndHorizontal();
        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Selected GameObject");
        activeOptions.BrushObject = (GameObject)EditorGUILayout.ObjectField(activeOptions.BrushObject, typeof(GameObject), false, guiOptions);
        GUILayout.EndHorizontal();
        GUILayout.Space(margin * 3f);
        EditorGUILayout.LabelField("============ Brush Options =============", EditorStyles.boldLabel);
        GUILayout.Space(margin * 2f);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Brush Radius", guiOptions);
        activeOptions.CircleRadius = EditorGUILayout.Slider(activeOptions.CircleRadius, 0.1f, 10, guiOptions);
        GUILayout.EndHorizontal();
        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Brush Density", guiOptions);
        activeOptions.Density = (int)EditorGUILayout.Slider(activeOptions.Density, 1, 10, guiOptions);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(margin * 3f);
        EditorGUILayout.LabelField("============ Placing Options ============", EditorStyles.boldLabel);
        GUILayout.Space(margin * 2f);
        GUILayout.BeginHorizontal();
        activeOptions.BuildOnTop = EditorGUILayout.Toggle("Allow building on top", activeOptions.BuildOnTop, guiOptions);
        GUILayout.EndHorizontal();
        GUILayout.Space(margin * 2f);
        GUILayout.BeginVertical();
        if (!activeOptions.RandomRotation)
        {
            GUILayout.BeginHorizontal();
            activeOptions.SnapNormal = EditorGUILayout.Toggle("Snap to normal", activeOptions.SnapNormal, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
        }
        if (!activeOptions.SnapNormal)
        {
            GUILayout.BeginHorizontal();
            activeOptions.RandomRotation = EditorGUILayout.Toggle("Random Rotation", activeOptions.RandomRotation, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
        }
        if (activeOptions.RandomRotation)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Randomness on X Axis", guiOptions);
            activeOptions.AngleX = EditorGUILayout.Slider(activeOptions.AngleX, 0, 1, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Randomness on Y Axis", guiOptions);
            activeOptions.AngleY = EditorGUILayout.Slider(activeOptions.AngleY, 0, 1, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Randomness on Z Axis", guiOptions);
            activeOptions.AngleZ = EditorGUILayout.Slider(activeOptions.AngleZ, 0, 1, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(margin);
        }
        GUILayout.BeginHorizontal();
        activeOptions.RandomScale = EditorGUILayout.Toggle("Random Scale", activeOptions.RandomScale, guiOptions);
        GUILayout.EndHorizontal();
        if (activeOptions.RandomScale)
        {
            GUILayout.Space(margin);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Minimum Scale", guiOptions);
            activeOptions.MinScale = EditorGUILayout.IntField((int)activeOptions.MinScale, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Maximum Scale", guiOptions);
            activeOptions.MaxScale = EditorGUILayout.IntField((int)activeOptions.MaxScale, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(margin);
        }
        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        activeOptions.PositionOffset = EditorGUILayout.Vector3Field("Position Offset", activeOptions.PositionOffset);
        GUILayout.EndHorizontal();
        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        activeOptions.RotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", activeOptions.RotationOffset);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(margin * 4f);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (!BrushActive)
        {
            if (GUILayout.Button("Enable Brush"))
            {
                BrushActive = true;
                if (!activeOptions.BuildOnTop)
                {
                    Debug.Log("In order to avoid building prefabs in top of each other, they will be tagged properly.");
                    Debug.Log("If you don't want the script to modify your prefabs tags, please check \"Allow building on top\".");
                    Debug.Log("Note that this option might end up in creating models on top of each other, if they have a collider.");
                }
            }
        }
        else
        {
            if (GUILayout.Button("Disable Brush"))
            {
                BrushActive = false;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Brush"))
        {
            SaveBrush();
        }
        if (GUILayout.Button("Load Brush"))
        {
            LoadBrush();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndVertical();

        DetermineIfDirty();
        Repaint();
    }

    void DetermineIfDirty()
    {
        if (!activeOptions.Equals(pastOptions))
            isDirty = true;

        pastOptions = new BrushOptions(activeOptions);
    }

    private void OnDestroy()
    {
        if (isDirty)
        {
            if (EditorUtility.DisplayDialog("Save current brush ?", "Your current brush contains unsaved changes. Do you wish to save it ?", "Yes", "No"))
            {
                SaveBrush();
            }
        }
    }

    void OnSceneGUI(SceneView view)
    {
        if (!BrushActive)
        {
            return;
        }

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); // We remove scene selection while a brush is active; GET CRAZY
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Handles.color = Color.white;
            Handles.DrawWireDisc(hit.point, hit.normal, activeOptions.CircleRadius);

            if (hit.transform.tag == customTagName && !activeOptions.BuildOnTop)
                return;

            if (Event.current.type == EventType.MouseDown)
            {
                PositionObjectProperly(hit);
            }
            if (Event.current.type == EventType.MouseDrag)
            {
                PositionObjectProperly(hit);
            }
        }

        SceneView.RepaintAll();
    }

    void PositionObjectProperly(RaycastHit hit)
    {
        for (int i = 0; i < activeOptions.Density; i++)
        {
            Vector3 point = RandomPointOnXZCircle(hit.point, hit.normal.normalized, activeOptions.CircleRadius);

            RaycastHit trueHit;
            if (Physics.Raycast(point + 0.01f * hit.normal, -hit.normal, out trueHit))
            {
                float dotProd = Vector3.Dot(trueHit.normal.normalized, hit.normal);
                if (dotProd > 1 + 0.1f || dotProd < 1 - 0.1f)
                {
                    continue;
                }
                var obj = Instantiate(activeOptions.BrushObject);
                SnapObjectToNormalOrNot(hit, point, obj);

                if (!activeOptions.BuildOnTop)
                    HandleTagCreationAndPlacing(obj);

                if (activeOptions.RandomRotation)
                {
                    float finalAngleX = activeOptions.AngleX * 180;
                    float finalAngleY = activeOptions.AngleY * 360;
                    float finalAngleZ = activeOptions.AngleZ * 180;

                    Vector3 obtainedRotation = new Vector3(UnityEngine.Random.Range(0, finalAngleX), UnityEngine.Random.Range(0, finalAngleY), UnityEngine.Random.Range(0, finalAngleZ)); ;
                    Quaternion finalRotation = Quaternion.Euler(obtainedRotation);
                    obj.transform.rotation = finalRotation;
                }

                if (activeOptions.RotationOffset != Vector3.zero)
                    obj.transform.rotation *= Quaternion.Euler(activeOptions.RotationOffset);
            }
        }
    }

    void HandleTagCreationAndPlacing(GameObject obj)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // First check if it is not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(customTagName)) { found = true; break; }
        }

        // if not found, add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = customTagName;
        }

        tagManager.ApplyModifiedProperties();

        obj.tag = customTagName;
    }

    void SaveBrush()
    {
        BrushActive = false;
        string brushAsJson = JsonUtility.ToJson(activeOptions);
        var path = EditorUtility.SaveFilePanel("Save Active Brush", "Assets/PaintPrefab/Brushes", activeOptions.Name == "" ? "brush_" + Guid.NewGuid().ToString() : activeOptions.Name, "OPBrush");
        if (path.Length != 0)
        {
            File.WriteAllText(path, brushAsJson);
            isDirty = false;
        }
    }

    void LoadBrush()
    {
        BrushActive = false;

        if (isDirty)
        {
            if (EditorUtility.DisplayDialog("Save current brush ?", "Your current brush contains unsaved changes. Do you wish to save it ?", "Yes", "No"))
            {
                SaveBrush();
            }
        }

        string pathToBrush = EditorUtility.OpenFilePanel("Load Brush File", "Assets/PaintPrefab/Brushes", "OPBrush");
        if (pathToBrush.Length != 0)
        {
            string brushAsJson = File.ReadAllText(pathToBrush);
            activeOptions = JsonUtility.FromJson<BrushOptions>(brushAsJson);
            isDirty = false;
            pastOptions = new BrushOptions(activeOptions);
        }
    }

    void SnapObjectToNormalOrNot(RaycastHit hit, Vector3 point, GameObject obj)
    {
        obj.transform.position = point + activeOptions.PositionOffset;


        if (activeOptions.RandomScale)
        {
            if (activeOptions.MaxScale < activeOptions.MinScale)
            {
                var temp = activeOptions.MaxScale;
                activeOptions.MaxScale = activeOptions.MinScale;
                activeOptions.MinScale = temp;
            }

            if (activeOptions.MaxScale == 0)
                activeOptions.MaxScale = 1;

            if (activeOptions.MinScale == 0)
                activeOptions.MinScale = 1;

            var randomScale = UnityEngine.Random.Range(activeOptions.MinScale, activeOptions.MaxScale);
            obj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }


        if (activeOptions.SnapNormal)
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
    }

    Vector3 RandomPointOnXZCircle(Vector3 hitpoint, Vector3 normal, float radius)
    {
        //Vector2 hitAsV2 = new Vector2(hitpoint.x, hitpoint.z);
        Vector2 tempPoint = UnityEngine.Random.insideUnitCircle * radius;
        var surfaceRot = Quaternion.FromToRotation(Vector3.up, normal);
        return surfaceRot*new Vector3(tempPoint.x, 0.0f, tempPoint.y)+hitpoint;
    }
}
