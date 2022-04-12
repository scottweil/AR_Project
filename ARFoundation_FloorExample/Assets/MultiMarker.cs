using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//ARTrackedImageManager에게 마커가 추적되었는지 정보를 받고 싶다.
//추적된 마커가 내가 알고 있는 목록에 있는 녀석이라면
// 그 마커에 해당하는 오브젝트를 그 위치에 배치하고 싶다.
public class MultiMarker : MonoBehaviour
{
    ARTrackedImageManager aRTrackedImageManager;

    //유니티 lifecycle(Awake - enable - start )
    private void Awake()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    private void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += OntrackedImagesChanged; //델리게이트, event driven
    }
    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OntrackedImagesChanged; //델리게이트
    }
    void Start()
    {

    }

    [System.Serializable]
    public class MarkerInfo
    {
        public string name;
        public GameObject Object;
    }
    public MarkerInfo[] infos;

    private void OntrackedImagesChanged(ARTrackedImagesChangedEventArgs arg)
    {
        var list = arg.updated;
        for (int i = 0; i < list.Count; i++)
        {
            var marker = list[i];
            for (int j = 0; j < infos.Length; j++)
            {
                //추적된 마커가 내가 알고 있는 목록에 있는 녀석이라면
                if (marker.referenceImage.name == infos[j].name)
                {
                    if (marker.trackingState == TrackingState.Tracking)
                    {
                        // 그 마커에 해당하는 오브젝트를 그 위치에 배치하고 싶다.
                        infos[j].Object.SetActive(true);
                        infos[j].Object.transform.position = marker.transform.position;
                        infos[j].Object.transform.rotation = marker.transform.rotation;
                    }
                    else
                    {
                        infos[j].Object.SetActive(false);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
