using System.Collections.Generic;

using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public Dictionary<CharacterController2D, HashSet<Collider2D>> targets = new Dictionary<CharacterController2D, HashSet<Collider2D>>();
    public LayerMask layers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<CharacterController2D>();
        if(controller && layers.MatchLayer(collision.gameObject.layer))
        {
            if(!targets.ContainsKey(controller))
                targets.Add(controller, new HashSet<Collider2D>());
            targets[controller].Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<CharacterController2D>();
        if(controller && layers.MatchLayer(collision.gameObject.layer))
        {
            if(!targets.ContainsKey(controller))
                return;

            targets[controller].Remove(collision);
            if(targets[controller].Count == 0)
                targets.Remove(controller);
        }
    }
}
