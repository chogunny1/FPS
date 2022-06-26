using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        par = GetComponentInChildren<ParticleSystem>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        par.Stop();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            StopAllCoroutines();
            return; 
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h,0,v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(1,4);
        switch (ranAction)
        {
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
                StartCoroutine(RockShot());
                break;
            case 3:
                StartCoroutine(Taunt());
                break;
        }
    }


    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position,missilePortA.rotation);
        BossMissile bossMissleA = instantMissileA.GetComponent<BossMissile>();
        bossMissleA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissleB = instantMissileB.GetComponent<BossMissile>();
        bossMissleB.target = target;

        yield return new WaitForSeconds(2f);
        Destroy(bossMissleA);
        Destroy(bossMissleB);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
