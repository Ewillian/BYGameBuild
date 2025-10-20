using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Elements")]
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private List<GameObject> _gameObjectList;

    [Header("Blink Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Color baseColor = Color.white;

    private int _step;
    private List<string> _tutorialMessages;
    private Renderer targetRenderer;
    private SpriteRenderer spriteRenderer;
    private Graphic uiGraphic;

    private Color originalColor;
    private Coroutine blinkRoutine;
    private bool hasTarget = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _previousButton.onClick.AddListener(delegate ()
        {
            ApplyScript(false);
        });

        _nextButton.onClick.AddListener(delegate ()
        {
            ApplyScript(true);
        });

        ApplyScript(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        _tutorialMessages = new List<string>
        {
            "L'objectif du jeu est de récupérer le balle de Grogu",
            "Pour cela, il faut amener la jauge à l'indicateur", "Attention ! Si mando vous voit, il resserrera la boule !", "Vous devez y arriver avant la fin du temps imparti.", "Que la force soit avec vous !"
        };
    }

    private void ApplyScript(bool isNext)
    {
        if (_tutorialText == null) return;

        if (isNext)
        {
            if (_step < _tutorialMessages.Count - 1)
            {
                _step++;
            }
        }
        else
        {
            if (_step > 0)
            {
                _step--;
            }
        }

        switch (_step)
        {
            case 0:
                SetTarget(_gameObjectList.ElementAt(0));
                break;
            case 1:
                SetTarget(_gameObjectList.ElementAt(1));
                break;
            case 2:
                SetTarget(_gameObjectList.ElementAt(2));
                break;
        }

        if (hasTarget) StartBlink();

        _tutorialText.SetText(_tutorialMessages.ElementAt(_step));
    }

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

    public void StartBlink()
    {
        if (!hasTarget)
        {
            Debug.LogWarning("Impossible de clignoter : aucune cible définie.");
            return;
        }
        Debug.Log($"notnull :{blinkRoutine != null}");
        if (blinkRoutine == null)
            blinkRoutine = StartCoroutine(BlinkCoroutine());
    }

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
