using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EcilosAssetProcessor : AssetPostprocessor {
  // Define a custom struct.
  [System.Serializable]
  public struct MyData {
    public string cid;
    public string hash;
    public string guid;
  }

  static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                                     string[] movedFromAssetPaths, bool didDomainReload) {
    bool isModified = false;
    foreach (string str in importedAssets) {
      if (str.EndsWith(".mat")) {
        Debug.Log("Reimported Asset: " + str);
        Hash128 hash = AssetDatabase.GetAssetDependencyHash(str);
        Material matFile = AssetDatabase.LoadAssetAtPath<Material>(str);
        var guid = AssetDatabase.AssetPathToGUID(str);
        // if (!String.IsNullOrEmpty(guid)) {}

        // Get the import settings SerializedObject of the asset.
        SerializedObject importSettings = new SerializedObject(AssetImporter.GetAtPath(str));

        // Convert the serialized object to a JSON string
        string jsonString = JsonUtility.ToJson(importSettings);
        Debug.Log(jsonString);

        // Create an instance of the struct and set some values.
        MyData myData = new MyData {
          cid = "ABCD2",
          guid = AssetDatabase.AssetPathToGUID(str),
          hash = hash.ToString(),
        };

        // Modify the metadata of the material asset.
        importSettings.FindProperty("m_UserData").stringValue = JsonUtility.ToJson(myData);

        // Apply the modifications to the import settings.
        importSettings.Update();
        importSettings.ApplyModifiedProperties();
        EditorUtility.SetDirty(importSettings.targetObject);
        isModified = true;

        /*
                        // Get the path of the texture's .meta file via asset.
                        //string metaPath =
           AssetDatabase.GetTextMetaFilePathFromAssetPath(AssetDatabase.GetAssetPath(str)); string metaPath = str +
           ".meta"; Debug.Log("Meta file: " + metaPath);

                        // Load the .meta file as a TextAsset.
                        TextAsset metaFile = AssetDatabase.LoadAssetAtPath<TextAsset>(metaPath);

                        // Create a SerializedObject for the .meta file
                        SerializedObject serializedMetaFile = new SerializedObject(metaFile);

                        // Add a new key-value item to the meta file.
                        serializedMetaFile.FindProperty("customData").stringValue = "Foo: Bar";

                        // Apply the modified properties to the .meta file.
                        serializedMetaFile.ApplyModifiedProperties();
                        */

        // Add a new key-value item to the meta file.
        // metaFile.CustomData.SetString("foo", "bar");
        // File.SetAttributes(metaPath, FileAttributes.Normal);
        // AssetDatabase.StartAssetEditing();
        // File.WriteAllLines(metaPath, meta);
        // Ensure the AssetDatabase knows we're finished editing
        // AssetDatabase.StopAssetEditing();
      }
    }
    foreach (string str in deletedAssets) {
      Debug.Log("Deleted Asset: " + str);
    }

    for (int i = 0; i < movedAssets.Length; i++) {
      Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
    }

    if (didDomainReload) {
      Debug.Log("Domain has been reloaded");
    }
    if (isModified) {
      AssetDatabase.SaveAssets();
    }
    Debug.Log(
        $"\nimportedAssets({importedAssets.Length}):\n\t-{importedAssets.Length>0}{string.Join( ",\n\t-", importedAssets)}" +
        $"\ndeletedAssets({deletedAssets.Length}):\n\t-{string.Join( ",\n\t-", deletedAssets )}" +
        $"\nmovedAssets({movedAssets.Length}):\n\t-{string.Join( ",\n\t-", movedAssets )}" +
        $"\nmovedFromAssetPaths({movedFromAssetPaths.Length}):\n\t-{string.Join( ",\n\t-", movedFromAssetPaths )}");
  }

  void OnPreprocessModel() {
    if (assetPath.Contains("@")) {
      ModelImporter modelImporter = assetImporter as ModelImporter;
      modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
    } else {
      Debug.Log("Preprocessing file: " + assetPath);
    }
  }

  public static string guidToPath(string guid) {
    return AssetDatabase.GUIDToAssetPath(guid);
  }

  public static string pathToGUID(string path) {
    return AssetDatabase.AssetPathToGUID(path);
  }

  static void Startup() {
    Debug.Log("Up and running");
  }

  static void Update() {
    Debug.Log("Updating");
  }
}
