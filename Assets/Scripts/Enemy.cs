using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A,B,C,BOSS};
    public Type EnemyType;
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public bool isChase;

    private Player pl;

    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameManager manager;
    public GameObject[] coins;
    public bool isAttack;
    public bool isDead;

    bool isPartic;

    protected Rigidbody rb;
    protected BoxCollider boxCollider;
    protected ParticleSystem par;
    protected NavMeshAgent nav;
    protected Animator anim;

    //public GameObject hpBarPr;
    //public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    //private Canvas uiCanvas;
    //private Image hpBarImage;

    // Start is called before the first frame update

    //private void Start()
    //{
    //    //SetHpBar();
    //}
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        par = GetComponentInChildren<ParticleSystem>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        par.Stop();

        if(EnemyType != Type.BOSS)
        {
            Invoke("ChaseStart", 2);
        }
    }

    //void SetHpBar()
    //{
    //    uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
    //    GameObject hpBar = Instantiate<GameObject>(hpBarPr, uiCanvas.transform);
    //    hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

    //    var _hpbar = hpBar.GetComponent<EnemyHpBar>();
    //    _hpbar.targetT = this.gameObject.transform;
    //    _hpbar.offset = hpBarOffset;
    //}

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk",true);
    }

    // Update is called once per frame
    void Update()
    {
        if(nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //pBarImage.fillAmount= curHealth/maxHealth;
            Vector3 reactVec = transform.position - other.transform.position;


            StartCoroutine(OnDamage(reactVec));
            Debug.Log("Melee : " + curHealth);
        }   
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            //hpBarImage.fillAmount = curHealth / maxHealth;
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
            Debug.Log("Range : " + curHealth);
        }
    }

    void Targeting()
    {
        if(!isDead && EnemyType != Type.BOSS)
        {
            float targetRadius = 0;
            float targetRange = 0f;

            switch (EnemyType)
            {
                case Type.A:
                    targetRadius = 1;
                    targetRange = 4f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 4f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 100f;
                    break;


            }


            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
            if (raycastHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        } 
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (EnemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rb.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;
                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody bulletRigd = instantBullet.GetComponent<Rigidbody>();
                bulletRigd.velocity = transform.forward * 20;
                yield return new WaitForSeconds(1f);
                Destroy(instantBullet);
                break;
        }


        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack",false);
    }

    private void FixedUpdate()
    {
        Targeting();
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {

        yield return new WaitForSeconds(0.5f);
        
        if(curHealth > 0)
        {

        }
        else
        { 
            gameObject.layer = 16;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            
            Player player = target.GetComponent<Player>();
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            switch (EnemyType)
            {
                case Type.A:
                    manager.enemyA--;
                    break;
                case Type.B:
                    manager.enemyB--;
                    break;
                case Type.C:
                    manager.enemyC--;
                    break;
                //case Type.BOSS:
                //    manager.enemyD--;
                //    break;
            }


            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            isPartic = true;
            par.Play();
            rb.AddForce(reactVec * 1,ForceMode.Impulse);

                Destroy(gameObject, 4);
        }
    }
}
