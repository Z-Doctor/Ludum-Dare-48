using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerUINav : MonoBehaviour
{
    [HideInInspector] [SerializeField] EventSystem eventSystem;
    [SerializeField] int selectedIndex;
    [SerializeField] Button[] buttons;

    Button pressedButton;

    public int SelectedIndex { get => eventSystem.currentSelectedGameObject ? selectedIndex : 0; set => SetSelected(value); }

    public void Clean()
    {
        buttons = buttons.Where(b => b).ToArray();
    }

    public void RemoveButton(GameObject gameObject)
    {
        buttons = buttons.Where(b => b && b.gameObject != gameObject).ToArray();
    }

    void SetSelected(int value)
    {
        selectedIndex = value;
        var button = GetAt(selectedIndex);
        if(button)
            eventSystem.SetSelectedGameObject(button.gameObject);
        else
            eventSystem.SetSelectedGameObject(null);
    }

    public Button GetAt(int index)
    {
        if(index == 0 || !index.InBetween(1, buttons.Length))
            return null;
        return buttons[index - 1];
    }

    public void Select(CallbackContext context)
    {
        if(!gameObject.activeSelf)
            return;

        var button = GetAt(SelectedIndex);

        if(!button)
            return;

        if(context.phase == InputActionPhase.Started)
        {
            Deselect();
            button.OnPointerDown(new PointerEventData(eventSystem));
            pressedButton = button;
        }
        else if(context.performed && pressedButton == button)
        {
            button.OnPointerUp(new PointerEventData(eventSystem));
            button.onClick?.Invoke();
        }
        else
            Deselect();
    }

    public void Deselect()
    {
        if(pressedButton)
        {
            pressedButton.OnPointerUp(new PointerEventData(eventSystem));
            pressedButton = null;
        }
    }

    public void NavUp(CallbackContext context)
    {
        if(!context.performed || !gameObject.activeSelf)
            return;

        Deselect();

        if(SelectedIndex == 0)
            SelectedIndex = 1;
        else
        {
            selectedIndex--;
            if(selectedIndex <= 0)
                SelectedIndex = buttons.Length;
            else
                SelectedIndex = selectedIndex;
        }
    }

    public void NavDown(CallbackContext context)
    {
        if(!context.performed || !gameObject.activeSelf)
            return;

        Deselect();

        if(SelectedIndex == 0)
            SelectedIndex = 1;
        else
        {
            selectedIndex++;
            if(selectedIndex > buttons.Length)
                SelectedIndex = 1;
            else
                SelectedIndex = selectedIndex;
        }
    }

    private void OnValidate()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }
}
