using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FileDatas : MonoBehaviour, IPointerClickHandler
{
    public string name;
    public long bytes;
    public void OnPointerClick(PointerEventData eventData)
    {
        print(name + bytes);
        Enemy._instance.GoBack(Mathf.Log(bytes, 100));
        Destroy(gameObject);
    }
}
