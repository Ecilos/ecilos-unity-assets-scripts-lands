using UnityEditor;
using UnityEngine;

namespace Ecilos.Editor.Asset {
  public class EcilosAssetProcessor : AssetPostprocessor {
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                                       string[] movedFromAssetPaths, bool didDomainReload) {
      foreach (string str in importedAssets) {
        Debug.Log("Reimported Asset: " + str);
        if (str.EndsWith(".mat")) {
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
}
