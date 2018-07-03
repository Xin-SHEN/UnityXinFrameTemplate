using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KioTools))]
public class KioToolsEditor : Editor
{
    //public static KioTools Instance = null;

    #region 菜单/静态方法
    [MenuItem("品奥插件/添加品奥插件管理器 _F9", false, 0)]
    public static void AddKioToolsInstance()
    {
        if (KioTools.Instance != null) { Debug.Log("插件管理器已经存在。"); return;}
        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Tools/KioTools.prefab"));
        go.name = "KioTools";
        Selection.activeGameObject = go;
    }

    [MenuItem("品奥插件/添加品奥插件管理器 _F9", true)]
    public static bool AddKioToolsInstanceValidation()
    {
        return KioTools.Instance == null;
    }

    [MenuItem("品奥插件/添加UDP控件 _F10", false, 1)]
    public static void AddUDPAgentInstance()
    {
        if (UDPAgent.Instance != null) { Debug.Log("UDP控件已经存在。"); return; }
        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Net/UDPAgent.prefab"));
        go.name = "UDPAgent";
        Selection.activeGameObject = go;
    }

    [MenuItem("品奥插件/添加UDP控件 _F10", true)]
    public static bool AddUDPAgentInstanceValidation()
    {
        return UDPAgent.Instance == null;
    }

    #region CDKey
    [MenuItem("品奥插件/添加注册控件 _F11", false, 2)]
    public static void AddCdkeyCoverInstance()
    {
        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Tools/CDKEY_Canvas.prefab"));
        go.name = "CDKEY_Canvas";
        Selection.activeGameObject = go;
    } 
    #endregion

    [MenuItem("品奥插件/从模板生成外部配置文件", false, 50)]
    public static void RenewConfigAndMedia()
    {
        KioTools.Instance.RenewConfigAndMedia();
    }

    [MenuItem("品奥插件/从模板生成外部配置文件", true)]
    public static bool RenewConfigAndMediaValidation()
    {
        return KioTools.Instance != null;
    }

    [MenuItem("品奥插件/重新检查外部配置文件", false, 51)]
    public static void RecheckConfigAndMedia()
    {
        KioTools.Instance.ReCheck();
    }

    [MenuItem("品奥插件/重新检查外部配置文件", true)]
    public static bool RecheckConfigAndMediaValidation()
    {
        return KioTools.Instance != null;
    }

    [MenuItem("品奥插件/生成全屏视频播放器", false, 100)]
    public static void CreatSingleMoviePlayer()
    {
        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Movie/SingleMoviePlayer.prefab"));
        go.name = "SingleMoviePlayer";
        Selection.activeGameObject = go;
    }

    [MenuItem("品奥插件/生成材质视频播放器", false, 101)]
    public static void CreatTextureMoviePlayer()
    {
        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Movie/TextureMoviePlayer.prefab"));
        go.name = "TextureMoviePlayer";
        Selection.activeGameObject = go;
    } 
    #endregion


    //----序列化变量
    private SerializedObject _target;
    private SerializedProperty _mute;
    private SerializedProperty _returnActionTarget;
    private SerializedProperty _mouseHide;

    //----内部变量
    private GUIStyle kioStyle;
    private KioTools _KioTools;
    private Texture _KIO_B;

    void OnEnable()
    {
        //GUISetup();

        //得到【LoadConfigXml_VerifyMediaFolder】对象
        _KioTools = (KioTools)target;
        _target = new SerializedObject(target);
        _mute = _target.FindProperty("mute");
        _returnActionTarget = _target.FindProperty("_returnActionTarget");
        _mouseHide = _target.FindProperty("_mouseHide");

        _KIO_B = Resources.Load("品奥LOGO（彩色）") as Texture;
    }

    void GUISetup()
    {
        //没效果？
        kioStyle = new GUIStyle();
        kioStyle.fontSize = 5;
        kioStyle.alignment = TextAnchor.MiddleCenter;
        kioStyle.fontStyle = FontStyle.Bold;
    }

    //绘制Inspector面板。
    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();


        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Separator();
            GUILayout.Label(_KIO_B,GUILayout.MaxHeight(80),GUILayout.MaxWidth(100));
            EditorGUILayout.BeginVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
        GUILayout.Label("品奥项目工具");
                GUILayout.Label("Version " + KioTools.VERSION);
                GUILayout.Label("Unity Compatible " + KioTools.UNITY_COMPATIBLE_VERSION);
            EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (GUILayout.Button("从模板生成外部配置文件"))
            _KioTools.RenewConfigAndMedia();
        if (GUILayout.Button("重新检查外部配置文件"))
            _KioTools.ReCheck();
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_mute, GUIContent.none, GUILayout.MaxWidth(15));
            GUILayout.Label("静默，仅报告错误",GUILayout.MaxWidth(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("检测到外部配置隐藏鼠标为：【 " + _KioTools._mouseHide+" 】",MessageType.Info,true);

        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.HelpBox("检测到外部配置返回延迟为：【 " + _KioTools._returnHomepageDelay + " 】秒", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("执行返回首页操作的对象：",GUILayout.MaxWidth(135));
            EditorGUILayout.PropertyField(_returnActionTarget,GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("注意：该对象必须有一个方法为【ReturnHome】");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("检测到外部UDP配置-目标IP地址为：【 " + _KioTools._targetIPAddress +":"+ _KioTools._sendPort + " 】，本地端口：【 " + _KioTools._receivePort+" 】", MessageType.Info, true);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("----【视频播放器】----------------------------------");
        if (GUILayout.Button("生成全屏视频播放器"))
            CreatSingleMoviePlayer();
        if (GUILayout.Button("生成材质视频播放器"))
            CreatTextureMoviePlayer();
        EditorGUILayout.LabelField("注意：材质视频播放器将把影片渲染到一个特定材质上。");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        
        _target.ApplyModifiedProperties();
    }
}
