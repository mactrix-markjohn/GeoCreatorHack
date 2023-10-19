using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{


    [SerializeField] private ARRaycastManager m_RaycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject spawnablePrefab;
    [SerializeField] private GameObject placementIndicatorr;



    
    private GameObject placementIndicator;
    
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    public Camera arCam;
    
    private GameObject spawnedObject;
    
    private bool isSpawned = false;


    private void Awake()
    {
        placementIndicator = Instantiate(placementIndicatorr);
        placementIndicator.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;

    }

    // Update is called once per frame
     void Update()
    {
        HandlePlaneDetectionAndPlacement();
        UpdatePlacementIndicator();
        
    }

    void HandlePlaneDetectionAndPlacement()
    {
        if (Input.touchCount == 0)
            return;

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position,m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnnable")
                    {
                        //spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {

                if (isSpawned)
                    return;
                
                //spawnedObject.transform.position = m_Hits[0].pose.position;
            }

            /*if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }*/
        }
        
    }

    void UpdatePlacementIndicator()
    {

        if (isSpawned)
        {
            placementIndicator.SetActive(false);
            
            // m_RaycastManager.enabled = false;
            planeManager.enabled = false;

            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            
            return;
        }


        var screenCenter = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        m_RaycastManager.Raycast(screenCenter, hits);

        bool poseIsValid = hits.Count > 0;

        if (poseIsValid)
        {
            Pose pose = hits[0].pose;

            var cameraForward = arCam.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x,0,cameraForward.z).normalized;
            pose.rotation = Quaternion.LookRotation(cameraBearing);
            
            
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(pose.position,pose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        if (isSpawned)
            return;
        
        var cameraForward = arCam.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x,0,cameraForward.z).normalized;
        Quaternion lookRotation= Quaternion.LookRotation(cameraBearing);
        
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, lookRotation);
        
        
        //Start the engine and rotor of fans immediately after spawning
        /*touchHeliInput.HandleThrottle(1f);
        touchHeliInput.HandleThrottle(1f);
        touchHeliInput.HandleThrottle(1f);*/
        
        isSpawned = true;
        
       //spawnedObject.transform.Find("HeliPad").gameObject.SetActive(false);
    }
}
