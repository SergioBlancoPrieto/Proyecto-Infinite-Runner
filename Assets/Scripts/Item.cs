using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	health,
	money,
	mana
}

public class Item : MonoBehaviour
{
	public ItemType type;
	public AudioClip itemSound;
	public int value;

	void Show()
	{
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;     
	}

	void Hide()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	void Collect()
	{
		Hide();
		//GameManager.sharedInstance.CollectItem(value);
		AudioSource.PlayClipAtPoint(itemSound, gameObject.transform.position, 1f);

		switch (type)
		{
			case ItemType.money:
			{
				GameManager.sharedInstance.CollectItem(value);
				break;
			}
			case ItemType.health:
			{
				PlayerController.sharedInstance.CollectHealth(value);
				break;
			}
			case ItemType.mana:
			{
				PlayerController.sharedInstance.BuffPlayer();
				break;
			}
		}
	}
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
}
