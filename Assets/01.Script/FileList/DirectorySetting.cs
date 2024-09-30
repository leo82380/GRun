using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DirectorySetting : MonoBehaviour
{
    public GameObject filePrefab;
    private string path;
    public List<string> paths;
    private void Start()
    {
        path = Application.dataPath;
    }

    private int i = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            path = paths[i++];
            string[] files = System.IO.Directory.GetFiles(path);

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (string file in files)
            {
                GameObject gm = Instantiate(filePrefab, transform);
                FileInfo info = new FileInfo(file);
                gm.transform.GetComponent<FileData>().name = info.Name;
                gm.transform.GetComponent<FileData>().bytes = info.Length/1024;
                gm.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = info.Name;
                gm.transform.Find("FileSize").GetComponent<TextMeshProUGUI>().text =(info.Length/1024)+"KB";
            }
        }
        
    }
}