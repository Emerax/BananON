using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionManager : MonoBehaviourPunCallbacks {
    [SerializeField]
    private float spawnDistance = 5;
    private Vector3 spawnerPos;
    private static readonly string ROOM_NAME = "THE_ISLAND";
    private static readonly float GROUND_LEVEL = 1f;
    [SerializeField]
    private SpectatorCamera spectatorPrefab;

    void Awake() {
        spawnerPos = FindObjectOfType<BananaSpawner>().transform.position;
    }

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update() {
        if(Keyboard.current.spaceKey.wasPressedThisFrame) {
            photonView.RPC(nameof(SayHiRPC), RpcTarget.Others, PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, GetRoomOptions(), null);
    }

    public override void OnJoinedRoom() {
#if ENABLE_VR
        SpawnVRPlayer();
#elif UNITY_EDITOR
        SpawnSpectator();
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
        switch(returnCode) {
            default:
                Debug.LogError($"Failed to create room with code {returnCode} and message {message}");
                break;
        }
    }

    private void SpawnVRPlayer() {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPoint = spawnerPos + new Vector3(spawnDirection.x, 0, spawnDirection.y) * spawnDistance;
        spawnPoint.y = GROUND_LEVEL;
        object[] spawnParams = { PhotonNetwork.LocalPlayer.ActorNumber };
        PhotonNetwork.Instantiate("VRPlayer", spawnPoint, Quaternion.LookRotation(spawnPoint - new Vector3(0, GROUND_LEVEL, 0)), data: spawnParams);
    }

    private void SpawnSpectator() {
        Instantiate(spectatorPrefab);
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
    private void SayHiRPC(VRPlayer sender) {
        Debug.LogError($"Player {sender} says hi!");
    }
}
