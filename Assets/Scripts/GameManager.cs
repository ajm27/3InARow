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

    public List<GameObject[]> matches_red_h;
    public List<GameObject[]> matches_blue_h;
    public List<GameObject[]> matches_green_h;
    public List<GameObject[]> matches_yellow_h;

    public List<GameObject[]> matches_red_v;
    public List<GameObject[]> matches_blue_v;
    public List<GameObject[]> matches_green_v;
    public List<GameObject[]> matches_yellow_v;

    private int[,] rows = { {0, 1, 2, 3, 4, 5}, { 6, 7, 8, 9, 10, 11 }, { 12, 13, 14, 15 ,16, 17 }, { 18, 19, 20, 21, 22, 23 }, { 24, 25, 26, 27, 28, 29 } };
    private int[,] cols = { { 0, 6, 12, 18, 24 }, { 1, 7, 13, 19, 25 }, { 2, 8, 14, 20, 26 }, { 3, 9, 15, 21, 27 }, { 4, 10, 16, 22, 28 }, { 5, 11, 17, 23, 29 } };

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

        matches_red_h = new List<GameObject[]>();
        matches_blue_h = new List<GameObject[]>();
        matches_green_h = new List<GameObject[]>();
        matches_yellow_h = new List<GameObject[]>();

        matches_red_v = new List<GameObject[]>();
        matches_blue_v = new List<GameObject[]>();
        matches_green_v = new List<GameObject[]>();
        matches_yellow_v = new List<GameObject[]>();
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

        if(Input.GetMouseButtonUp(0))
        {
            checkForMatches();
            Debug.Log("Red Horizontal matches: " + matches_red_h.Count);
            Debug.Log("Blue Horizontal matches: " + matches_blue_h.Count);
            Debug.Log("Green Horizontal matches: " + matches_green_h.Count);
            Debug.Log("Yellow Horizontal matches: " + matches_yellow_h.Count);

            Debug.Log("Red Vertical matches: " + matches_red_h.Count);
            Debug.Log("Blue Vertical matches: " + matches_blue_h.Count);
            Debug.Log("Green Vertical matches: " + matches_green_h.Count);
            Debug.Log("Yellow Vertical matches: " + matches_yellow_h.Count);

            matches_red_h = new List<GameObject[]>();
            matches_blue_h = new List<GameObject[]>();
            matches_green_h = new List<GameObject[]>();
            matches_yellow_h = new List<GameObject[]>();

            matches_red_v = new List<GameObject[]>();
            matches_blue_v = new List<GameObject[]>();
            matches_green_v = new List<GameObject[]>();
            matches_yellow_v = new List<GameObject[]>();
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
            moveOrbByIndex(orb.GetComponent<Orb> ());
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

    bool checkIfEnd_Hor (int index)
    {
        if (index == 5 || index == 11 || index == 17 || index == 23 || index == 29)
            return true;

        return false;
    }

    bool checkIfEnd_Vert(int index)
    {
        if ((index + 6) > 29)
            return true;

        return false;
    }

    Orb findOrbByIndex (int index)
    {
        foreach(GameObject orb in orbs)
        {
            if (orb.GetComponent<Orb>().getOrbIndex() == index)
                return orb.GetComponent<Orb>();
        }

        return null;
    }

    public void checkForMatches ()
    {
        Debug.Log("Checking for Matches.");
        foreach(GameObject orb in orbs)
        {
            Orb orb_S = orb.GetComponent<Orb>();
            Orb nextOrb = null;

            GameObject[] tmp = new GameObject[2];

            //horizontal check
            if(!checkIfEnd_Hor(orb_S.getOrbIndex()))
            {
                nextOrb = findOrbByIndex(orb_S.getOrbIndex() + 1);
            }
            if (nextOrb != null)
            {
                if (orb_S.getOrbColor().Equals(nextOrb.getOrbColor()))
                {
                    tmp[0] = orb_S.gameObject;
                    tmp[1] = nextOrb.gameObject;

                    if (orb_S.getOrbColor().Equals("red"))
                    {
                        matches_red_h.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("blue"))
                    {
                        matches_blue_h.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("green"))
                    {
                        matches_green_h.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("yellow"))
                    {
                        matches_yellow_h.Add(tmp);
                    }
                }
            }
            //vertical check
            if (!checkIfEnd_Vert(orb_S.getOrbIndex()))
            {
                nextOrb = findOrbByIndex(orb_S.getOrbIndex() + 1);
            }
            if (nextOrb != null)
            {
                if (orb_S.getOrbColor().Equals(nextOrb.getOrbColor()))
                {
                    tmp[0] = orb_S.gameObject;
                    tmp[1] = nextOrb.gameObject;

                    if (orb_S.getOrbColor().Equals("red"))
                    {
                        matches_red_v.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("blue"))
                    {
                        matches_blue_v.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("green"))
                    {
                        matches_green_v.Add(tmp);
                    }
                    if (orb_S.getOrbColor().Equals("yellow"))
                    {
                        matches_yellow_v.Add(tmp);
                    }
                }
            }
        }
    }
}