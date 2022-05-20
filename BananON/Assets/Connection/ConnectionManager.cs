using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionManager : MonoBehaviourPunCallbacks {
    private static readonly string ROOM_NAME = "THE_ISLAND";

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            photonView.RPC(nameof(SayHiRPC), RpcTarget.Others, PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, GetRoomOptions(), null);
    }

    public override void OnJoinedRoom() {
        Debug.LogError($"Joined room {PhotonNetwork.CurrentRoom.Name} as {(PhotonNetwork.IsMasterClient ? "host" : "client")}");
#if UNITY_EDITOR
        //Create spectator
//#else //un-comment this
        //Create player
#endif
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        switch(returnCode) {
            default:
                Debug.LogError($"Failed to create room with code {returnCode} and message {message}");
                break;
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        switch (returnCode) {
            default:
                Debug.LogError($"Failed to create room with code {returnCode} and message {message}");
                break;
        }
    }

    private RoomOptions GetRoomOptions() {
        return new RoomOptions {
            EmptyRoomTtl = 0,
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 20,
            PlayerTtl = 0
        };
    }

    [PunRPC]
    private void SayHiRPC(Player sender) {
        Debug.LogError($"Player {sender} says hi!");
    }
}
