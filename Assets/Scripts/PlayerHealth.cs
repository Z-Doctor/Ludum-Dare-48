using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] [SerializeField] PlayerController player;

    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;

    int maxHealth;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Redraw Health")]
    public void RedrawHealth()
    {
        ClearChildren();
        for(int i = 1; i <= player.MaxHealth; i++)
        {
            Sprite sprite;
            if(player.Health == 0 || i > Mathf.CeilToInt(player.Health))
                sprite = emptyHeart;
            else if(player.Health < i)
                sprite = halfHeart;
            else
                sprite = fullHeart;

            var obj = new GameObject()
            {
                name = sprite.name + "_" + i,
            };
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;

            var rect = obj.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = Vector2.up;
            obj.AddComponent<Image>().sprite = sprite;
        }
    }

    void ClearChildren()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            if(Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);

        }
    }

    void OnValidate()
    {
        player = FindObjectOfType<PlayerController>();
        if(!player)
        {
            Debug.LogWarning("Can't find player");
            return;
        }

        StartCoroutine(Redraw());

        IEnumerator Redraw()
        {
            yield return null;
            RedrawHealth();
        }
    }
}
