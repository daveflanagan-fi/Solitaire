using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> selectedCards = new List<Card>();
    public Vector3 offset;
    public Vector3 startLocation;
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip dropSound;

    public static Player singleton;

    Player()
    {
        singleton = this;
    }

    private void Update()
    {
        if (selectedCards.Count > 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            pos.z = -50;
            for (int i = 0; i < selectedCards.Count; i++)
                selectedCards[i].transform.position = pos - new Vector3(0, i * 0.2f, i);
        }
    }

    public void Pickup(Card[] cards)
    {
        audioSource.PlayOneShot(pickupSound);
        selectedCards.AddRange(cards);
        offset = selectedCards[0].transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startLocation = selectedCards[0].transform.position;
    }

    public void Drop(Card card)
    {
        if (selectedCards.Count == 0 || selectedCards[0] != card)
            return;
        audioSource.PlayOneShot(dropSound);
        for (int i = 0; i < selectedCards.Count; i++)
            selectedCards[i].transform.position = startLocation - new Vector3(0, i * 0.2f, i);
        selectedCards.Clear();
    }

    public void Drop(Card card, List<Column> columns, List<Stack> stacks)
    {
        if (selectedCards.Count == 0 || selectedCards[0] != card)
            return;
        audioSource.PlayOneShot(dropSound);
        foreach (Stack stack in stacks)
        {
            if (selectedCards.Count > 1)
                break;
            if (stack == null)
                continue;
            if (stack.CanPlace(card))
            {
                foreach (Card selected in selectedCards)
                    stack.Place(selected);
                selectedCards.Clear();
                return;
            }
        }
        foreach (Column column in columns)
        {
            if (column == null)
                continue;
            if (column.CanPlace(card))
            {
                foreach (Card selected in selectedCards)
                    column.Place(selected);
                selectedCards.Clear();
                return;
            }
        }
        Drop(card);
    }
}
