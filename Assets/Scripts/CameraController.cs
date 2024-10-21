using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2f;
    public float minY = 10f;
    public float maxY = 80f;

    public Vector2 xLimit = new Vector2(-50f, 50f);  // x ��ǥ ����
    public Vector2 zLimit = new Vector2(-50f, 50f);  // z ��ǥ ����

    private Vector3 dragOrigin;

    void Update()
    {
        if (GameManager.gameEnded)
        {
            this.enabled = false;
        }

        // �巡�� ó��
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 difference = Input.mousePosition - dragOrigin;
        dragOrigin = Input.mousePosition;

        Vector3 move = new Vector3(-difference.x * dragSpeed * Time.deltaTime, 0, -difference.y * dragSpeed * Time.deltaTime);
        transform.Translate(move, Space.World);

        // x, z �� �̵� ����
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.z = Mathf.Clamp(pos.z, zLimit.x, zLimit.y);
        transform.position = pos;
    }
}