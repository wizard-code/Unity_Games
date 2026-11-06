using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dia;
    public string[] lines;
    public float textSpeed;
    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dia.text = string.Empty;
        startdialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(dia.text == lines[index])
            {
                Nextline();
            }
            else
            {
                StopAllCoroutines();
                dia.text = lines[index];
            }
        }
    }
    void startdialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach( char c in lines[index].ToCharArray())
        {
            dia.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void Nextline()
    {
        if(index < lines.Length - 1)
        {
            index++;
            dia.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
