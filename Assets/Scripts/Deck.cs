using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit
{
    Clubs,      // 0
    Diamonds,   // 1
    Spades,     // 2
    Hearts,     // 3
    None
}

public enum CardNumber
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Card> allCards = new List<Card>();
    public List<Card> cards = new List<Card>();
    public List<Card> cardsInPlay = new List<Card>();
    public Column[] columns;
    public Stack[] stacks;

    public Sprite[] faces;
    public Sprite back;

    public AudioSource audioSource;
    public AudioClip drawSound;

    System.Random random = new System.Random();

    public static Deck singleton;

    Deck()
    {
        singleton = this;
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        foreach (Card card in allCards)
            Destroy(card.gameObject);
        allCards.Clear();
        cards.Clear();
        cardsInPlay.Clear();

        foreach (Stack stack in stacks)
            stack.cards.Clear();

        foreach (Column column in columns)
            column.cards.Clear();

        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            if (suit == CardSuit.None)
                continue;

            foreach (CardNumber number in Enum.GetValues(typeof(CardNumber)))
            {
                GameObject obj = Instantiate(cardPrefab, transform);
                Card card = obj.GetComponent<Card>();
                card.SetCard(suit, number);
                card.transform.position = transform.position + new Vector3(0, 0, -1);
                cards.Add(card);
                allCards.Add(card);
            }
        }

        for (int i = 0; i < columns.Length; i++)
        {
            Column column = columns[i];
            for (int j = 0; j < i + 1; j++)
            {
                int index = random.Next(0, cards.Count - 1);
                Card card = cards[index];
                cards.RemoveAt(index);

                card.transform.parent = column.transform;
                card.transform.position = column.transform.position + new Vector3(0, j * -0.2f, -j - 1);

                if (j == i)
                    card.Toggle(true);

                column.cards.Add(card);
            }
        }

        Draw();
    }

    public void Draw()
    {
        if (cards.Count == 0)
            return;

        audioSource.PlayOneShot(drawSound);
        Card cardToPlay = cards[random.Next(0, cards.Count - 1)];
        cards.Remove(cardToPlay);
        cardToPlay.Toggle(true);
        cardToPlay.transform.position = transform.position + new Vector3(1.2f, 0, -cardsInPlay.Count - 1);
        cardsInPlay.Add(cardToPlay);
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
        cardsInPlay.Remove(card);
    }

    private void OnMouseDown()
    {
        cards.AddRange(cardsInPlay);
        cardsInPlay.Clear();
        foreach (Card card in cards)
        {
            card.transform.position = transform.position + new Vector3(0, 0, -1);
            card.Toggle(false);
        }
        Draw();
    }
}
