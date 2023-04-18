using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake(){
        cardList.Add(new Card(Name: "Stannis Baratheon", CardType: "character", Code: "354",
            Faction: "baratheon", Image: Resources.Load<Sprite>("stannis_baratheon")));

        cardList.Add(new Card(Name: "Robert Baratheon", CardType: "character", Code: "362",
            Faction: "baratheon", Image: Resources.Load<Sprite>("robert_baratheon")));

        cardList.Add(new Card(Name: "Ser Davos Seaworth", CardType: "character", Code: "927",
            Faction: "baratheon", Image: Resources.Load<Sprite>("ser_davos_seaworth")));

        cardList.Add(new Card(Name: "Melisandre", CardType: "character", Code: "253",
            Faction: "baratheon", Image: Resources.Load<Sprite>("red_woman")));

    }



}
