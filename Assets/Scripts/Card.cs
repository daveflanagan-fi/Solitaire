using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardSuit suit;
    public CardNumber number;
    public bool visible;
    private int _index;
    private List<Stack> _stacks = new List<Stack>();
    private List<Column> _columns = new List<Column>();

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _index = ((int)suit * 13) + (int)number;
        _renderer.sprite = visible ? Deck.singleton.faces[_index] : Deck.singleton.back;
    }

    public void Toggle(bool show)
    {
        visible = show;
        _renderer.sprite = visible ? Deck.singleton.faces[_index] : Deck.singleton.back;
    }

    public void SetCard(CardSuit suit, CardNumber number)
    {
        Start();

        this.suit = suit;
        this.number = number;

        _index = ((int)suit * 13) + (int)number;
        _renderer.sprite = visible ? Deck.singleton.faces[_index] : Deck.singleton.back;
    }

    public bool Pickup()
    {
        if (!visible)
            return false;
        return true;
    }

    private void OnMouseDown()
    {
        if (Pickup())
        {
            if (GetComponentInParent<Column>() != null)
                Player.singleton.Pickup(GetComponentInParent<Column>().GetCards(this));
            else
                Player.singleton.Pickup(new Card[] { this });
        }
        else if (transform.parent.GetComponent<Deck>() != null)
            Deck.singleton.Draw();
    }

    private void OnMouseUp()
    {
        Player.singleton.Drop(this, _columns, _stacks);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _stacks.Add(collision.GetComponent<Stack>());
        _columns.Add(collision.GetComponent<Column>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _stacks.Remove(collision.GetComponent<Stack>());
        _columns.Remove(collision.GetComponent<Column>());
    }
}
