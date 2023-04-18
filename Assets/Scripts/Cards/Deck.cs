using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck{
    private string faction;
    private Card agenda;
    private List<Card> plots;
    private List<Card> usedPlots;
    private List<Card> draw;
    private List<Card> discard;
    private List<Card> dead;

    public Deck(string Faction, List<Card> Plots, List<Card> Draw, Card Agenda=null){
        faction = Faction;
        plots = Plots;
        draw = Draw;
        agenda = Agenda;
        discard = new List<Card>();
        dead = new List<Card>();
        usedPlots = new List<Card>();
    }

    public string Faction(){return faction;}

    public Card Agenda(){return agenda;}

    public List<Card> Plots(){return plots;}

    public List<Card> UsedPlots(){return usedPlots;}

    public List<Card> DrawPile(){return draw;}

    public List<Card> DiscardPile(){return discard;}

    public List<Card> DeadPile(){return dead;}

    public void shuffle(){
        Card temp;
        int pivot;
        for(int i = draw.Count - 1; i > 0; i--){
            pivot = Random.Range(0, i);
            temp = draw[i];
            draw[i] = draw[pivot];
            draw[pivot] = temp;
        }
    }
}
