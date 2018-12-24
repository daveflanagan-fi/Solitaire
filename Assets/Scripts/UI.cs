using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image musicButton;
    public AudioSource source;
    public AudioClip clickSound;
    public Sprite musicOn;
    public Sprite musicOff;

    public void Restart()
    {
        Deck.singleton.Setup();
        source.PlayOneShot(clickSound);
    }

    public void ToggleMusic()
    {
        if (source.volume == 0)
        {
            source.volume = 1;
            musicButton.sprite = musicOn;
        }
        else
        {
            source.volume = 0;
            musicButton.sprite = musicOff;
        }
        source.PlayOneShot(clickSound);
    }
}
