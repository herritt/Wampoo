using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform board;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private Transform p3;
    [SerializeField] private Transform p4;
    [SerializeField] private float rotateSpeed;

    public enum CameraMode { ROTATE_BOARD, MOVE_TO_PLAYER, CENTRED_ON_PLAYER };

    public CameraMode cameraMode;

    private Transform currentPlayer;

    private float EPISOLON = 0.01f;
    public float smoothTime = 7F;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        switch (cameraMode)
        {
            case CameraMode.ROTATE_BOARD:
                cam.transform.RotateAround(board.position, board.forward, Time.deltaTime * rotateSpeed);
                break;
            case CameraMode.MOVE_TO_PLAYER:
                //cam.transform.position = Vector3.Lerp(cam.transform.position, currentPlayer.position, Time.deltaTime);
                cam.transform.LookAt(board.position);

                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, currentPlayer.position, ref velocity, smoothTime);

                if (Vector2.Distance(cam.transform.position, currentPlayer.position) < EPISOLON)
                {
                    cam.transform.position = currentPlayer.position;
                    cameraMode = CameraMode.CENTRED_ON_PLAYER;
                }

                break;
            case CameraMode.CENTRED_ON_PLAYER:
                

                break;
        }


    }

    public void MoveToPlayer(int player)
    {
        switch (player)
        {
            case 0:
                currentPlayer = p1;
                break;
            case 1:
                currentPlayer = p2;
                break;
            case 2:
                currentPlayer = p3;
                break;
            case 3:
                currentPlayer = p4;
                break;
        }

        cameraMode = CameraMode.MOVE_TO_PLAYER;
    }
}