using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent<PlayerController> onPlayerEnter;
    public UnityEvent<PlayerController> onPlayerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if(player)
            onPlayerEnter.Invoke(player);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if(player)
            onPlayerExit.Invoke(player);
    }
}
