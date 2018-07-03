using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoPlayerManager : MonoBehaviour
{
    //public Image _img;
    //public InputField _text;
    //private CDKey _cdKey;
    //public GameObject _cdkeyCover;
    //public Image bg;

    //private bool IsValid = false;

    void Start()
    {
        //string _fileURL = CheckImageFile();
        //StartCoroutine(WaitLoad(_fileURL));

        //_cdKey = new CDKey();
        //if (_cdKey.VerifyCDKEY())
        //{
        //    _cdkeyCover.SetActive(false);
        //    IsValid = true;
        //}
        //else
        //    _text.text = SystemInfo.deviceUniqueIdentifier;

        SingleMoviePlayer.Instance.mvCompleteHandler += MovieFinishHandler;
    }
    

    //影片结束事件处理
    void MovieFinishHandler(int index)
    {
        //bg.enabled = true;
    }

    void Update () {
        if (UDPAgent.Instance.recvData != null)
        {
            if (UDPAgent.Instance.recvData.Length >= 6 && UDPAgent.Instance.recvData.Substring(0, 6) == "replay")
            {
                try
                {
                    int replayIndex = int.Parse( UDPAgent.Instance.recvData.Substring(6, 1));
                    SingleMoviePlayer.Instance.ReplayMovieAt(replayIndex-1);
                    //bg.enabled = false;
                }
                catch (Exception)
                {
                    Debug.Log("Replay Failed");
                }
            }

            switch (UDPAgent.Instance.recvData)
            {
                case "play":
                    
                    break;
                case "stop":
                    SingleMoviePlayer.Instance.StopMovie();
                    //bg.enabled = true;
                    break;
                case "pause":

                    break;
                case "volumedown":
                    SingleMoviePlayer.Instance.VolumeDown();
                    break;
                case "volumeup":
                   SingleMoviePlayer.Instance.VolumeUp();
                    break;
            }

            UDPAgent.Instance.recvData = null;
        }
    }
}
