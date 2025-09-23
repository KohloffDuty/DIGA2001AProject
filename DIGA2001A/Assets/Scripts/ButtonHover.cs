using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    private Vector3 originalScale;
    private bool isHovered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale; 
    }

    void Update()
    {
        Vector3 targetScale = isHovered ? hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 8f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPostRender(PointerEventData eventData)
    {
        isHovered = false;
    }

    // Update is called once per frame
    
        
    }

