using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using UnityEditor;
//using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Ecilos.Ipfs.Settings
{
  /// <summary>
  /// Scriptable Object that holds setting information for the Ipfs endpoints.
  /// </summary>
  public class EcilosIpfsSettings : ScriptableObject, ISerializationCallbackReceiver
  {
    const string DEFAULT_PATH = "Assets/Settings/Ecilos";
    const string DEFAULT_NAME = "EcilosIpfsSettings";
    const string CONTENT_RANGE_HEADER = "Content-Range";
    static string DEFAULT_SETTING_PATH = $"{DEFAULT_PATH}/{DEFAULT_NAME}.asset";

    [InitializeOnLoadMethod]
    internal static void InitializeEnvironment()
    {
    }

    /// <summary>
    /// Group types that exist within the settings object
    /// </summary>
    //[SerializeField] public List<ProfileGroupType> profileGroupTypes = new List<ProfileGroupType>();

    [SerializeField]
    internal List<Environment> environments = new List<Environment>();

    [SerializeField]
    internal Environment currentEnvironment;

    /// <summary>
    /// Creates, if needed, and returns the Ipfs settings for the project.
    /// </summary>
    /// <param name="path">Desired path to put settings</param>
    /// <param name="settingName">Desired name for settings</param>
    /// <returns></returns>
    public static EcilosIpfsSettings Create(string path = null, string settingName = null)
    {
      EcilosIpfsSettings aa;
      var assetPath = DEFAULT_SETTING_PATH;

      if (path != null && settingName != null)
      {
        assetPath = $"{path}/{settingName}.asset";
      }

      aa = AssetDatabase.LoadAssetAtPath<EcilosIpfsSettings>(assetPath);
      if (aa == null)
      {
        Directory.CreateDirectory(path != null ? path : DEFAULT_PATH);
        aa = CreateInstance<EcilosIpfsSettings>();
        AssetDatabase.CreateAsset(aa, assetPath);
        aa = AssetDatabase.LoadAssetAtPath<EcilosIpfsSettings>(assetPath);
        //aa.profileGroupTypes = CreateDefaultGroupTypes();
        EditorUtility.SetDirty(aa);
      }

      return aa;
    }

    /// <summary>
    /// Gets the profile data source settings for the project.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="settingName"></param>
    /// <returns></returns>
    public static EcilosIpfsSettings GetSettings(string path = null, string settingName = null)
    {
      EcilosIpfsSettings aa;
      var assetPath = DEFAULT_SETTING_PATH;

      if (path != null && settingName != null)
      {
        assetPath = $"{path}/{settingName}.asset";
      }

      aa = AssetDatabase.LoadAssetAtPath<EcilosIpfsSettings>(assetPath);
      if (aa == null)
      {
        return Create();
      }
      return aa;
    }

    /*
    /// <summary>
    /// Creates a list of default group types that are automatically added on EcilosIpfsSettings object creation
    /// </summary>
    /// <returns>List of ProfileGroupTypes: Built-In and Editor Hosted</returns>
    public static List<ProfileGroupType> CreateDefaultGroupTypes() => new List<ProfileGroupType>
    {
        CreateBuiltInGroupType(),
        CreateEditorHostedGroupType(),
    };

    static ProfileGroupType CreateBuiltInGroupType()
    {
        ProfileGroupType defaultBuiltIn = new ProfileGroupType(AddressableAssetSettings.LocalGroupTypePrefix);
        defaultBuiltIn.AddVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kBuildPath, AddressableAssetSettings.kLocalBuildPathValue));
        defaultBuiltIn.AddVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kLoadPath, AddressableAssetSettings.kLocalLoadPathValue));
        return defaultBuiltIn;
    }

    static ProfileGroupType CreateEditorHostedGroupType()
    {
        ProfileGroupType defaultRemote = new ProfileGroupType(AddressableAssetSettings.EditorHostedGroupTypePrefix);
        defaultRemote.AddVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kBuildPath, AddressableAssetSettings.kRemoteBuildPathValue));
        defaultRemote.AddVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kLoadPath, AddressableAssetSettings.RemoteLoadPathValue));
        return defaultRemote;
    }


    /// <summary>
    /// Given a valid profileGroupType, searches the settings and returns, if exists, the profile group type
    /// </summary>
    /// <param name="groupType"></param>
    /// <returns>ProfileGroupType if found, null otherwise</returns>
    public ProfileGroupType FindGroupType(ProfileGroupType groupType)
    {
        ProfileGroupType result = null;
        if (!groupType.IsValidGroupType())
        {
            throw new ArgumentException("Group Type is not valid. Group Type must include a build path and load path variables");
        }

        var buildPath = groupType.GetVariableBySuffix(AddressableAssetSettings.kBuildPath);
        var loadPath = groupType.GetVariableBySuffix(AddressableAssetSettings.kLoadPath);
        foreach (ProfileGroupType settingsGroupType in profileGroupTypes)
        {
            var foundBuildPath = settingsGroupType.ContainsVariable(buildPath);
            var foundLoadPath = settingsGroupType.ContainsVariable(loadPath);
            if (foundBuildPath && foundLoadPath)
            {
                result = settingsGroupType;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Retrieves a list of ProfileGroupType that matches the given prefix
    /// </summary>
    /// <param name="prefix">prefix to search by</param>
    /// <returns>List of ProfileGroupType</returns>
    public List<ProfileGroupType> GetGroupTypesByPrefix(string prefix)
    {
        return profileGroupTypes.Where((groupType) => groupType.GroupTypePrefix.StartsWith(prefix, StringComparison.Ordinal)).ToList();
    }
    */

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      /*
      // Ensure static Group types have the correct string
      // Local
      var types = GetGroupTypesByPrefix(AddressableAssetSettings.LocalGroupTypePrefix);
      if (types == null || types.Count == 0)
        profileGroupTypes.Add(CreateBuiltInGroupType());
      else
      {
        types[0].AddOrUpdateVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kBuildPath,
            AddressableAssetSettings.kLocalBuildPathValue));
        types[0].AddOrUpdateVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kLoadPath,
            AddressableAssetSettings.kLocalLoadPathValue));
      }

      // Editor Hosted
      types = GetGroupTypesByPrefix(AddressableAssetSettings.EditorHostedGroupTypePrefix);
      if (types.Count == 0)
        profileGroupTypes.Add(CreateEditorHostedGroupType());
      else
      {
        types[0].AddOrUpdateVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kBuildPath,
            AddressableAssetSettings.kRemoteBuildPathValue));
        types[0].AddOrUpdateVariable(new ProfileGroupType.GroupTypeVariable(AddressableAssetSettings.kLoadPath,
            AddressableAssetSettings.RemoteLoadPathValue));
      }
      */
    }

    /// <summary>
    /// Access Token
    /// </summary>
    private class Token
    {
      [SerializeField]
      public string token;
    }

    /// <summary>
    /// Environment Wrapper Object
    /// </summary>
    internal class Environments
    {
      [SerializeField]
      public List<Environment> results;
    }

    /// <summary>
    /// Identity API Environment Object
    /// </summary>
    [Serializable]
    internal class Environment
    {
      [SerializeField]
      public string id;

      [SerializeField]
      public string projectId;

      [SerializeField]
      public string projectGenesisId;

      [SerializeField]
      public string name;

      [SerializeField]
      public bool isDefault;
    }
  }
}
