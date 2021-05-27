using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour
{

	public bool black;
	public bool white;
	public bool purple;
	public bool metal;


	public GameObject bloonPrefab;
	public BezierSpline spline;
	public float duration;
	public float progress;
	public int health = 1;
	public BezierSpline track;
	public GameObject lastHitBy;
	public WaveManager waveManager;
	public Color bloonColour= Color.red;
	public MeshRenderer skin;
	public bool camo;
	public Material rainbow;
	public Material ceramic;
	public Material camoflage;
	public Material zebra;
	public Material whitemat;


	public List<Projectile.damageType> immunities;

	private int armor;
	public int RBE;
	private bool movingForward = true;
	private bool dead;



	private void Start()
	{

		skin = gameObject.GetComponent<MeshRenderer>();
		ChangeForm();


	}

	private void Update()
	{
		if (movingForward)
		{
			progress += Time.deltaTime / duration;
			if (progress > 1f)
			{			
					progress = 1f;	
			}
		}
		else
		{
			progress -= Time.deltaTime / duration;
			if (progress < 0f)
			{
				progress = - progress;
				movingForward = true;
            }
		}
		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
		if(progress == 1)
        {

			FindObjectOfType<WaveManager>().TakeDamage(RBE);
			Destroy(gameObject);
        }
 	}

	///////////////////////////////////////////DAMAGE CALCULATIONS///////////////////////////////////////////////////
	public void TakeDamage(int amount, GameObject hitBy, Projectile.damageType damageType)
    {
		Physics.IgnoreCollision(GetComponent<SphereCollider>(), hitBy.GetComponent<SphereCollider>());

		
			if (!immunities.Contains(damageType))
			{


				int remainingDamage = (armor - amount) * -1;
				armor = armor - amount;
				if (armor <= 0)
				{
					black = false;
					//white = false;

					FindObjectOfType<WaveManager>().AddCash(1);
					hitBy.GetComponent<Projectile>().thrower.popcount++;
					CheckExceptions(remainingDamage, hitBy, damageType);
					health = health - 1;
					ChangeForm();
					if (health <= 0)
					{
						Destroy(gameObject);
						dead = true;

					}
					if (remainingDamage >= 0 && health >= 0 && !dead)
					{
						TakeDamage(remainingDamage, hitBy, damageType);
					}
				}
			}
		
	}
	
	public void SpawnBabyBloon(int hp, int amount, int remainingDamage, GameObject hitby, string exception, Projectile.damageType damageType)

	{
        GameObject newbloon = Instantiate(bloonPrefab);
        Physics.IgnoreCollision(hitby.GetComponent<SphereCollider>(), newbloon.GetComponent<SphereCollider>());
		newbloon.GetComponent<Bloon>().immunities = immunities;
		newbloon.GetComponent<Bloon>().health = hp;
		newbloon.GetComponent<Bloon>().ChangeForm();

        newbloon.GetComponent<Bloon>().progress = progress + .02f;
		newbloon.GetComponent<Bloon>().TakeDamage(remainingDamage, hitby, damageType);
		newbloon.GetComponent<Bloon>().metal = false;

		if (exception == "black") {
			black = true;
			white = false;
			newbloon.GetComponent<Bloon>().black = true;
			newbloon.GetComponent<Bloon>().white = false;
			newbloon.GetComponent<Bloon>().ChangeForm();

		}
		else if(exception == "white")
        {
			newbloon.GetComponent<Bloon>().progress = progress + .035f;
			newbloon.GetComponent<Bloon>().white = true;
			newbloon.GetComponent<Bloon>().black = false;

			newbloon.GetComponent<Bloon>().ChangeForm();

			black = false;
			white = true;
		}
		newbloon.GetComponent<Bloon>().ChangeForm();


		if (amount == 2)
        {
            newbloon.GetComponent<Bloon>().SpawnBabyBloon(hp, 1, remainingDamage, hitby, "a", damageType);
        }
        if (amount == 3)
        {
            newbloon.GetComponent<Bloon>().SpawnBabyBloon(hp, 2, remainingDamage, hitby, "a", damageType);
		}
	}












	///////////////////////////////////////////MESS DOWN HERE///////////////////////////////////////////////////
	///////////////////////////////////////////MESS DOWN HERE///////////////////////////////////////////////////
	void CheckExceptions(int remainingDamage, GameObject hitby, Projectile.damageType damageType)
    {
		if (health == 6)
		{
			SpawnBabyBloon(5, 1,remainingDamage,hitby, "a", damageType);
		}
		if (health == 7)
		{
            if (metal)
            {
			SpawnBabyBloon(6, 1, remainingDamage, hitby, "black", damageType);
			metal = false;
			black = true;

			}
			else
            {
			SpawnBabyBloon(6, 1, remainingDamage, hitby, "black", damageType);
			SpawnBabyBloon(6, 1, remainingDamage, hitby, "white", damageType);
			dead = true;
			Destroy(gameObject);
            }

		}
		if (health == 8)
		{
			SpawnBabyBloon(7, 1, remainingDamage, hitby, "a", damageType);
		}
		if (health == 9)
		{
			SpawnBabyBloon(8, 1, remainingDamage, hitby, "a", damageType);
		}
		if (health == 10)
		{
			SpawnBabyBloon(9, 3, remainingDamage, hitby, "a", damageType);
		}
		if (health == 11)
		{
			SpawnBabyBloon(10, 3, remainingDamage, hitby, "a", damageType);
		}
		if (health == 12)
		{
			SpawnBabyBloon(11, 3, remainingDamage, hitby, "a", damageType);
		}
		if (health == 13)
		{
			SpawnBabyBloon(12, 3, remainingDamage, hitby, "a", damageType);
		}


	}
	void ChangeForm()
	{
		switch (health)
		{
			case 1:
				transform.localScale = new Vector3(.63f, .9f, .63f);
				RBE = 1;
				bloonColour = Color.red;
				immunities = new List<Projectile.damageType> {};

				armor = 1;
				break;
			case 2:
				transform.localScale = new Vector3(.77f, 1.1f, .77f);
				RBE = 2;
				bloonColour = Color.blue;
				armor = 1;
				immunities = new List<Projectile.damageType> {};

				break;
			case 3:
				RBE = 3;
				bloonColour = Color.green;
				transform.localScale = new Vector3(.77f, 1.1f, .77f);
				armor = 1;
				immunities = new List<Projectile.damageType> { };

				break;
			case 4:
				RBE = 4;
				transform.localScale = new Vector3(.77f, 1.1f, .77f);
				bloonColour = Color.yellow;
				armor = 1;
				immunities = new List<Projectile.damageType> { };

				break;
			case 5:
				RBE = 5;
				transform.localScale = new Vector3(.84f, 1.2f, .84f);
				bloonColour = Color.magenta;
				armor = 1;
				immunities = new List<Projectile.damageType> {};

				break;
			case 6:
				RBE = 11;
				armor = 1;
		
                if(white) { 
					bloonColour = Color.white;
					gameObject.GetComponent<MeshRenderer>().material = whitemat;
					immunities = new List<Projectile.damageType> { Projectile.damageType.ice };

				}
				if (purple)
                {
					bloonColour = Color.magenta;
					gameObject.GetComponent<MeshRenderer>().material = whitemat;
					immunities = new List<Projectile.damageType> { Projectile.damageType.energy };

				}
				if (black)
                {

					bloonColour = Color.black;
					gameObject.GetComponent<MeshRenderer>().material = whitemat;
					immunities = new List<Projectile.damageType> { Projectile.damageType.explosive};


				}


				transform.localScale = new Vector3(.7f, .7f, .7f);
				armor = 1;
				break;
			case 7:
				RBE = 23;
				transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                if (metal)
                {
					bloonColour = Color.grey;
					gameObject.GetComponent<MeshRenderer>().material = whitemat;
					immunities = new List<Projectile.damageType> {Projectile.damageType.sharp };

				}
				else
                {

				black = true;
				white = true;
				bloonColour = Color.white;
				gameObject.GetComponent<MeshRenderer>().material = zebra;
				immunities = new List<Projectile.damageType> { Projectile.damageType.explosive, Projectile.damageType.ice };


				}


				armor = 1;
				break;
			case 8:
				RBE = 47;
				transform.localScale = new Vector3(.9f, 1.3f, .9f);
				gameObject.GetComponent<MeshRenderer>().material = rainbow;
				bloonColour = Color.white;
				immunities = new List<Projectile.damageType> {};
				armor = 1;
				break;
			case 9:
				RBE = 104;
				transform.localScale = new Vector3(.9f, 1.3f, .9f);
				gameObject.GetComponent<MeshRenderer>().material = ceramic;
				bloonColour = Color.white;
				armor = 10;
				break;
			case 10:
				RBE = 616;
				transform.localScale = new Vector3(3f, 2f, 2f);
				bloonColour = Color.blue;
				armor = 200;
				break;
			case 11:
				RBE = 3164;
				transform.localScale = new Vector3(4f, 3f, 3f);
				bloonColour = Color.red;
				armor = 700;
				break;
			case 12:
				RBE = 16656;
				transform.localScale = new Vector3(5f, 4f, 4);
				bloonColour = Color.green;
				armor = 4000;
				break;
			case 13:
				RBE = 55760;
				transform.localScale = new Vector3(5.5f, 4.5f, 4.5f);
				bloonColour = Color.magenta;
				armor = 20000;
				break;

		}

				skin.material.color = bloonColour;
        if (camo)
        {
			gameObject.layer = 13;
        }
        else
        {
			gameObject.layer = 0;
        }
	}
}
