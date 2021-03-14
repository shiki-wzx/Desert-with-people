using System;
using Sirenix.Utilities;
using UnityEngine;


[Serializable]
public class CameraBound {
    public Vector2 X = new Vector2(-6, 6);
    public Vector2 Y = new Vector2(1, 10);
    public Vector2 Z = new Vector2(-6, 12);

    // no need to clamp rotation (probably)
    //public Vector2 Yaw;
}


// todo: smooth move
public class CameraControl : SingletonMono<CameraControl> {
    [SerializeField] private float panSpeed = 45;
    [SerializeField] private float orbitSpeed = 450;
    [SerializeField] private float zoomSpeed = 450;

    [SerializeField] private CameraBound bound;


    /// <summary> Listen to this to handel mouse click on block. </summary>
    public Action<BlkRtInfo> ClickCallback;


    private new Camera camera;


    protected override void OnInstanceAwake() {
        camera = GetComponent<Camera>();

        ClickCallback += delegate(BlkRtInfo info) {
            //var hl = Instantiate(highlightPrefab, info.transform, false);
            //Destroy(hl, 1);
            Debug.Log($"#{info.BlkIdx} {info.BlkParam.Type} clicked");
        };
    }


    private const string MouseX = "Mouse X", MouseY = "Mouse Y", MouseScroll = "Mouse ScrollWheel";
    private const int MouseLeft = 0, MouseRight = 1, MouseMiddle = 2;


    private void LateUpdate() {
        var zoom = Input.GetAxis(MouseScroll) * zoomSpeed * Time.deltaTime;
        var panX = 0f;
        var panY = 0f;
        if(Input.GetMouseButton(MouseMiddle)) {
            panX = Input.GetAxis(MouseX) * panSpeed * Time.deltaTime;
            panY = Input.GetAxis(MouseY) * panSpeed * Time.deltaTime;
        }

        transform.Translate(-panX, -panY, zoom);
        transform.position = transform.position.Clamp(new Vector3(bound.X[0], bound.Y[0], bound.Z[0]),
                                                      new Vector3(bound.X[1], bound.Y[1], bound.Z[1]));

        var pitch = 0f;
        var yaw = 0f;
        if(Input.GetMouseButton(MouseRight)) {
            pitch = Input.GetAxis(MouseY) * orbitSpeed * Time.deltaTime;
            yaw = Input.GetAxis(MouseX) * orbitSpeed * Time.deltaTime;
        }

        transform.Rotate(0, yaw, 0, Space.World);
        transform.Rotate(-pitch, 0, 0); // Space.Self

        if(Input.GetMouseButtonDown(MouseLeft)) {
            var hasHit = Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition),
                                         out var hitInfo, 100f, LayerMask.GetMask("BlkLayer"));
            if(hasHit) {
                var rtInfo = hitInfo.collider.GetComponentInParent<BlkRtInfo>();
                ClickCallback?.Invoke(rtInfo);
            }
        }
    }
}
