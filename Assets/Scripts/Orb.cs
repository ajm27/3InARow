using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
	string color;
	public int index;
	public Vector2 orb_position;
    public Vector4 orbCheck = new Vector4 (-1,-1,-1,-1); // order of check: bottom, left, top, right;
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

        if (index != 0 && index != 1 && index != 2 && index != 3 && index != 4 && index != 5) //bottom check
            orbCheck.x = index - 6;
        if (index != 0 && index != 6 && index != 12 && index != 18 && index != 24) //left check
            orbCheck.y = index - 1;
        if (index != 24 && index != 25 && index != 26 && index != 27 && index != 28 && index != 29) //top check
            orbCheck.z = index + 6;
        if (index != 5 && index != 11 && index != 17 && index != 23 && index != 29) //right check
            orbCheck.w = index + 1;

        //Debug.Log(string.Format("orbCheck: {0},{1},{2},{3}", orbCheck[0], orbCheck[1], orbCheck[2], orbCheck[3] ));
        
    }

    public string getOrbColor()
    {
        return color;
    }

    public void setOrbColor(string _color)
    {
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
		//Debug.Log ("Mouse Down");
        gameObject.AddComponent<Rigidbody2D>();
		gameObject.GetComponent<CircleCollider2D> ().radius = 0.01f;
        isMoving = true;
		gm.selectedOrb = gameObject;
		gm.selectedOrb_S = this;
	}

	void OnMouseUp ()
	{
		//Debug.Log ("Mouse Up");
        Destroy(gameObject.GetComponent<Rigidbody2D>());
		gameObject.GetComponent<CircleCollider2D> ().radius = 0.255f;
        isMoving = false;
        gm.selectedOrb = null;
		gm.selectedOrb_S = null;
        gm.moveOrbByIndex(this);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		//Debug.Log ("Collided!");
		if (coll.gameObject.tag == "Orb" && isMoving) 
		{
			gm.switchOrb = coll.gameObject;
			gm.switchOrb_S = coll.gameObject.GetComponent<Orb> ();
		}
	}

    //public GameObject[] findMatches()
    //{
    //    GameObject[] partnerOrbs = new GameObject[4];

    //    if (orbCheck.x != -1)
    //    {
    //        if (gm.orbs[(int)orbCheck.x].GetComponent<Orb>().color == color)
    //            partnerOrbs[0] = gm.orbs[(int)orbCheck.x];
    //        else
    //            partnerOrbs[0] = null;
    //    }

    //    if (orbCheck.y != -1)
    //    {
    //        if (gm.orbs[(int)orbCheck.y].GetComponent<Orb>().color == color)
    //            partnerOrbs[1] = gm.orbs[(int)orbCheck.y];
    //        else
    //            partnerOrbs[1] = null;
    //    }

    //    if (orbCheck.z != -1)
    //        {
    //            if (gm.orbs[(int)orbCheck.z].GetComponent<Orb>().color == color)
    //                partnerOrbs[2] = gm.orbs[(int)orbCheck.z];
    //            else
    //                partnerOrbs[2] = null;
    //        }

    //    if (orbCheck.w != -1)
    //    {
    //        if (gm.orbs[(int)orbCheck.w].GetComponent<Orb>().color == color)
    //            partnerOrbs[3] = gm.orbs[(int)orbCheck.y];
    //        else
    //            partnerOrbs[3] = null;
    //    }

    //    return partnerOrbs;
    //}
}
