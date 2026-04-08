using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyableObstacle : Unit, IPointerEnterHandler, IPointerExitHandler
{
    private Obstacle Obstacle;

    public override void Awake()
    {
        Obstacle = this.GetComponent<ObstacleScript>().Obstacle;
        CurrentHealth = Obstacle.HP;
    }
    public override void DamageUnit(int damageAmount)
    {
        base.DamageUnit(damageAmount);
        if (CurrentHealth <= 0)
        {
            if (Obstacle.Explosive) Explode();
            Destroy(this.gameObject);
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(Obstacle.ObstacleExplosionPrefab, transform.position, Quaternion.identity);
        float explosionSize = Obstacle.ExplodeRange + Obstacle.GetWorldRadius();
        explosion.transform.localScale = new Vector2(explosionSize, explosionSize);
        WaitForBomboclat();

        this.GetComponent<Collider2D>().enabled = false;
        Collider2D[] victims = Physics2D.OverlapCircleAll(transform.position, Obstacle.ExplodeRange + Obstacle.GetWorldRadius());
        foreach (Collider2D victim in victims)
        {
            if (victim == this.gameObject) continue;
            if (victim.gameObject.GetComponent<Unit>())
            {
                victim.gameObject.GetComponent<Unit>().DamageUnit(Obstacle.ExplodeDamage);
            }
        }
    }
    public IEnumerator WaitForBomboclat()
    {
        yield return new WaitForSeconds(1);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Obstacle.Explosive)
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(Obstacle.Explosive)
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
