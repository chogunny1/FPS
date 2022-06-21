using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject startzone;
    public GameObject d;
    public int stage;
    public float playeTime;
    public bool isBattle;
    public int enemyA;
    public int enemyB;
    public int enemyC;
    public int enemyD;

    public Transform[] enemyZone;
    public GameObject[] enemies;
    public List<int> enemyList;

    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerAmmoTxt;
    public Text playerHealthTxt;
    public Text playerCoinTxt;


    public Image Skill1;
    public Image Skill2;

    public Text enemyATxt1;
    public Text enemyATxt2;
    public Text enemyATxt3;


   // public RectTransform bossHealthGroup;
   // public RectTransform bossHealthBar;


    private void Awake()
    {
        enemyList = new List<int>();
    }

    public void StageStart()
    {
        d.SetActive(false);
        itemShop.SetActive(false);
        startzone.SetActive(false);

        foreach (Transform zone in enemyZone)
            zone.gameObject.SetActive(true);
        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    { 
        itemShop.SetActive(true);
        startzone.SetActive(true);

        foreach (Transform zone in enemyZone)
            zone.gameObject.SetActive(false);
        isBattle = false;
        stage++;
        player.transform.position = Vector3.up * 0.8f;
    }

    public void GameOver()
    {
        
    }

    IEnumerator InBattle()
    { 
        if(stage % 5 == 0)
        {
            enemyD++;
            GameObject instantEnemy = Instantiate(enemies[enemyList[3]], enemyZone[0].position, enemyZone[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            Boss boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for (int i = 0; i < stage; i++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyA++;
                        break;
                    case 1:
                        enemyB++;
                        break;
                    case 2:
                        enemyC++;
                        break;
                }

            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 3);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZone[ranZone].position, enemyZone[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4);
            }
        }
        while(enemyA + enemyB + enemyC + enemyD > 0)
        {
            yield return null;
        }
            yield return new WaitForSeconds(4f);
            StageEnd();
    }


    private void Update()
    {
        if(isBattle)
        {
            playeTime += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        stageTxt.text = "STAGE " + stage;
        int hour = (int)(playeTime / 3600);
        int min = (int)((playeTime - hour * 3600)/60);
        int second = (int)(playeTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}",min) + ":" + string.Format("{0:00}",second);

        playerHealthTxt.text = player.health + "/" + player.Maxhealth;
        playerCoinTxt.text = string.Format("{0:00}", player.coin);
        if(player.equipWeapon == null)
        {
            playerAmmoTxt.text = "- /" + player.ammo;
        }
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTxt.text = "- /" + player.ammo;
        }
        else
        {
            playerAmmoTxt.text = player.equipWeapon.Curammo + "/" + player.ammo;
        }

        //Skill1.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        //Skill1.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        //Skill1.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);

        enemyATxt1.text = enemyA.ToString();
        enemyATxt2.text = enemyB.ToString();
        enemyATxt3.text = enemyC.ToString();

        //if(boss != null)
        //    bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth,1,1);


    }
}
