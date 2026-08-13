using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class InventoryLogicTests
{
	GameObject dummyObject;
	Item dummyItem;
	Inventory dummyInventory;
	InventoryLogic logic;
	[SetUp]
	public void Setup()
	{
		dummyObject = new GameObject();
		dummyItem = ScriptableObject.CreateInstance<Item>();
		logic = dummyObject.AddComponent<InventoryLogic>();
		dummyInventory = ScriptableObject.CreateInstance<Inventory>();
		InventoryLogic.ActiveInventory = dummyInventory;
		dummyInventory.Init(10); //10 slots
	}

	[Test]
	public void ObtainItemTest()
	{
		InventoryLogic.ObtainItem(dummyItem);

		Assert.AreEqual(InventoryLogic.ActiveInventory.slots[0], dummyItem);
	}

	[Test]
	public void EquipWeaponTest()
	{
		dummyItem.ItemTag = Enums.SlotTag.Weapon;
		InventoryLogic.ObtainItem(dummyItem);

		InventoryLogic.EquipItem(dummyItem);

		Assert.AreEqual(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Weapon], dummyItem);
	}

	[Test]
	public void EquipArmorTest()
	{
		dummyItem.ItemTag = Enums.SlotTag.Armor;
		InventoryLogic.ObtainItem(dummyItem);

		InventoryLogic.EquipItem(dummyItem);

		Assert.AreEqual(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Armor], dummyItem);
	}

	[Test]
	public void EquipAccessoryTest()
	{
		dummyItem.ItemTag = Enums.SlotTag.Accessory;
		InventoryLogic.ObtainItem(dummyItem);

		InventoryLogic.EquipItem(dummyItem);

		Assert.AreEqual(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Accessory], dummyItem);
	}

	[Test]
	public void EquipAbilityTest()
	{
		dummyItem.ItemTag = Enums.SlotTag.Ability;
		InventoryLogic.ObtainItem(dummyItem);

		InventoryLogic.EquipItem(dummyItem);

		Assert.AreEqual(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability], dummyItem);
	}

	[Test]
	public void EquipAlreadyEquippedSlotTest()
	{
		dummyItem.ItemTag = Enums.SlotTag.Weapon;
		Item secondDummyItem = ScriptableObject.CreateInstance<Item>();
		secondDummyItem.ItemTag = Enums.SlotTag.Weapon;
		InventoryLogic.ObtainItem(dummyItem);
		InventoryLogic.ObtainItem(secondDummyItem);
		InventoryLogic.EquipItem(dummyItem);

		InventoryLogic.EquipItem(secondDummyItem);

		Assert.AreEqual(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Weapon], secondDummyItem);
		Assert.AreEqual(InventoryLogic.ActiveInventory.slots[0], dummyItem);
	}

	[Test]
	public void UnEquipItemTest()
	{
		InventoryLogic.ObtainItem(dummyItem);
		dummyItem.ItemTag = Enums.SlotTag.Weapon;
		InventoryLogic.EquipItem(dummyItem);

		InventoryLogic.UnEquipItem((int)Enums.SlotTag.Weapon);

		Assert.AreEqual(InventoryLogic.ActiveInventory.slots[0], dummyItem);
		Assert.IsNull(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Weapon]);
	}

}
