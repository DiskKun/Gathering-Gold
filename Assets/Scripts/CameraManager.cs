using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public List<CinemachineCamera> cams;
    public int camIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CinemachineCamera c in cams)
        {
            if (cams.IndexOf(c) == camIndex)
            {
                c.enabled = true;
            }
            else
            {
                c.enabled = false;
            }
        }
    }


}
