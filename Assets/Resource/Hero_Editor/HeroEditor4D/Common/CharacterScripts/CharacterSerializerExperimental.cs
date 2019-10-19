using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.EditorScripts;
using HeroEditor.Common;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
	public partial class Character
	{
		public override string ToJson()
		{
			var sc = SpriteCollection.Instance;

			if (sc == null) throw new Exception("SpriteCollection is missed on scene!");

			var description = new SerializableDictionary<string, string>
			{
				{ "Head", GetSpriteEntryId(sc.Head, Head) },
				{ "Body", GetSpriteEntryId(sc.Body, Body) },
				{ "Ears", GetSpriteEntryId(sc.Ears, Ears) },
				{ "Hair", GetSpriteEntryId(sc.Hair, Hair) },
				{ "Beard", GetSpriteEntryId(sc.Beard, Beard) },
				{ "Helmet", GetSpriteEntryId(sc.Helmet, Helmet) },
				{ "Glasses", GetSpriteEntryId(sc.Glasses, Glasses) },
				{ "Mask", GetSpriteEntryId(sc.Mask, Mask) },
				{ "Armor", GetSpriteEntryId(sc.Armor, Armor) },
				{ "PrimaryMeleeWeapon", GetSpriteEntryId(GetWeaponCollection(WeaponType), PrimaryMeleeWeapon) },
				{ "SecondaryMeleeWeapon", GetSpriteEntryId(GetWeaponCollection(WeaponType), SecondaryMeleeWeapon) },
				{ "Cape", GetSpriteEntryId(sc.Cape, Cape) },
				{ "Back", GetSpriteEntryId(sc.Back, Back) },
				{ "Shield", GetSpriteEntryId(sc.Shield, Shield) },
				{ "Bow", GetSpriteEntryId(sc.Bow, Bow) },
				{ "Firearms", GetSpriteEntryId(GetWeaponCollection(WeaponType), Firearms) },
				{ "FirearmParams", Firearm.Params.Name },
				{ "WeaponType", WeaponType.ToString() },
				{ "Expression", Expression }
			};

			foreach (var expression in Expressions)
			{
				description.Add(string.Format("Expression.{0}.Eyebrows", expression.Name), GetSpriteEntryId(sc.Eyebrows, expression.Eyebrows));
				description.Add(string.Format("Expression.{0}.Eyes", expression.Name), GetSpriteEntryId(sc.Eyes, expression.Eyes));
				description.Add(string.Format("Expression.{0}.Mouth", expression.Name), GetSpriteEntryId(sc.Mouth, expression.Mouth));
			}

			return JsonUtility.ToJson(description);
		}

		public override void LoadFromJson(string serialized)
		{
			var description = JsonUtility.FromJson<SerializableDictionary<string, string>>(serialized);
			var sc = SpriteCollection.Instance;

			if (sc == null) throw new Exception("SpriteCollection is missed on scene!");

			Head = FindSpriteById(sc.Head, description["Head"]);
			Body = FindSpritesById(sc.Body, description["Body"]);
			Ears = FindSpriteById(sc.Ears, description["Ears"]);
			Hair = FindSpriteById(sc.Hair, description["Hair"]);
			Beard = FindSpriteById(sc.Beard, description["Beard"]);
			Helmet = FindSpriteById(sc.Helmet, description["Helmet"]);
			Glasses = FindSpriteById(sc.Glasses, description["Glasses"]);
			Mask = FindSpriteById(sc.Mask, description["Mask"]);
			Armor = FindSpritesById(sc.Armor, description["Armor"]);
			WeaponType = (WeaponType) Enum.Parse(typeof(WeaponType), description["WeaponType"]);
			PrimaryMeleeWeapon = FindSpriteById(GetWeaponCollection(WeaponType), description["PrimaryMeleeWeapon"]);
			SecondaryMeleeWeapon = FindSpriteById(GetWeaponCollection(WeaponType), description["SecondaryMeleeWeapon"]);
			Cape = FindSpriteById(sc.Cape, description["Cape"]);
			Back = FindSpriteById(sc.Mask, description["Back"]);
			Shield = FindSpriteById(sc.Shield, description["Shield"]);
			Bow = FindSpritesById(sc.Bow, description["Bow"]);
			Firearms = FindSpritesById(GetWeaponCollection(WeaponType), description["Firearms"]);
			//Firearm.Params = string.IsNullOrEmpty(description["FirearmParams"]) || FirearmCollection.Instance.Firearms == null ? new FirearmParams() : FirearmCollection.Instance.Firearms.Single(i => i.Name == description["FirearmParams"]);
			Expression = description["Expression"];
			Expressions = new List<Expression>();

			if (string.IsNullOrEmpty(description["FirearmParams"]))
			{
				Firearm.Params = new FirearmParams();
			}
			else
			{
				if (FirearmCollection.Instance == null) throw new Exception("FirearmCollection is missed on scene!");

				var firearmParams = FirearmCollection.Instance.Firearms.Single(i => i.Name == description["FirearmParams"]);

				if (firearmParams == null) throw new Exception(string.Format("FirearmCollection doesn't contain a definition for {0}!", description["FirearmParams"]));

				Firearm.Params = firearmParams;
			}

			foreach (var key in description.Keys)
			{
				if (key.Contains("Expression."))
				{
					var parts = key.Split('.');
					var expressionName = parts[1];
					var expressionPart = parts[2];
					var expression = Expressions.SingleOrDefault(i => i.Name == expressionName);

					if (expression == null)
					{
						expression = new Expression { Name = expressionName };
						Expressions.Add(expression);
					}

					switch (expressionPart)
					{
						case "Eyebrows":
							expression.Eyebrows = FindSpriteById(sc.Eyebrows, description[key]);
							break;
						case "Eyes":
							expression.Eyes = FindSpriteById(sc.Eyes, description[key]);
							break;
						case "Mouth":
							expression.Mouth = FindSpriteById(sc.Mouth, description[key]);
							break;
						default:
							throw new NotSupportedException(expressionPart);
					}
				}
			}

			Initialize();
			UpdateAnimation();
		}

		private static IEnumerable<SpriteGroupEntry> GetWeaponCollection(WeaponType weaponType)
		{
			switch (weaponType)
			{
				case WeaponType.Melee1H: return SpriteCollection.Instance.MeleeWeapon1H;
				case WeaponType.MeleePaired: return SpriteCollection.Instance.MeleeWeapon1H;
				case WeaponType.Melee2H: return SpriteCollection.Instance.MeleeWeapon2H;
				case WeaponType.Bow: return SpriteCollection.Instance.Bow;
				case WeaponType.Firearms1H: return SpriteCollection.Instance.Firearms1H;
				case WeaponType.FirearmsPaired: return SpriteCollection.Instance.Firearms1H;
				case WeaponType.Firearms2H: return SpriteCollection.Instance.Firearms2H;
				case WeaponType.Supplies: return SpriteCollection.Instance.Supplies;
				default:
					throw new NotSupportedException(weaponType.ToString());
			}
		}

		private static string GetSpriteEntryId(IEnumerable<SpriteGroupEntry> collection, Sprite sprite)
		{
			if (sprite == null) return null;

			var entry = collection.SingleOrDefault(i => i.Sprite == sprite);

			if (entry == null) throw new Exception(string.Format("Can't find {0} in SpriteCollection.", sprite.name));
			
			return entry.Id;
		}

		private static string GetSpriteEntryId(IEnumerable<SpriteGroupEntry> collection, List<Sprite> sprites)
		{
			if (sprites == null || sprites.Count == 0) return null;

			return GetSpriteEntryId(collection, sprites[0]);
		}

		private static Sprite FindSpriteById(IEnumerable<SpriteGroupEntry> collection, string id)
		{
			if (string.IsNullOrEmpty(id)) return null;

			var entries = collection.Where(i => i.Id == id).ToList();

			switch (entries.Count)
			{
				case 1:
					return entries[0].Sprite;
				case 0:
					throw new Exception(string.Format("Entry with id {0} not found in SpriteCollection.", id));
				default:
					throw new Exception(string.Format("Multiple entries with id {0} found in SpriteCollection.", id));
			}
		}

		private static List<Sprite> FindSpritesById(IEnumerable<SpriteGroupEntry> collection, string id)
		{
			if (string.IsNullOrEmpty(id)) return new List<Sprite>();

			var entries = collection.Where(i => i.Id == id).ToList();

			switch (entries.Count)
			{
				case 1:
					return entries[0].Sprites;
				case 0:
					throw new Exception(string.Format("Entry with id {0} not found in SpriteCollection.", id));
				default:
					throw new Exception(string.Format("Multiple entries with id {0} found in SpriteCollection.", id));
			}
		}
	}
}