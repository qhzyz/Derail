using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    //目标对象
    public Transform target;
    public float angleX, angleY, angleZ;
    public float distance = 10.0f;
    public float showScale = 1.0f;
    public float rotateSpeed = 1.0f;
    // 需要锁定的坐标（可以实时生效）
    public bool freazeX, freazeY, freazeZ;

    // 跟随的平滑时间（类似于滞后时间）
    public float smoothTime = 0.3F;
    private float xVelocity, yVelocity, zVelocity = 0.0F;

    // 跟随的偏移量
    private Vector3 offset;
    private Vector3 currentOffset;
    
    private Vector3 oldPosition;
    private Vector3 startPosition;
    float ToArc(float angle) {
        float arc = angle * 3.1415926f / 180.0f;
        return arc;
    }
    void Start() {
        transform.eulerAngles = new Vector3(angleX, angleY, angleZ);
        float tempX = -distance * Mathf.Cos(ToArc(angleX)) * Mathf.Sin(ToArc(angleY));
        float tempY = distance * Mathf.Sin(ToArc(angleX));
        float tempZ = -distance * Mathf.Cos(ToArc(angleX)) * Mathf.Cos(ToArc(angleY));

        transform.position = new Vector3(target.position.x+tempX, target.position.y + tempY, target.position.z + tempZ);
        startPosition = transform.position;
        offset = transform.position - target.position;
    }
    private void Update() {
        //CameraRotate();
    }
    void LateUpdate() {
        if (target == null) { return; }
        oldPosition = transform.position;
        currentOffset = offset * showScale;
        if (!freazeX) {
            oldPosition.x = Mathf.SmoothDamp(transform.position.x, target.position.x + currentOffset.x, ref xVelocity, smoothTime);
        }

        if (!freazeY) {
            oldPosition.y = Mathf.SmoothDamp(transform.position.y, target.position.y + currentOffset.y, ref yVelocity, smoothTime);
        }

        if (!freazeZ) {
            oldPosition.z = Mathf.SmoothDamp(transform.position.z, target.position.z + currentOffset.z, ref zVelocity, smoothTime);
        }
        transform.position = oldPosition;
    }
    public void ResetPosition() {
        transform.position = startPosition;
    }

    private void CameraRotate() //摄像机围绕目标旋转操作
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        if (Input.GetKey(KeyCode.Mouse1)) {
            transform.RotateAround(target.position, target.up, mouse_x * 5);
            offset = Quaternion.AngleAxis(mouse_x * 5, target.up) * offset;
        }
    }
    //private void camerazoom() //摄像机滚轮缩放
    //{
    //}
}
