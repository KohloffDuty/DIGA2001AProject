using UnityEngine;

public class PenguinReact : MonoBehaviour
{
    private Vector3 originalPos;
    private bool isJumping = false;
    private float jumpHeight = 40f;
    private float jumpSpeed = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void OnHoverEnter()
    {
        if (!isJumping)
            StartCoroutine(Jump());
    }

    public void OnHoverExit()
    {
        transform.localPosition = originalPos;
        isJumping = false;
    }

    private System.Collections.IEnumerator Jump()
    {
        isJumping = true;

        Vector3 target = originalPos + new Vector3(0, jumpHeight, 0);
        while (Vector3.Distance(transform.localPosition, target) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * jumpSpeed);
            yield return null;
        }
        {
            while (Vector3.Distance(transform.localPosition, originalPos) > 0.1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * jumpSpeed);
                yield return null;
            }

            transform.localPosition = originalPos;
            isJumping = false;
        }
    }
 
}
