//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyHpBar : MonoBehaviour
//{
//    private Camera uiCamera;
//    private Canvas canvas;
//    private RectTransform rectTransform;
//    private RectTransform rectHP;

//    [HideInInspector] public Vector3 offset  = Vector3.zero;    
//    [HideInInspector] public Transform targetT;
    



//    // Start is called before the first frame update
//    void Start()
//    {
//        canvas = GetComponentInParent<Canvas>();
//        uiCamera = canvas.worldCamera;
//        rectTransform = canvas.GetComponent<RectTransform>();
//        rectHP = this.gameObject.GetComponent<RectTransform>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    private void LateUpdate()
//    {
//        var screenPos = Camera.main.WorldToScreenPoint(targetT.position + offset);

//        //if(screenPos.z < 0.0f)
//        //{
//        //    screenPos *= -1.0f;
//        //}

//        var localPos = Vector2.zero;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,screenPos,uiCamera
//            , out localPos);

//        rectHP.localPosition = localPos;    
//    }
//}
