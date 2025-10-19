using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Blink Settings")]
    public float speed = 2f;
    public Color baseColor = Color.white;
    public GameObject target;

    private Renderer targetRenderer;
    private SpriteRenderer spriteRenderer;
    private Graphic uiGraphic;

    private Color originalColor;
    private Coroutine blinkRoutine;
    private bool hasTarget = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTarget(target);
        StartBlink();
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    // 🎯 Définit la cible
    public void SetTarget(GameObject newTarget)
    {
        StopBlink();

        if (newTarget == null)
        {
            ClearTarget();
            Debug.LogWarning("Aucune cible (null).");
            return;
        }

        targetRenderer = newTarget.GetComponent<Renderer>();
        spriteRenderer = newTarget.GetComponent<SpriteRenderer>();
        uiGraphic = newTarget.GetComponent<Graphic>();

        hasTarget = targetRenderer != null || spriteRenderer != null || uiGraphic != null;

        if (hasTarget)
        {
            if (targetRenderer != null)
                originalColor = targetRenderer.material.color;
            else if (spriteRenderer != null)
                originalColor = spriteRenderer.color;
            else
                originalColor = uiGraphic.color;
        }
        else
        {
            Debug.LogWarning($"L'objet {newTarget.name} n'a pas de Renderer, SpriteRenderer ni Graphic !");
        }
    }

    private void ClearTarget()
    {
        targetRenderer = null;
        spriteRenderer = null;
        uiGraphic = null;
        hasTarget = false;
    }

    // 🔆 Lance le clignotement
    public void StartBlink()
    {
        if (!hasTarget)
        {
            Debug.LogWarning("Impossible de clignoter : aucune cible définie.");
            return;
        }

        if (blinkRoutine == null)
            blinkRoutine = StartCoroutine(BlinkCoroutine());
    }

    // ❌ Stoppe le clignotement et restaure la couleur
    public void StopBlink()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        if (hasTarget)
        {
            if (targetRenderer != null)
                targetRenderer.material.color = originalColor;
            else if (spriteRenderer != null)
                spriteRenderer.color = originalColor;
            else if (uiGraphic != null)
                uiGraphic.color = originalColor;
        }
    }

    // ⚙️ Coroutine du blink
    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            float alpha = (Mathf.Sin(Time.time * speed) + 1) / 2f;
            Color c = baseColor;
            c.a = alpha;

            if (targetRenderer != null)
                targetRenderer.material.color = c;
            else if (spriteRenderer != null)
                spriteRenderer.color = c;
            else if (uiGraphic != null)
                uiGraphic.color = c;

            yield return null;
        }
    }
}
