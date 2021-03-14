using System;
using UnityEngine;
using Sirenix.OdinInspector;

// todo: bound this
// todo: smooth this
public struct CamLimit
{
	public Vector2 zClamp;//translate
	public Vector2 xClamp;//translate
	public Vector2 yClamp;
	public Vector2 yawClamp;//rotation
}
public class CameraControl : SingletonMono<CameraControl>
{
	//[SerializeField] private GameObject highlightPrefab;

	[SerializeField] private float panSpeed = 50;
	[SerializeField] private float orbitSpeed = 500;
	[SerializeField] private float zoomSpeed = 500;
	[ShowInInspector]
	CamLimit limit;
	

	/// <summary>
	/// Listen to this to handel mouse click on block.
	/// </summary>
	public Action<BlkRtInfo> ClickCallback;


	private new Camera camera;


	protected override void OnInstanceAwake()
	{
		camera = GetComponent<Camera>();

		// todo: test code
		ClickCallback += delegate (BlkRtInfo info)
		{
			//var hl = Instantiate(highlightPrefab, info.transform, false);
			//Destroy(hl, 1);
			Debug.Log($"#{info.BlkIdx} {info.BlkParam.Type} clicked");
		};
	}

	private const string MouseX = "Mouse X", MouseY = "Mouse Y", MouseScroll = "Mouse ScrollWheel";
	private const int MouseLeft = 0, MouseRight = 1, MouseMiddle = 2;


	private void LateUpdate()
	{
		var zoom = Input.GetAxis(MouseScroll) * zoomSpeed * Time.deltaTime;
		var panX = 0f;
		var panY = 0f;
		//zoom = Mathf.Clamp(zoom, limit.zClamp[0], limit.zClamp[1]);

		if (Input.GetMouseButton(MouseMiddle))
		{
			panX = Input.GetAxis(MouseX) * panSpeed * Time.deltaTime;
			panY = Input.GetAxis(MouseY) * panSpeed * Time.deltaTime;
		}

		transform.Translate(-panX, -panY, zoom);

		var pitch = 0f;
		var yaw = 0f;
		if (Input.GetMouseButton(MouseRight))
		{
			pitch = Input.GetAxis(MouseY) * orbitSpeed * Time.deltaTime;
			yaw = Input.GetAxis(MouseX) * orbitSpeed * Time.deltaTime;
		}

		transform.Rotate(0, yaw, 0, Space.World);
		transform.Rotate(-pitch, 0, 0); // Space.Self


		if (Input.GetMouseButtonDown(MouseLeft))
		{
			var hasHit = Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition),
										 out var hitInfo, 100f, LayerMask.GetMask("BlkLayer"));
			if (hasHit)
			{
				var rtInfo = hitInfo.collider.GetComponentInParent<BlkRtInfo>();
				ClickCallback?.Invoke(rtInfo);
			}
		}
	}
}
