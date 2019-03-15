using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
	public static BattleManager instance;
	public GameController theGameController;
	public GameObject ConsoleUI;
	public InputField textIn; 
	public bool isActive = false;
	public GameObject battleScene;
	public GameObject movingEnemy;
	public Transform[] playerPos;
	public Transform[] enemyPos;

	public BattleChar[] playerPre;
	public BattleChar[] enemyPre;

	public List<BattleChar> activeBattler = new List<BattleChar>();

	private bool fleeing;
	public int currentTurn;
	public bool turnWaiting;
	public int enemyPresent = 0;
	public GameObject uiButtons;
	public BattleMoves[] moveList;
	public GameObject enemyAttackEffect;
	public DamageNumber theDamage;
	public int chanceToFlee = 30;
	public Text[] playerName, playerHp;
	public string gameOver;
	public TrollController Troll;
	public BatController Bat;
	public CyclopsController Cyclops;
	public int enemyType = 0;
	public int trollDead = 0, batDead = 0, cyclopsDead = 0;
	public int trollPresent = 0, batPresent = 0, cyclopsPresent = 0;

	//public PlayerLoader loadPlayer;
	//public PlayerController thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
		if(isActive)
		{
			if(turnWaiting)
			{
				if(activeBattler[currentTurn].notEnemy)
				{
					uiButtons.SetActive(true);
				}
				else
				{
					uiButtons.SetActive(false);
					StartCoroutine(EnemyMoveCo());
				}
			}
		}

//		if(Input.GetKeyDown(KeyCode.Q))
//		{
//			NextTurn();
//BattleStart("troll");
//		}
    }

	public void BattleStart(string enemy)
	{
			if(isActive == false)
			{
				if (enemy == "troll" && enemyType == 1)
				{
					isActive = true;
					PlayerController.instance.canMove = false;
					transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
					ConsoleUI.SetActive(false);
					battleScene.SetActive(true);
					movingEnemy.SetActive(false);
					BattleChar newPlayer = Instantiate(playerPre[0], playerPos[0].position, playerPos[0].rotation);
					newPlayer.transform.parent = playerPos[0];
					activeBattler.Add(newPlayer);

					CharStats player = GameController.instance.playerStats;
					activeBattler[0].currentHp = player.currentHP;
					activeBattler[0].maxHp = player.maxHP;
					activeBattler[0].strength = player.strength;
					activeBattler[0].defence = player.defence;
					activeBattler[0].attackPower = player.attackPower;
					activeBattler[0].armor = player.armor;

					BattleChar newEnemy = Instantiate(enemyPre[0], enemyPos[0].position, enemyPos[0].rotation);
					newEnemy.transform.parent = enemyPos[0];
					activeBattler.Add(newEnemy);
					turnWaiting = true;
					currentTurn = 0;
					UpdateUIStats();
				}

				if (enemy == "bat" && enemyType == 2)
				{
					isActive = true;
					PlayerController.instance.canMove = false;
					transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
					ConsoleUI.SetActive(false);
					battleScene.SetActive(true);
					movingEnemy.SetActive(false);
					BattleChar newPlayer = Instantiate(playerPre[0], playerPos[0].position, playerPos[0].rotation);
					newPlayer.transform.parent = playerPos[0];
					activeBattler.Add(newPlayer);

					CharStats player = GameController.instance.playerStats;
					activeBattler[0].currentHp = player.currentHP;
					activeBattler[0].maxHp = player.maxHP;
					activeBattler[0].strength = player.strength;
					activeBattler[0].defence = player.defence;
					activeBattler[0].attackPower = player.attackPower;
					activeBattler[0].armor = player.armor;

					BattleChar newEnemy = Instantiate(enemyPre[1], enemyPos[0].position, enemyPos[0].rotation);
					newEnemy.transform.parent = enemyPos[0];
					activeBattler.Add(newEnemy);
					turnWaiting = true;
					currentTurn = 0;
					UpdateUIStats();
				}

				if (enemy == "cyclops" && enemyType == 3)
				{
					isActive = true;
					PlayerController.instance.canMove = false;
					transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
					ConsoleUI.SetActive(false);
					battleScene.SetActive(true);
					movingEnemy.SetActive(false);
					BattleChar newPlayer = Instantiate(playerPre[0], playerPos[0].position, playerPos[0].rotation);
					newPlayer.transform.parent = playerPos[0];
					activeBattler.Add(newPlayer);

					CharStats player = GameController.instance.playerStats;
					activeBattler[0].currentHp = player.currentHP;
					activeBattler[0].maxHp = player.maxHP;
					activeBattler[0].strength = player.strength;
					activeBattler[0].defence = player.defence;
					activeBattler[0].attackPower = player.attackPower;
					activeBattler[0].armor = player.armor;

					BattleChar newEnemy = Instantiate(enemyPre[2], enemyPos[0].position, enemyPos[0].rotation);
					newEnemy.transform.parent = enemyPos[0];
					activeBattler.Add(newEnemy);
					turnWaiting = true;
					currentTurn = 0;
					UpdateUIStats();
				}
//				turnWaiting = true;
//				currentTurn = 0;
//				UpdateUIStats();
			}
		
	}

	public void NextTurn()
	{
		currentTurn++;
		if(currentTurn >= activeBattler.Count)
		{
			currentTurn = 0;
		}

		turnWaiting = true;
		UpdateBattle();
		UpdateUIStats();
	}

	public void UpdateBattle()
	{
		bool enemyDead = true;
		bool playerDead = true; 

		for(int i=0; i < activeBattler.Count; i++)
		{
			if(activeBattler[i].currentHp < 0)
			{
				activeBattler[i].currentHp = 0;
			}

			if (activeBattler[i].currentHp == 0)
			{
				//end battle
			}

			else
			{
				if(activeBattler[i].notEnemy)
				{
					playerDead = false;
				}
				else
				{
					enemyDead = false;
				}
			}
		}
		
		if (playerDead || enemyDead)
		{
			if (enemyDead)
			{
				fleeing = false;
				StartCoroutine(EndBattleCo());
			}

			else
			{
				fleeing = false;
				StartCoroutine(EndBattleCo());
			}

			battleScene.SetActive(false);
			ConsoleUI.SetActive(true);
			movingEnemy.SetActive(true);
			isActive = false;
			PlayerController.instance.canMove = true;
			textIn.Select();
		}
	}

	public IEnumerator EnemyMoveCo()
	{
		turnWaiting = false;
		yield return new WaitForSeconds(1f);
		EnemyAttack();
		yield return new WaitForSeconds(1f);
		NextTurn();
	}


	public void EnemyAttack()
	{
		List<int> players = new List<int>();
		int moveP = 0;
		for (int i = 0; i < activeBattler.Count; i++)
		{
			if (activeBattler[i].notEnemy && activeBattler[i].currentHp > 0)
			{
				players.Add(i);
			}
		}

		int enemyTarget = players[0];

		//activeBattler[enemyTarget].currentHp -= 30;
		Instantiate(moveList[0].attackAnimation, activeBattler[enemyTarget].transform.position, activeBattler[enemyTarget].transform.rotation);
		moveP = moveList[0].movePower;
		Instantiate(enemyAttackEffect, activeBattler[currentTurn].transform.position, activeBattler[currentTurn].transform.rotation);
		DealDamage(enemyTarget, moveP);
	}

	public void DealDamage(int target, int movePower)
	{
		float totalAttack = activeBattler[currentTurn].strength + activeBattler[currentTurn].attackPower;
		float totalDefence = activeBattler[target].defence + activeBattler[target].armor;

		float damageDelt = (totalAttack/totalDefence) * movePower * Random.Range(.9f, 1.1f);
		int damageGiven = Mathf.RoundToInt(damageDelt);

		Debug.Log(activeBattler[currentTurn].playerName + " is dealing " + damageDelt + "(" + damageGiven + ") damage to " + activeBattler[target].playerName);

		activeBattler[target].currentHp -= damageGiven;
		Instantiate(theDamage, activeBattler[target].transform.position, activeBattler[target].transform.rotation).SetDamage(damageGiven);

		UpdateUIStats();
	}

	public void DealDamageFire(int target, int movePower)
	{
		float totalAttack = activeBattler[currentTurn].strength + activeBattler[currentTurn].attackPower;
		float totalDefence = activeBattler[target].armor * 2;

		float damageDelt = (totalAttack / totalDefence) * movePower * Random.Range(.9f, 1.1f);
		int damageGiven = Mathf.RoundToInt(damageDelt);

		Debug.Log(activeBattler[currentTurn].playerName + " is dealing " + damageDelt + "(" + damageGiven + ") damage to " + activeBattler[target].playerName);

		activeBattler[target].currentHp -= damageGiven;
		Instantiate(theDamage, activeBattler[target].transform.position, activeBattler[target].transform.rotation).SetDamage(damageGiven);

		UpdateUIStats();
	}

	public void HealPlayer(int target, int movePower)
	{
		float damageDelt = movePower * Random.Range(.9f, 1.1f);
		int damageGiven = Mathf.RoundToInt(damageDelt);

		Debug.Log(activeBattler[currentTurn].playerName + " is dealing " + damageDelt + "(" + damageGiven + ") damage to " + activeBattler[target].playerName);

		activeBattler[target].currentHp += damageGiven;
		if (activeBattler[target].currentHp >= activeBattler[target].maxHp)
		{
			activeBattler[target].currentHp = activeBattler[target].maxHp;
		}
		Instantiate(theDamage, activeBattler[target].transform.position, activeBattler[target].transform.rotation).SetDamage(damageGiven);

		UpdateUIStats();
	}

	public void UpdateUIStats()
	{
		for (int i = 0; i < playerName.Length; i++)
		{
			if (activeBattler.Count > i)
			{
				if(activeBattler[i].notEnemy)
				{
					BattleChar player = activeBattler[i];
					playerName[i].gameObject.SetActive(true);
					playerName[i].text = player.playerName;
					playerHp[i].text = Mathf.Clamp(player.currentHp, 0, int.MaxValue) + "/" + player.maxHp;
				}

				else
				{
					playerName[i].gameObject.SetActive(false);
				}
			}

			else
			{
				playerName[i].gameObject.SetActive(false);
			}
		}
	}

	public void PlayerAttack(string moveName /*,int selectedTarget*/)
	{
		int selectedTarget = 1;
		int movePower = 0;
		
		Instantiate(moveList[0].attackAnimation, activeBattler[selectedTarget].transform.position, activeBattler[selectedTarget].transform.rotation);
		movePower = moveList[0].movePower;
		
		Instantiate(enemyAttackEffect, activeBattler[currentTurn].transform.position, activeBattler[currentTurn].transform.rotation);

		DealDamage(selectedTarget, movePower);

		uiButtons.SetActive(false);
		//targetMenu.SetActive(false);

		NextTurn();
	}

	public void PlayerHeal(string moveName /*,int selectedTarget*/)
	{
		int selectedTarget = 0;
		int movePower = 0;
	
		Instantiate(moveList[1].attackAnimation, activeBattler[selectedTarget].transform.position, activeBattler[selectedTarget].transform.rotation);
		movePower = moveList[1].movePower;
	

		Instantiate(enemyAttackEffect, activeBattler[currentTurn].transform.position, activeBattler[currentTurn].transform.rotation);

		HealPlayer(selectedTarget, movePower);

		uiButtons.SetActive(false);
		//targetMenu.SetActive(false);

		NextTurn();
	}

	public void PlayerFire(string moveName /*,int selectedTarget*/)
	{
		int selectedTarget = 1;
		int movePower = 0;

		Instantiate(moveList[2].attackAnimation, activeBattler[selectedTarget].transform.position, activeBattler[selectedTarget].transform.rotation);
		movePower = moveList[2].movePower;

		Instantiate(enemyAttackEffect, activeBattler[currentTurn].transform.position, activeBattler[currentTurn].transform.rotation);

		DealDamageFire(selectedTarget, movePower);

		uiButtons.SetActive(false);
		//targetMenu.SetActive(false);

		NextTurn();
	}

	public void Flee()
	{
		
			int fleeSuccess = Random.Range(0, 100);
			if (fleeSuccess < chanceToFlee)
			{
				//end the battle
				//battleActive = false;
				//battleScene.SetActive(false);
				fleeing = true;
				StartCoroutine(EndBattleCo());
			}
			else
			{
				NextTurn();
			}
	}

	public IEnumerator EndBattleCo()
	{
		isActive = false;
		uiButtons.SetActive(false);

		yield return new WaitForSeconds(.5f);

		for (int i = 0; i < activeBattler.Count; i++)
		{
			Destroy(activeBattler[i].gameObject);
		}

		battleScene.SetActive(false);
		activeBattler.Clear();
		currentTurn = 0;
		if (fleeing)
		{
			fleeing = false;
			enemyPresent = 1;
			battleScene.SetActive(false);
			ConsoleUI.SetActive(true);
			movingEnemy.SetActive(true);
			isActive = false;
			PlayerController.instance.canMove = true;
			textIn.Select();
		}
		else
		{
			if (trollPresent == 1)
			{
				Troll.trollDeath = true;
				trollPresent = 0;
			}

			else if (batPresent == 1)
			{
				Bat.batDeath = true;
				batPresent = 0;
			}

			else if (cyclopsPresent == 1)
			{
				Cyclops.cyclopsDeath = true;
				cyclopsPresent = 0;

			}

			enemyPresent = 0;
			KillEnemy();
			battleScene.SetActive(false);
			ConsoleUI.SetActive(true);
			movingEnemy.SetActive(true);
			isActive = false;
			PlayerController.instance.canMove = true;
			textIn.Select();
		}
	}


	

	public IEnumerator GameOverCo()
	{
		isActive = false;
		battleScene.SetActive(false);
	//	UIFade.instance.FadeToBlack();
		yield return new WaitForSeconds(1.5f);
		battleScene.SetActive(false);
		SceneManager.LoadScene(gameOver);
	}

	public void KillEnemy()
	{
		if (enemyType == 1)
		{
			Troll.trollDeath = true;
 		}

		if (enemyType == 2)
		{
			Bat.batDeath = true;
		}

		if (enemyType == 3)
		{
			Cyclops.cyclopsDeath = true;
		}
	}

}
