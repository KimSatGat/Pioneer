using System.Linq;
using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor.Common;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.ExampleScripts
{
	/// <summary>
	/// Changing equipment at runtime examples.
	/// </summary>
	public class RuntimeSetup : MonoBehaviour
	{
		public Character Character;

		/// <summary>
		/// Example call: SetBody("HeadScar", "Basic", "Human", "Basic");
		/// </summary>
		public void SetBody(string headName, string headCollection, string bodyName, string bodyCollection)
		{
			var body = SpriteCollection.Instance.Body.Single(i => i.Name == bodyName && i.Collection == bodyCollection);

			Character.SetBody( body.Sprites);
		}

		public void EquipMeleeWeapon1H(string sname, string collection)
		{
			var entry = SpriteCollection.Instance.MeleeWeapon1H.Single(i => i.Name == sname && i.Collection == collection);
			
			Character.EquipMeleeWeapon(entry.Sprite);
		}

		public void EquipMeleeWeapon2H(string sname, string collection)
		{
			var entry = SpriteCollection.Instance.MeleeWeapon2H.Single(i => i.Name == sname && i.Collection == collection);

			Character.EquipMeleeWeapon(entry.Sprite, twoHanded: true);
		}

		public void EquipBow(string sname, string collection)
		{
			var sprites = SpriteCollection.Instance.Bow.Single(i => i.Name == sname && i.Collection == collection).Sprites;

			Character.EquipBow(sprites);
		}

		public void EquipShield(string sname, string collection)
		{
			var sprite = SpriteCollection.Instance.Shield.Single(i => i.Name == sname && i.Collection == collection).Sprite;

			Character.EquipShield(sprite);
		}

		public void EquipArmor(string sname, string collection)
		{
			var sprites = SpriteCollection.Instance.Armor.Single(i => i.Name == sname && i.Collection == collection).Sprites;

			Character.EquipArmor(sprites);
		}

		public void EquipHelmet(string sname, string collection)
		{
			var sprite = SpriteCollection.Instance.Armor.Single(i => i.Name == sname && i.Collection == collection).Sprite;

			Character.EquipHelmet(sprite);
		}
	}
}