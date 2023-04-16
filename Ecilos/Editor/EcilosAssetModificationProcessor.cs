using Ecilos.Ipfs;
using System;
using UnityEditor;
using UnityEngine;

public class EcilosAssetModificationProcessor : UnityEditor.AssetModificationProcessor {
  internal static bool IsEnabled { get; set; }
  internal const string META_EXTENSION = ".meta";
  // Define a custom struct.
  [System.Serializable]
  public struct UserData {
    public string cid;
    public string hash;
    public string guid;
  }

  public static string GetMetaPath(string path) {
    return string.Concat(path, META_EXTENSION);
  }

  public static string GetPathFromMetaPath(string path) {
    return path.Substring(0, path.Length - META_EXTENSION.Length);
  }

  public static bool IsMetaPath(string path) {
    return path.EndsWith(META_EXTENSION);
  }

  // Unity calls this method when it is about to create an Asset you haven't imported.
  static void OnWillCreateAsset(string path) {
    if (IsMetaPath(path)) {
      ProcessMetaAsset(path);
      Debug.Log("OnWillCreateAsset: " + path);
    }
  }

  // This is called by Unity when it is about to write serialized assets or Scene files to disk.
  static string[] OnWillSaveAssets(string[] paths) {
    foreach (string path in paths) {
      Debug.Log("OnWillSaveAssets: " + path);
      if (IsMetaPath(path)) {
        ProcessMetaAsset(path);
      } else {
        if (ProcessAsset(path)) {
          UploadAsset(path);
        }
      }
    }
    return paths;
  }

  // Unity calls this method when it is about to move an Asset on disk.
  private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {
    Debug.Log("Source path: " + sourcePath + ". Destination path: " + destinationPath + ".");
    AssetMoveResult assetMoveResult = AssetMoveResult.DidNotMove;

    // Perform operations on the asset and set the value of 'assetMoveResult'
    // accordingly.

    return assetMoveResult;
  }

  private static bool ProcessAsset(string path) {
    bool isSupported = false;
    Type assetType = AssetDatabase.GetMainAssetTypeAtPath(path);

    isSupported |= assetType == typeof(Material);
    if (!isSupported) {
      return isSupported;
    }

    if (path.EndsWith(".prefab") && assetType == typeof(GameObject)) {
      // var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
      // PrefabUtility.SavePrefabAsset(prefab);
      return isSupported;
    }

    Hash128 hash = AssetDatabase.GetAssetDependencyHash(path);
    // string metaPath = AssetDatabase.GetTextMetaFilePathFromAssetPath(AssetDatabase.GetAssetPath(path));
    var guid = AssetDatabase.AssetPathToGUID(path);
    // if (!String.IsNullOrEmpty(guid)) {}

    // Get the import settings SerializedObject of the asset.
    SerializedObject importSettings = new SerializedObject(AssetImporter.GetAtPath(path));

    // Convert the serialized object to a JSON string
    string jsonString = JsonUtility.ToJson(importSettings);
    Debug.Log(jsonString);

    // Create an instance of the struct and set some values.
    UserData myData = new UserData {
      cid = "ABCD",
      guid = AssetDatabase.AssetPathToGUID(path),
      hash = hash.ToString(),
    };

    // Modify the metadata of the material asset.
    importSettings.Update();
    importSettings.FindProperty("m_UserData").stringValue = JsonUtility.ToJson(myData);

    // Apply the modifications to the import settings.
    importSettings.ApplyModifiedProperties();
    EditorUtility.SetDirty(importSettings.targetObject);

    // Add a new key-value item to the meta file.
    // metaFile.CustomData.SetString("foo", "bar");
    // File.SetAttributes(metaPath, FileAttributes.Normal);
    // AssetDatabase.StartAssetEditing();
    // File.WriteAllLines(metaPath, meta);
    // Ensure the AssetDatabase knows we're finished editing
    // AssetDatabase.StopAssetEditing();
    return isSupported;
  }

  private static void ProcessMetaAsset(string path) {
    string assetPath = GetPathFromMetaPath(path);

    // Load the .meta file as a TextAsset.
    TextAsset metaFile = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
    Debug.Log("metaFile: " + metaFile);

    // Create a SerializedObject for the .meta file
    // SerializedObject serializedMetaFile = new SerializedObject(metaFile);
  }

  private static void UploadAsset(string path) {
    Ecilos.Ipfs.EcilosIpfsUploader.UploadFile(path);
  }

  private string GetDebuggerDisplay() {
    return ToString();
  }
}