using System;
using UnityEngine;
using UnityEngine.UI;

public class MazePlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Text _text;
    private int _handle;
    private float _x;
    private float _y;

    private void Awake()
    {
        _handle = SetWindowPosScript.GetHandle();
        _text.text = _handle.ToString();
    }

    private void Update()
    {
        _x += Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime;
        _y += Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
        transform.position = new Vector3(_x, _y, 0);
        SetWindowPosScript.SetWindowPos(_handle, 0, (int)(_x), -(int)(_y), 0, 0, 0x0001 | 0x0004);
    }
}