using System;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
/// <summary>
/// 本类为全屏视频播放组件
/// 1. 需要XML文件已正确载入
/// 2. 需要Media文件夹下视频文件已正确载入，文件名称已在XML文件中正确配置
/// </summary>

public class TextureMoviePlayer : MonoBehaviour {

	//---单例设置---------------------------------
    [SerializeField] private bool singleton = false;
    public static TextureMoviePlayer Instance = null;

    //---公共变量---------------------------------
    [HideInInspector] public MediaPlayer mvPlayer;
    [HideInInspector] public Action<int> mvStartHandler;                //影片开始“事件”
    [HideInInspector] public Action<int> mvCompleteHandler;             //影片结束“事件”
    [HideInInspector] public int currentMovieIndex = -1;                //当前影片序号

    void Awake()
    {
        if (singleton) Instance = this;
        mvPlayer = GetComponent<MediaPlayer>();
    }

    void Start(){}

    /// <summary>
    /// API：按照指定索引号播放影片，索引号取自外部XML配置
    /// </summary>
    /// <param name="index">影片索引号</param>
    public void ReplayMovieAt(int index)
    {
        if (index < 0 || index > ConfigXML.XMLData["MovieList"].ChildNodes.Count)
        {
            Debug.LogError("影片序号超出边界！");
            return;
        }



        currentMovieIndex = index;

        mvPlayer.m_VideoPath = ConfigXML.AppMediaPath + ConfigXML.XMLData["MovieList"].ChildNodes[index].InnerText;
        //mvSource._colourFormat = AVProWindowsMediaMovie.ColourFormat.RGBA32;
        //mvSource._folder = ConfigXML.AppMediaPath;
        //mvSource._filename = ConfigXML.XMLData["MovieList"].ChildNodes[index].InnerText;
        //bool loaded = mvSource.LoadMovie(true);
        mvPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToProjectFolder, ConfigXML.AppMediaPath + ConfigXML.XMLData["MovieList"].ChildNodes[index].InnerText);
        if (mvStartHandler != null) mvStartHandler.Invoke(currentMovieIndex);

        KioTools.StopReturnHomeClockStatic();
    }

    /// <summary>
    /// API: 影片停止播放
    /// </summary>
    public void StopMovie()
    {
        //mvSource.UnloadMovie();
        mvPlayer.Control.CloseVideo();
        KioTools.StartReturnHomeClockStatic();
    }

    //影片结束处理
    void MovieFinished()
    {
        //mvSource.UnloadMovie();
        if (mvCompleteHandler != null) mvCompleteHandler.Invoke(currentMovieIndex);
        KioTools.StartReturnHomeClockStatic();
    }

    public void VolumeUp()
    {
        mvPlayer.m_Volume += 0.05f;
    }

    public void VolumeDown()
    {
        mvPlayer.m_Volume -= 0.05f;
    }
}
