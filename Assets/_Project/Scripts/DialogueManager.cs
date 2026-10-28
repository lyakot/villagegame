using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private PixelMovement playerMovement;
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button closeButton;

    private string[] lines;
    private int currentLine;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsDialogueActive)
            NextLine();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseDialogue();
    }

    public void StartDialogue(string[] dialogueLines)
    {
        playerMovement.isLocked = true;
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        lines = dialogueLines;
        currentLine = 0;

        IsDialogueActive = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = lines[currentLine];
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= lines.Length)
        {
            CloseDialogue();
            return;
        }

        dialogueText.text = lines[currentLine];
    }

    public void CloseDialogue()
    {
        IsDialogueActive = false;

        dialoguePanel.SetActive(false);

        lines = null;
        currentLine = 0;
        playerMovement.isLocked = false;
    }
}