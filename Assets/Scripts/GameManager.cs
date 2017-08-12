using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	const int BOARD_SIZE_X = 5;
	const int BOARD_SIZE_Y = 6;
	const int BOARD_WRAP = 6;
	const float OFFSET 	   = 0.51f;

	public List<GameObject> orbs;
	GameObject orb_red, orb_blue, orb_green, orb_yellow;

	public Vector2 mousePosition;
	public GameObject selectedOrb;
	public Orb selectedOrb_S;
	public GameObject switchOrb;
	public Orb switchOrb_S;

	// Use this for initialization
	void Start ()
    {
		orbs = new List<GameObject> ();
		orb_red = (GameObject) Resources.Load ("Prefabs/Orb_Red");
		orb_blue = (GameObject) Resources.Load ("Prefabs/Orb_Blue");
		orb_green = (GameObject) Resources.Load ("Prefabs/Orb_Green");
		orb_yellow = (GameObject) Resources.Load ("Prefabs/Orb_Yellow");

		generateBoard ();
		displayBoard ();
	}
	
	// Update is called once per frame
	void Update ()
    {
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(selectedOrb != null)
		{
			selectedOrb_S.setPosition (mousePosition);
		}

        if(switchOrb != null)
        {
            switchOrbs(selectedOrb_S, switchOrb_S);
        }
	}

	GameObject getRandomOrb(int index)
	{
		int rand = Random.Range (1, 5);
		GameObject tmp;

		switch (rand) 
		{
		case 1:
				tmp = Instantiate (orb_red);
				tmp.GetComponent<Orb> ().setOrb(index, "red");
				return tmp;
			case 2: 
				tmp = Instantiate (orb_blue);
				tmp.GetComponent<Orb> ().setOrb(index, "blue");
				return tmp;
			case 3: 
				tmp = Instantiate (orb_green);
				tmp.GetComponent<Orb> ().setOrb(index, "green");
				return tmp;
			case 4: 
				tmp = Instantiate (orb_yellow);
				tmp.GetComponent<Orb> ().setOrb(index, "yellow");
				return tmp;
		}

		Debug.Log ("There was an issue returning an orb.");
		return null;
	}

	void generateBoard ()
	{
		for (int i = 0; i < (BOARD_SIZE_X * BOARD_SIZE_Y); i++) 
		{
			orbs.Add (getRandomOrb(i));
		}
	}

	int getY (int index)
	{
		int tmp = 0;

		while (index > BOARD_WRAP) 
		{
			index = index - BOARD_WRAP;
			tmp++;
		}

		return tmp;
	}

    public void moveOrbByIndex(Orb orbToMove)
    {
        int x, y;

        x = y = orbToMove.getOrbIndex() + 1;
        //get x
        while (x > BOARD_WRAP)
        {
            x = x - 6;
        }

        //get y
        y = getY(y);

        //Debug.Log (string.Format("displayBoard: Index: {2} X: {0} Y: {1}", x, y, orb.GetComponent<Orb>().index));

        orbToMove.setPosition(new Vector2(x * OFFSET, y * OFFSET));
    }

	void displayBoard ()
	{
		//Debug.Log ("displayBoard: Started.");
		foreach (GameObject orb in orbs) 
		{
			Orb orbToMove = orb.GetComponent<Orb> ();
			int x, y;

			x = y = orbToMove.getOrbIndex() + 1;
			//get x
			while (x > BOARD_WRAP) 
			{
				x = x - 6;
			}

			//get y
			y = getY(y);

			//Debug.Log (string.Format("displayBoard: Index: {2} X: {0} Y: {1}", x, y, orb.GetComponent<Orb>().index));

			orbToMove.setPosition (new Vector2(x*OFFSET,y*OFFSET));
		}
	}

	void switchOrbs (Orb orb1, Orb orb2) //orb1 - moving orb | orb2 - stationary orb
	{
        int temp_index = orb2.getOrbIndex();
    
		orb2.setOrbIndex (orb1.getOrbIndex ());
        moveOrbByIndex(orb2);

        orb1.setOrbIndex(temp_index);

        switchOrb = null;
        switchOrb_S = null;
	}
}
