using UnityEngine;
using System.IO;
using System.Xml;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
public class ConfigXML
{
    public static string AppRootPath;
    public static string AppMediaPath;
    public static XmlNode XMLData;
}

[ExecuteInEditMode]
public class LoadConfigXml_VerifyMediaFolder : MonoBehaviour
{
    [SerializeField] [Tooltip("关闭消息，只打印错误")] private bool mute = false;

    protected void OnEnable()
    {
        if (!CheckAllExteral())
            LogWithTimestamp.LogError("外部文件检查未通过！！！！！");
    }

    //按照模板重新生成XML文件和Media文件夹
    public void RenewConfigAndMedia()
    {
        XmlDocument xmlconfig = new XmlDocument();
        DirectoryInfo appDataDirInfo = new DirectoryInfo(Application.dataPath);
        string mediaFolderPath = appDataDirInfo.Parent.FullName + @"\Media";
        string moduleConfigPath = appDataDirInfo.FullName + @"\Resources\Config.xml";
        string targetConfigPath = appDataDirInfo.Parent.FullName + @"\Config.xml";

        //Media
        if (Directory.Exists(mediaFolderPath))
            Debug.Log("Media文件夹已存在。");
        else
            Directory.CreateDirectory(mediaFolderPath);

        //Config.xml
        if (!File.Exists(moduleConfigPath))
        {
            Debug.LogError("Config原始文件缺失");
            return;
        }
        if (File.Exists(targetConfigPath))
            File.Delete(targetConfigPath);
        File.Copy(moduleConfigPath, targetConfigPath);

        ReCheck();
    }

    //依次检测所有外部文件
    bool CheckAllExteral()
    {
        //1. 检测XML文件是否正确导入
        bool xmlFileExist = InitializeConfigXML();
        if (!xmlFileExist) return false;
        //2. 检测外部素材是否正确导入
        bool isExteralAssetsReady = InitializeMediaFolder();
        if (!isExteralAssetsReady) return false;

        return true;
    }

    //初始化外部配置
    bool InitializeConfigXML()
    {
        XmlDocument xmlconfig = new XmlDocument();
        DirectoryInfo appDataDirInfo = new DirectoryInfo(Application.dataPath);
        FileInfo localConfig = new FileInfo(appDataDirInfo.Parent.FullName + @"\Config.xml");
        if (localConfig.Exists)
        {
            ConfigXML.AppRootPath = appDataDirInfo.Parent.FullName;
            ConfigXML.AppMediaPath = appDataDirInfo.Parent.FullName + @"\Media\";
            xmlconfig.Load(appDataDirInfo.Parent.FullName + @"\Config.xml");
            ConfigXML.XMLData = xmlconfig.ChildNodes[1];
            if(!mute) LogWithTimestamp.Log("正常检测到XML文件");
            return true;
        }
        else
        {
            LogWithTimestamp.Log("Config文件未在程序根目录下。。。");
            return false;
        }
    }

    //初始化外部资源
    bool InitializeMediaFolder()
    {
        DirectoryInfo appDataDirInfo = new DirectoryInfo(Application.dataPath);
        DirectoryInfo mediaDirInfo = new DirectoryInfo(appDataDirInfo.Parent.FullName + @"\Media");
        if (mediaDirInfo.Exists)
        {
            if (!mute) LogWithTimestamp.Log("正常检测到Media文件夹");
            return true;
        }
        else
        {
            LogWithTimestamp.Log("未检测到Media文件夹在程序根目录下。。。");
            return false;
        }
    }

    /// <summary>
    /// API:重新检查外部文件
    /// </summary>
    public virtual void ReCheck()
    {
#if UNITY_EDITOR
        // This simply does "LogEntries.Clear()" the long way:  
        //var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        //var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        //clearMethod.Invoke(null, null);
        //LogEntries.Clear();
        //Debug.ClearDeveloperConsole();

        if (!CheckAllExteral())
            LogWithTimestamp.LogError("外部文件检查未通过！！！！！");
#endif
    }
}