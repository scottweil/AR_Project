using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//메인 카메라 위치에서 카메라의 앞방향으로 Ray를 만들고
//부딪힌 것이 있다면 그곳에 indicator를 배치하고 싶다.
//Ray로 바라볼 때 indicator 레이어를 제외하고 싶다.
//화면을 터치했을 때 indicator라면 그 위치에 모델을 배치하고 싶다.
//
public class AR_Markerless : MonoBehaviour
{
    public GameObject indicator;
    public GameObject model;
    GameObject placedModel;
    ARRaycastManager arRaycastManager;
    public GameObject floor;

    void Start()
    {
#if UNITY_EDITOR
        floor.SetActive(true);
#else
        floor.SetActive(false);
#endif
        indicator.SetActive(false);
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        UpdateIndicator();
        UpdateMakeModel();

#else
        UpdateIndicatorForAndroid();
        UpdateMakeModelForAndroid();
#endif
    }



    private void UpdateMakeModel()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Touch touch = Input.GetTouch(0);Input.touchCount > 0 || 
            // if (touch.phase == TouchPhase.Began)
            // {
            //클릭한 위치를 기준으로 Ray를 만들고
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Indicator"))
                {

                    placedModel = Instantiate(model, hitInfo.transform.position, hitInfo.transform.rotation);
                }
            }
            // if (placedModel == null)
            // {
            //     placedModel = Instantiate(model, indicator.transform.position, indicator.transform.rotation);
            // }
            // else
            // {
            //     placedModel.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
            // }
            //}
        }
    }

    void UpdateIndicator()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        int layermask = ~(1 << LayerMask.NameToLayer("Indicator"));
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layermask))
        {
            indicator.SetActive(true);
            indicator.transform.position = hitInfo.point + hitInfo.normal * 0.1f;
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    private void UpdateMakeModelForAndroid()
    {
        // 화면을 터치했다면
        if (Input.touchCount > 0)
        {
            //터치를 누르는 순간
            Touch touch = Input.GetTouch(0); //가장 먼저 눌린 터치
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Indicator"))
                    {
                        placedModel = Instantiate(model, hitInfo.transform.position, hitInfo.transform.rotation);
                    }
                }
            }
            // if (touch.phase == TouchPhase.Began)
            // {
            //     if (placedModel == null)
            //     {
            //         placedModel = Instantiate(model, indicator.transform.position, indicator.transform.rotation);
            //     }
            //     else
            //     {
            //         placedModel.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
            //     }
            // }
        }
    }

    void UpdateIndicatorForAndroid()
    {
        //arRaycastManager를 이용해서 Raycast를 하고
        Vector2 screen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

        if (arRaycastManager.Raycast(screen, hitResults))
        {
            indicator.SetActive(true);
            indicator.transform.position = hitResults[0].pose.position;
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
