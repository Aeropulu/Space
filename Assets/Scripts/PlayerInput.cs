﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public RectTransform cursor;
    //public RectTransform[] cards;
    //public RectTransform[] factories;
    private int whatcard = 0;
    private RectTransform[] current = null;
    private int selectedcard = 0;
    private PlaySpots spots = null;
    private SpawnCards spawn = null;
    public SpawnCards Opponent;
    
    public InputScheme inputScheme;
   

	// Use this for initialization
	void Start () {
        cursor.gameObject.SetActive(true);
        spots = GetComponent<PlaySpots>();
        spawn = GetComponent<SpawnCards>();
        current = spots.cardspots;
        MoveCursor();
        //GameState.GameSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();

    }

    private void ProcessInput()
    {
        inputScheme.ProcessInputs();
        if (inputScheme.right)
        {
            whatcard++;
            if (whatcard >= current.Length)
                whatcard -= current.Length;
            MoveCursor();
        }
        if (inputScheme.left)
        {
            whatcard--;
            if (whatcard < 0)
                whatcard += current.Length;
            MoveCursor();
        }
        if (inputScheme.confirm)
        {
            if (current == spots.cardspots)
                SelectCard();
            else
                PlayCard();

        }
        if (inputScheme.cancel)
        {
            if (current == spots.cardspots)
                Discard();
            else
            {
                DeselectCard();
            }
        }
    }

    void Discard()
    {
        if (current[whatcard].GetComponent<CardTimer>() == null)
            return;

        RectTransform card = current[whatcard];
        CardTimer timer = card.GetComponent<CardTimer>();
        if (!timer.isAvailable)
        {

            timer.GetComponent<Animator>().Play("CardNope", -1, 0.0f);

            return;
        }
        current[whatcard] = (RectTransform)card.parent;
        card.GetComponent<Animator>().Play("CardAnim");
        Destroy(card.gameObject, 0.5f);
        spawn.SpawnCard(whatcard);
        card.SetAsLastSibling();

    }

    void MoveCursor()
    {
        if (current[whatcard].GetComponent<CardTimer>())
            cursor.SetParent(current[whatcard].parent, false);
        else
            cursor.SetParent(current[whatcard], false);
        
        cursor.SetAsFirstSibling();
    }

    void DeselectCard()
    {
        RectTransform card = spots.cardspots[selectedcard];
        CardTimer timer = card.GetComponent<CardTimer>();
        timer.GetComponent<Animator>().Play("CardPlay");
        current = spots.cardspots;
        MoveCursor();
    }

    void SelectCard()
    {
        
        CardTimer timer = current[whatcard].GetComponent<CardTimer>();
        
        if (current[whatcard].GetComponent<CardTimer>() == null)
            return;
        if (!timer.isAvailable)
        {
            
            timer.GetComponent<Animator>().Play("CardNope", -1, 0.0f);

            return;
        }
            

        timer.GetComponent<Animator>().Play("CardSelect");

        selectedcard = whatcard;
        current = spots.factoryspots;
        MoveCursor();
    }

    void PlayCard()
    {
        

        RectTransform card = spots.cardspots[selectedcard];
        CardTimer timer = card.GetComponent<CardTimer>();
        
        if (current[whatcard].GetComponent<CardTimer>() != null)
        {
            RectTransform oldcard = current[whatcard];
            current[whatcard] = (RectTransform)oldcard.parent;
            MoveCursor();
            Destroy(oldcard.gameObject);
        }

        Opponent.AddCardToDeck(timer.type);

        spots.cardspots[selectedcard] = (RectTransform) card.parent;
        card.SetParent(current[whatcard], false);
        current[whatcard] = card;
        timer.Activate();
        timer.GetComponent<Animator>().Play("CardPlay");
        /*timer.isActive = true;
        timer.isAvailable = false;
        timer.duration = timer.type.duration;*/
        spawn.SpawnCard(selectedcard);
        current = spots.cardspots;
        MoveCursor();

        GameState.GameSpeed += 0.05f;
    }
}
