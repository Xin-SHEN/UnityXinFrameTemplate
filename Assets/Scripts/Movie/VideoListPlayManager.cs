using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class VideoListPlayManager : MonoBehaviour
{
    int currentIndex = 0, maxIndex;
    [SerializeField] DisplayUGUI _displayUGUI;
    [SerializeField] GameObject bg, _MP;
    //bool replay = false;

    void Start()
    {
        SingleMoviePlayer.Instance.mvPlayer.Events.AddListener(OnVideoEvent);
        maxIndex = ConfigXML.XMLData["MovieList"].ChildNodes.Count - 1;
        StartCoroutine(StartLoopMovie());
        InitProjectCalibration();
    }

    void InitProjectCalibration()
    {
        if (ConfigXML.XMLData["CalibrationEnable"].InnerText == "true")
        {
            _MP.SetActive(true);
        }
    }

    IEnumerator StartLoopMovie()
    {
        yield return new WaitForEndOfFrame();
        string loopMovieURL = ConfigXML.XMLData["LoopMovieURL"].InnerText;
        bool playable = SingleMoviePlayer.Instance.ReplayMovieByURL(
            ConfigXML.AppMediaPath + loopMovieURL,
            ConfigXML.XMLData["LoopMovieURL"].Attributes["scaleMode"].Value,
            true);
        bg.SetActive(!playable);
        //replay = playable;
    }

    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                break;
            case MediaPlayerEvent.EventType.Started:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                _displayUGUI.color = Color.white;
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                //Debug.Log("Event: !!");
                //currentIndex++;
                //if (currentIndex > maxIndex) currentIndex = 0;
                //SingleMoviePlayer.Instance.ReplayMovieAt(currentIndex);
                //SingleMoviePlayer.Instance.StopMovie();
                _displayUGUI.color = Color.black;
                if (!SingleMoviePlayer.Instance.mvPlayer.Control.IsLooping())
                {
                    SingleMoviePlayer.Instance.StopMovie();
                    bg.SetActive(true);
                    _displayUGUI.color = Color.black;
                }
                break;
        }

        Debug.Log("Event: " + et.ToString());
    }

    //影片结束事件处理
    void MovieFinishHandler()
    {

    }

    void Update()
    {
        if (UDPAgent.Instance.recvData != null)
        {
            Debug.Log(UDPAgent.Instance.recvData);
            if (UDPAgent.Instance.recvData.Length >= 8 && UDPAgent.Instance.recvData.Substring(0, 8) == "changebg")
            {
                try
                {
                    int changeBGIndex = int.Parse(UDPAgent.Instance.recvData.Substring(8, 1));
                    bg.GetComponent<VideoBackground>().ChangeImageAt(changeBGIndex-1);
                }
                catch (Exception)
                {
                    Debug.Log("ChangeBG Failed");
                }
            }
            else if (UDPAgent.Instance.recvData.Length >= 6 && UDPAgent.Instance.recvData.Substring(0, 6) == "replay")
            {
                try
                {
                    int replayIndex = int.Parse(UDPAgent.Instance.recvData.Substring(6, 1));
                    _displayUGUI.color = Color.black;
                    SingleMoviePlayer.Instance.ReplayMovieAt(replayIndex - 1);
                    bg.SetActive(false);
                    //bg.enabled = false;
                    SingleMoviePlayer.Instance.mvPlayer.Control.SetLooping(false);

                }
                catch (Exception)
                {
                    Debug.Log("Replay Failed");
                }
            }

            switch (UDPAgent.Instance.recvData)
            {
                case "resume":
                    SingleMoviePlayer.Instance.Play();
                    break;
                case "stop":
                    SingleMoviePlayer.Instance.StopMovie();
                    bg.SetActive(true);
                    _displayUGUI.color = Color.black;
                    break;
                case "pause":
                    SingleMoviePlayer.Instance.Pause();
                    break;
                case "volumedown":
                    SingleMoviePlayer.Instance.VolumeDown();
                    break;
                case "volumeup":
                    SingleMoviePlayer.Instance.VolumeUp();
                    break;
                case "replayloopmovie":
                    _displayUGUI.color = Color.black;
                    SingleMoviePlayer.Instance.mvPlayer.Control.SetLooping(true);
                    StartCoroutine(StartLoopMovie());
                    break;
            }

            UDPAgent.Instance.recvData = null;
        }
    }
}
