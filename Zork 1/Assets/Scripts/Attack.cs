using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zork 1/Actions/Attack")]

public class Attack : InputAction
{
	
	public override void RespondToInput(GameController controller, string[] separatedInputWords)
	{
		if (controller.typeOfEnemy > 0)
		{ 
			if (separatedInputWords.Length > 1)
			{
				controller.battle.BattleStart(separatedInputWords[1]);
			}
		}
	}
}
