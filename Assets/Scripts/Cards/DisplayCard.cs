using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();
    public int displayId;
    public Image artImage;
    Deck deck;


    void Start(){
        deck = new Deck(Faction: "baratheon", Plots: CardDatabase.cardList,
            Draw: CardDatabase.cardList);
        deck.shuffle();
        displayCard.Add(deck.DrawPile()[displayId]);
        artImage.sprite = displayCard[0].image;
    }

    void Update(){
        if(displayCard[0].revealed)
            artImage.sprite = displayCard[0].image;
        else
        {
            artImage.sprite = Resources.Load<Sprite>("card_back");
        }
    }

    public void Shuffle(){
        displayCard[0].revealed = !displayCard[0].revealed;
        deck.shuffle();
    }
}
