using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public List<Card> cards;

    Column()
    {
        cards = new List<Card>();
    }

    public Card GetCard()
    {
        return cards.Count == 0 ? null : cards[cards.Count - 1];
    }

    public Card[] GetCards(Card card)
    {
        List<Card> result = new List<Card>();
        bool found = false;
        foreach (Card check in cards)
        {
            if (check == card)
                found = true;
            if (found)
                result.Add(check);
        }
        return result.ToArray();
    }

    public bool CanPlace(Card card)
    {
        Card topCard = GetCard();
        if (topCard == null)
            return card.number == CardNumber.King;

        if ((int)topCard.number != (int)card.number + 1)
            return false;

        if ((topCard.suit == CardSuit.Clubs || topCard.suit == CardSuit.Spades)
            && (card.suit == CardSuit.Clubs || card.suit == CardSuit.Spades))
            return false;

        if ((topCard.suit == CardSuit.Hearts || topCard.suit == CardSuit.Diamonds)
            && (card.suit == CardSuit.Hearts || card.suit == CardSuit.Diamonds))
            return false;

        return true;
    }

    public void Place(Card card)
    {
        if (card.transform.parent != null && card.transform.parent.GetComponent<Column>() != null)
            card.transform.parent.GetComponent<Column>().Remove(card);
        if (card.transform.parent != null && card.transform.parent.GetComponent<Stack>() != null)
            card.transform.parent.GetComponent<Stack>().Remove(card);
        if (card.transform.parent != null && card.transform.parent.GetComponent<Deck>() != null)
            Deck.singleton.Remove(card);
        card.transform.parent = transform;
        card.transform.position = transform.position + new Vector3(0, cards.Count * -0.2f, -cards.Count - 1);
        cards.Add(card);
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
        if (GetCard() != null)
            GetCard().Toggle(true);
    }
}
