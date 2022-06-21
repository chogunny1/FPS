using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type {Melee,Range };
    public Type type;
    public int damage;
    public float rate;
    public int Maxammo;
    public int Curammo;
    public BoxCollider meleeArea;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;


    public void use()
    {
        if(type == Type.Melee)
        {
            StartCoroutine("Swing");
            StopCoroutine("Swing");
        }
        else if(type == Type.Range && Curammo > 0)
        {
            Curammo -= 1;
            StartCoroutine("shot");
            StopCoroutine("shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);

    }   

    IEnumerator shot()
    {
        GameObject intantBullet = Instantiate(bullet,bulletPos.position,bulletPos.rotation);
        Rigidbody bulletRd = intantBullet.GetComponent<Rigidbody>();
        bulletRd.velocity = bulletPos.forward * 50;

        yield return new WaitForSeconds(1f);
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up;
        CaseRid.AddForce(caseVec,ForceMode.Impulse);
        CaseRid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Idle");    
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
