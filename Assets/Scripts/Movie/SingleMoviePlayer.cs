using System;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using System.IO;
/// <summary>
/// 本类为全屏视频播放组件
/// 1. 需要XML文件已正确载入
/// 2. 需要Media文件夹下视频文件已正确载入，文件名称已在XML文件中正确配置
/// </summary>
public class SingleMoviePlayer : MonoBehaviour
{
    //---单例设置---------------------------------
    [SerializeField] private bool singleton = false;
    public static SingleMoviePlayer Instance = null;

    //---公共变量---------------------------------
    [HideInInspector] public MediaPlayer mvPlayer;
    //[HideInInspector] public AVProWindowsMediaMovie mvSource;
    //[HideInInspector] public DisplayUGUI mvDisplay;
    [HideInInspector] public Action<int> mvStartHandler;                //影片开始“事件”
    [HideInInspector] public Action<int> mvCompleteHandler;             //影片结束“事件”
    [HideInInspector] public int currentMovieIndex = -1;                //当前影片序号

    void Awake()
    {
        if(singleton) Instance = this;
        mvPlayer = GetComponent<MediaPlayer>();
        //mvSource = GetComponent<AVProWindowsMediaMovie>();
        //mvSource._loadOnStart = false;
        //mvDisplay = GetComponent<DisplayUGUI>();
        //mvDisplay._movie = mvSource;
    }

    void Start() {}

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

    public bool ReplayMovieByURL(string url, string _scaleMode, bool isLoop = false)
    {
        if (!string.IsNullOrEmpty(url) && File.Exists(url))
        {
            //switch (_scaleMode)
            //{
            //    case "stretch":
            //        mvDisplay._scaleMode = ScaleMode.ScaleToFit;
            //        break;
            //    case "proportionalInside":
            //        mvDisplay._scaleMode = ScaleMode.StretchToFill;
            //        break;
            //    case "proportionalOutside":
            //        mvDisplay._scaleMode = ScaleMode.ScaleAndCrop;
            //        break;
            //    default:
            //        mvDisplay._scaleMode = ScaleMode.ScaleToFit;
            //        break;
            //}
            mvPlayer.m_Loop = isLoop;
            mvPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url);
            if (mvStartHandler != null) mvStartHandler.Invoke(currentMovieIndex);
            KioTools.StopReturnHomeClockStatic();
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// API: 影片停止播放
    /// </summary>
    public void StopMovie()
    {
        //mvSource.UnloadMovie();
        //mvPlayer.Control.Stop();
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

    public void Pause() {
        mvPlayer.Control.Pause();
    }

    public void Play()
    {
        mvPlayer.Control.Play();
    }

    public void VolumeUp()
    {
        if(mvPlayer.Control.GetVolume()<=0.95f)
            mvPlayer.Control.SetVolume( mvPlayer.Control.GetVolume() + 0.05f);
        Debug.Log(mvPlayer.Control.GetVolume());
    }

    public void VolumeDown()
    {
        if (mvPlayer.Control.GetVolume() >= 0.05f)
            mvPlayer.Control.SetVolume(mvPlayer.Control.GetVolume() - 0.05f);
        Debug.Log(mvPlayer.Control.GetVolume());
    }
}
