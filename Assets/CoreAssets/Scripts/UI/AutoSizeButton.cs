using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AutoSizeButton : MonoBehaviour
{
    [SerializeField] private float padding = 15f;

    private Button button;
    private RectTransform buttonRect;
    private TextMeshProUGUI buttonText;

    private void Awake( )
    {
        button = GetComponent<Button>( );
        buttonRect = GetComponent<RectTransform>( );
        buttonText = GetComponentInChildren<TextMeshProUGUI>( );
        ResizeButton( );
    }

    public void ResizeButton( )
    {
        if ( buttonText == null )
            return;

        float preferredWidth = LayoutUtility.GetPreferredWidth( buttonText.rectTransform );

        preferredWidth += 2 * padding; // Add padding on both sides

        buttonRect.sizeDelta = new Vector2( preferredWidth, buttonRect.sizeDelta.y );
    }

    public void AddListener(UnityAction callback)
    {
        button.onClick.AddListener( callback );
    }
    public void RemoveListener( UnityAction callback )
    {
        button.onClick.RemoveListener( callback );
    }
}
