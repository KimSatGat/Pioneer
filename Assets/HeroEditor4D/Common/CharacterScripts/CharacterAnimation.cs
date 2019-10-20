using System;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.CharacterScripts
{
	/// <summary>
	/// Used to play animations.
	/// </summary>
	public class CharacterAnimation : MonoBehaviour
	{
		public Character4D Character;
		public Animator Animator;

		public void SetState(CharacterState state)
		{
			Animator.SetInteger("State", (int) state);
		}

		public void Attack()
		{
			switch (Character.WeaponType)
			{
				case WeaponType.Melee1H:
				case WeaponType.Melee2H:
					Slash1H();
					break;
				case WeaponType.Bow:
					ShotBow();
					break;
				default:
					throw new NotImplementedException("This feature may be implemented in next updates.");
			}
		}

		public void Slash1H()
		{
			Animator.SetTrigger("Slash1H");
		}

		public void ShotBow()
		{
			Animator.SetTrigger("ShotBow");
		}
	}
}