using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Camera followCamera;

    public float speed = 5f;
    public float jump = 5f;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject[] healths;
    public GameObject[] bullets;
    public GameObject[] coins;
    public GameObject grenadObject;
    //public GameManager manager;

    public GameObject tab;

    public int health;
    public int coin;
    public int ammo;
    public int hasGrenades;


    public int Maxhealth;
    public int Maxcoin;
    public int Maxammo;
    public int MaxhasGrenades;


    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool fDonw;
    bool rDow;
   // bool gDown;
    bool iDown;

    bool sDown1;
    bool sDown2;
    bool sDown3;

      
    //bool isJump;
    bool isDodge;
    bool isjump;
    bool isFireReady;
    bool isReload;
    bool isBorder;
    bool isDamage;
    bool isPause;
    bool isDead;
   public bool isAttack;

    Rigidbody rb;
    Animator anim;

    Vector3 moveVec;
    Vector3 dodgeVec;

    MeshRenderer[] mesh;
    GameObject nearObject;
    public Weapon equipWeapon;

    int equipWeaponIndex = -1;
    float fireDelay;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody>();
        mesh = GetComponentsInChildren<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    void Update()
    {
        GetInput();
        move();
        turn();
        Jump();
        //Grenade();
        Attack();
        Reload();
        Dodge();
        Swap();
        Interation();

        StartCoroutine(click());
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDonw = Input.GetButtonDown("Fire1");
        //gDown = Input.GetButtonDown("Fire2");
        rDow = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    IEnumerator click()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(isPause == false)
            {
                Time.timeScale = 0;
                tab.SetActive(true);
                isPause = true;
                yield return new WaitForSeconds(1);
            }
            if (isPause == true)
            {
                Time.timeScale = 1;
                tab.SetActive(false);
                isPause = false;
                yield return new WaitForSeconds(1);
            }
        }
    }

    // Update is called once per frame

    void move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(isDodge)
        {
            moveVec = dodgeVec;
        }
        if (isReload || !isFireReady || isDead)
            moveVec = Vector3.zero;
        if (!isBorder)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("Run", moveVec != Vector3.zero);
        anim.SetBool("Walk", wDown);
    }


    void turn()
    {
        transform.LookAt(transform.position + moveVec);


        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100))
            {
                Vector3 nextVec = raycastHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Jump()
    {
        if (jDown && !isjump &&moveVec == Vector3.zero && !isDead)
        {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            isjump = true;
        }
    }


    void Grenade()
    {
        //if(hasGrenades == 0)
        //{
        //    return;
        //}

        //if(gDown && !isReload)
        //{
        //    Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit raycastHit;
        //    if (Physics.Raycast(ray, out raycastHit, 100))
        //    {
        //        Vector3 nextVec = raycastHit.point - transform.position;
        //        nextVec.y = 10;
        //        GameObject instantGrende = Instantiate(grenadObject, transform.position, transform.rotation);       
        //        Rigidbody rigidGrende = instantGrende.GetComponent<Rigidbody>();
        //        rigidGrende.AddForce(nextVec, ForceMode.Impulse);
        //        rigidGrende.AddTorque(Vector3.back * 10, ForceMode.Impulse);

        //        hasGrenades--;
        //        grenades[hasGrenades].SetActive(false);
        //     }
        //}  
    }

    void Attack()
    {
        if(equipWeapon == null)
        {
            return;
        }
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (Input.GetMouseButtonDown(0) && isFireReady && !isDead && !isAttack)
        {
            Debug.Log("น฿ป็");
            equipWeapon.use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShoot");
            equipWeapon.use();
            fireDelay = 0;
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    equipWeapon.use();
        //    anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShoot");
        //    equipWeapon.use();
        //    fireDelay = 0;
        //}
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if(rDow && !isDodge && isFireReady && !isDead)
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 2f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.Maxammo ? ammo : equipWeapon.Maxammo;
        equipWeapon.Curammo =reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isDodge && !isDead)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doMove");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        isDodge = false;
        speed *= 0.5f;
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2  && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;


        int WeaponIndex = -1;
        if (sDown1) WeaponIndex = 0;
        if (sDown2) WeaponIndex = 1;
        if (sDown3) WeaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isDodge && !isDead)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = WeaponIndex;
            equipWeapon = weapons[WeaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            
            
        }
    }

    void Interation()
    {
        if(iDown && nearObject != null && !isDodge && !isDead)
        {
            if(nearObject.tag == "Weapon")
            {
                item item = nearObject.GetComponent<item>();
                int WeaponIndex = item.value;
                hasWeapons[WeaponIndex] = true;

                Destroy(nearObject);
            }
            else if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isAttack = true;
            }
        }
    }

    void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));

    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (moveVec == Vector3.zero)
                isjump = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "item")
        {
            item item = other.GetComponent<item>();
            switch(item.type)
            {
                case item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > Maxammo)
                    {
                        ammo = Maxammo;
                    }
                    break;
                case item.Type.Coin:
                    coin += item.value;
                    if (coin > Maxcoin)
                    {
                        coin = Maxcoin;
                    }
                    break;
                case item.Type.Heart:
                    health += item.value;
                    if (health > Maxhealth)
                    {
                        health = Maxhealth;
                    }
                    break;
                case item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > MaxhasGrenades)
                    {
                        hasGrenades = MaxhasGrenades;
                    }
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                bool isBossAtk = other.name == "Boss Melee Area";
                StartCoroutine(Ondamage(isBossAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "Enemy")
        {
            if(!isDamage)
            {

            }
        }
    }

    IEnumerator Ondamage(bool isBossAtk)
    {
        isDamage = true;
        foreach(MeshRenderer mesh in mesh)
        {
            mesh.material.color = Color.blue;
        }
        if (isBossAtk)
            rb.AddForce(transform.forward * -25, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);
        isDamage = false;
        foreach (MeshRenderer mesh in mesh)
        {
            mesh.material.color = Color.white;
        }

        if(isBossAtk)
        {
            rb.velocity = Vector3.zero;
        }

        if(health <= 0)
        {
            Ondie();
        }
    }
    
    void Ondie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        //manager.GameOver();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon" || other.tag == "Shop")
        {
            nearObject = other.gameObject;
            Debug.Log(nearObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
        else if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isAttack = false;
            nearObject = null;
        } 
    }
}   
