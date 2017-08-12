using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
	string color;
	public int index;
	public Vector2 orb_position;
    bool isMoving = false;

	GameManager gm;

	void Start ()
	{
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}

	//void Update (){}

	public void setOrb (int _index, string _color)
	{
		index = _index;
		color = _color;
	}

	public int getOrbIndex()
	{
		return index;
	}

	public void setOrbIndex(int _index)
	{
		index = _index;
	}

    public void setCatalogedPosition(Vector2 _position)
    {
        orb_position = _position;
    }

	public Vector2 getOrbPosition()
	{
		return orb_position;
	}

	public void setPosition ()
	{
		transform.position = orb_position;
	}

    public void setPosition (Vector2 _position)
	{
		transform.position = orb_position = _position;
		//Debug.Log (string.Format("displayBoard: Index: {2} X: {0} Y: {1}", position.x, position.y, index));
	}

	void OnMouseDown ()
	{
		Debug.Log ("Mouse Down");
        gameObject.AddComponent<Rigidbody2D>();
		gameObject.GetComponent<CircleCollider2D> ().radius = 0.01f;
        isMoving = true;
		gm.selectedOrb = gameObject;
		gm.selectedOrb_S = this;
	}

	void OnMouseUp ()
	{
		Debug.Log ("Mouse Up");
        Destroy(gameObject.GetComponent<Rigidbody2D>());
		gameObject.GetComponent<CircleCollider2D> ().radius = 0.255f;
        isMoving = false;
        gm.selectedOrb = null;
		gm.selectedOrb_S = null;
        gm.moveOrbByIndex(this);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log ("Collided!");
		if (coll.gameObject.tag == "Orb" && isMoving) 
		{
			gm.switchOrb = coll.gameObject;
			gm.switchOrb_S = coll.gameObject.GetComponent<Orb> ();
		}
	}
}
