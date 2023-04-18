using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card{
    public string name;
    public string cardType;
    public int cost;
#nullable enable
    public List<string>? traits;
#nullable disable
    public string faction;
    public Sprite image;
    public string code;
    public string text;
    public int deckLimit;
    public bool abilityImplimented;

    public bool revealed;

    // character card specific
    public bool? hasMilitary;
    public bool? hasIntrigue;
    public bool? hasPower;
    public bool? isLoyal;
    public int? strength;

    // plot card specific
    public int? initiative;
    public int? claim;
    public int? reserve;
    public int? income;


    public Card(string Name,
                string CardType,
                string Faction,
                string Code,
                int Cost=0,
                List<string> Traits=null,
                Sprite Image=null,
                string Text="",
                int DeckLimit=3,
                bool AbilityImplimented=false,
                bool Military=false,
                bool? Intrigue=false,
                bool? Power=false,
                bool? Loyal=false,
                int? Strength=null,
                int? Income=null,
                int? Initiative=null,
                int? Claim=null,
                int? Reserve=null){

            // required
            name = Name;
            cardType = CardType;
            faction = Faction;
            code = Code;

            // not required
            cost = Cost;
            traits = Traits ?? new List<string>();
            image = Image;
            text = Text;
            deckLimit = DeckLimit;
            abilityImplimented = AbilityImplimented;

            // character
            hasMilitary = Military;
            hasIntrigue = Intrigue;
            hasPower = Power;
            isLoyal = Loyal;
            strength = Strength;

            // plot
            income = Income;
            initiative = Initiative;
            claim = Claim;
            reserve = Reserve;

            revealed = true;
        }
}
