using NavMeshPlus.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleScript : MonoBehaviour
{
	[field: SerializeField] public Obstacle Obstacle { get; set; }
	private Player player;

	public void Setup()
	{
		this.GetComponent<SpriteRenderer>().sprite = Obstacle.Sprite;

		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		Movable();
		if (Obstacle.Passable) Passable();
		if (Obstacle.Destroyable) Destroyable();
		if (Obstacle.Carnivore || Obstacle.Herbivore) Edible();
	}

	private void Passable()
	{
		Collider2D col = this.GetComponent<Collider2D>();
		col.isTrigger = true;
		if (Obstacle.HigherThanPlayer) this.GetComponent<SpriteRenderer>().sortingLayerName = "HighObstacle";
	}
	private void Movable()
	{
		if (Obstacle.Movable)
		{
			Rigidbody2D rb = this.AddComponent<Rigidbody2D>();
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.angularDrag = 5;
			rb.gravityScale = 0;
			rb.drag = 5;
			rb.freezeRotation = true;
			rb.mass = Obstacle.size switch
			{
				Enums.ObstacleSize.Small => 1f,
				Enums.ObstacleSize.Medium => 3f,
				Enums.ObstacleSize.Large => 10f,
				_ => 2f
			};
		}
		else if (!Obstacle.Passable)
		{
			NavMeshObstacle navObs = this.AddComponent<NavMeshObstacle>();
			navObs.shape = NavMeshObstacleShape.Box;
			float actualWidth = this.GetComponent<SpriteRenderer>().bounds.size.x / transform.localScale.x;

			navObs.size = new Vector3(actualWidth, actualWidth, 1f);
			navObs.carving = true;
			navObs.carveOnlyStationary = true;
		}


	}
	private void Destroyable()
	{
		this.AddComponent<DestroyableObstacle>();
	}

	private void Edible()
	{
		if (InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability].ItemName == "Carnivore" || InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability].ItemName == "Herbivore")
		{
			player.HealUnit(Obstacle.HP);
			Destroy(this.gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Unit>())
			collision.GetComponent<Unit>().MoveSpeed += Obstacle.SpeedModifier;
		if (Obstacle.HigherThanPlayer && collision.CompareTag("Player"))
		{
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			Color tmpColor = sr.color;
			tmpColor.a = 0.5f;
			sr.color = tmpColor;
		}
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			collision.gameObject.GetComponent<Player>().DamageUnit(Obstacle.ContactDamage, 1f);
		}
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Unit>())
			collision.GetComponent<Unit>().MoveSpeed -= Obstacle.SpeedModifier;
		if (Obstacle.HigherThanPlayer && collision.CompareTag("Player"))
		{
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			Color tmpColor = sr.color;
			tmpColor.a = 1f;
			sr.color = tmpColor;
		}

	}
	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			collision.gameObject.GetComponent<Player>().DamageUnit(Obstacle.ContactDamage, 1f);
		}
	}
	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			collision.gameObject.GetComponent<Player>().DamageUnit(Obstacle.ContactDamage, 1f);
		}
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			collision.gameObject.GetComponent<Player>().DamageUnit(Obstacle.ContactDamage, 1f);
		}
	}

}
