using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
public class Manager : MonoBehaviourPunCallbacks
{
    public TMP_Text textIdicator;
    // Start is called before the first frame update
    public void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public void ConectPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }


    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("si");
        textIdicator.text = "si";
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        textIdicator.text = "bienbenido" + PhotonNetwork.NickName;
    }

    public void CreatePlayer(string PlayerName)
    {
        PhotonNetwork.NickName = PlayerName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("desconectado");
    }
    void Update()
    {
        
    }
}
