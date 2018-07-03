using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPQueueTest : MonoBehaviour {

    public void SendAQueue() {
        for (int i = 0; i < 100; i++)
        {
            UDPAgent.Instance.SendUDP(i.ToString());
        }        
    }
}
