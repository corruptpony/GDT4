﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainGame : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameField GameField;

    public Text PlayerTurn;

    private List<GameObject> Players;
    private int maxNumPlayers;
    private int turn;

	// Use this for initialization
	void Start ()
    {
        //Add players to the list of available players
        Players = new List<GameObject>();
        Players.Add(Player1);
        Players.Add(Player2);

        //Make a random player start the game
        maxNumPlayers = 2;
        turn = Random.Range(1, maxNumPlayers);
        getPlayerControlerCurrentTurn().giveTurn();
	}
	
	// Update is called once per frame
	void Update ()
    {
        PlayerTurn.text = "Player " + turn + " turn!";

        if (Input.GetMouseButtonDown(0))
        {
            //prevent raycast from going through buttons
            if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object

                    if (hit.transform.tag == "GameTile")
                    {
                        GameField.deselectAll();
                        hit.collider.gameObject.GetComponent<TileManager>().selectTile();
                    }
                }
            }     
        }
    }

    public void buyUnitButton(string unit)
    {
        GameObject selectedGameTile = GameField.getSelectedGameTile();
        //Debug.Log(selectedGameTile);
        if (selectedGameTile != null)
        {
            //Debug.Log(selectedGameTile.GetComponent<TileManager>().getOccupied());
            if (selectedGameTile.GetComponent<TileManager>().getOccupied() == 0)
            { 
                if (getPlayerControlerCurrentTurn().buyUnit(unit, selectedGameTile))
                {
                    nextTurn();
                }
            }
            
        }
    }

    public void endTurnButton()
    {
        nextTurn();
    }

    private void nextTurn()
    {
        GameField.deselectAll();

        getPlayerControlerCurrentTurn().endTurn();
        turn++;
        if (turn > maxNumPlayers)
        {
            turn = 1;
        }
        getPlayerControlerCurrentTurn().giveTurn();
    }

    private PlayerController getPlayerControlerCurrentTurn()
    {
        return Players[turn - 1].GetComponent<PlayerController>();
    }
}
