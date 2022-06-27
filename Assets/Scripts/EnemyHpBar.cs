using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    //[SerializeField] GameObject Enemy = null;

    //List<Transform> targets = new List<Transform>();
    //List<GameObject> hpBar = new List<GameObject>();

    //Camera cam = null;

    //private void Start()
    //{
    //    cam = Camera.main;

    //    GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
    //    for(int i = 0; i < objects.Length; i++)
    //    {
    //        targets.Add(objects[i].transform);
    //        GameObject HPBar = Instantiate(Enemy, objects[i].transform.position,Quaternion.identity,transform);
    //        hpBar.Add(HPBar);
    //    }
    //}

    //private void Update()
    //{
    //    for(int i = 0; i < targets.Count;  i++)
    //    {
    //        hpBar[i].transform.position = cam.WorldToScreenPoint(targets[i].position + new Vector3(0,1.15f,0));
    //    }

    //}
}
