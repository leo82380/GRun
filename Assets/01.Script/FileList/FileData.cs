using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FileData : MonoBehaviour, IPointerClickHandler
{
    public string name;
    public long bytes;
    public void OnPointerClick(PointerEventData eventData)
    {
        print(name + bytes);
        //do damage
        Destroy(gameObject);
    }
}
