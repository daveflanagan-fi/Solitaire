using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public CardSuit suit;
    public List<Card> cards;

    private void Start()
    {
        cards = new List<Card>();
    }

    public Card GetCard()
    {
        return cards.Count == 0 ? null : cards[cards.Count - 1];
    }

    public bool CanPlace(Card card)
    {
        Card topCard = GetCard();
        if ((topCard == null || suit == CardSuit.None) && card.number == CardNumber.Ace)
            return true;

        if (topCard != null && topCard.number == CardNumber.Ace && card.number == CardNumber.Two && card.suit == suit)
            return true;

        if (topCard != null && (int)topCard.number == (int)card.number - 1 && card.suit == suit)
            return true;

        return false;
    }

    public void Place(Card card)
    {
        if (card.transform.parent != null && card.transform.parent.GetComponent<Column>() != null)
            card.transform.parent.GetComponent<Column>().Remove(card);
        if (card.transform.parent != null && card.transform.parent.GetComponent<Stack>() != null)
            card.transform.parent.GetComponent<Stack>().Remove(card);
        if (card.transform.parent != null && card.transform.parent.GetComponent<Deck>() != null)
            Deck.singleton.Remove(card);
        cards.Add(card);
        card.transform.parent = transform;
        card.transform.position = transform.position + new Vector3(0, 0, -cards.Count - 1);
        suit = card.suit;
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
    }
}
