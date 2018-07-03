using UnityEngine;
using System.Collections;

public class UDPTest : MonoBehaviour {
	void Start () {
	    UDPAgent.Instance.SendUDP("aa");
	}

    void Update()
    {
        if (!string.IsNullOrEmpty(UDPAgent.Instance.recvData))
        {

            UDPAgent.Instance.SendUDP("bb");
            UDPAgent.Instance.recvData = null;
        }
    }
}
