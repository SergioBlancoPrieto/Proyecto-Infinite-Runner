using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	health,
	money
}

public class Item : MonoBehaviour
{
	public ItemType type;
	public AudioClip itemSound;
	private bool isCollected;
	public int value;

	void Show()
	{
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<CircleCollider2D>().enabled = true;
		isCollected = false;
	}

	void Hide()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
	}

	void Collect()
	{
		isCollected = true;
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
