using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour
{
    [SerializeField] private GameObject hub, chatApp, eventApp, chatButton, convoWindow, conversations;
    private GameObject viewport;
    private List<GameObject> convoList = new List<GameObject>();

    void Awake()
    {
        chatApp.SetActive(false);
        eventApp.SetActive(false);
        conversations.SetActive(false);
        viewport = conversations.transform.Find("Scroll View/Viewport").gameObject;
        AddConvoButton("Pekka");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleChat()
    {
        chatApp.SetActive(!chatApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void ToggleEvents()
    {
        eventApp.SetActive(!eventApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void OpenConversation(string name)
    {
    }

    public void AddConvoButton(string name)
    {
        GameObject newConvoBtn = Instantiate(chatButton, chatApp.transform.Find("Container").transform);
        newConvoBtn.GetComponent<Button>().onClick.AddListener(delegate { ButtonClick(name); });
        newConvoBtn.GetComponentInChildren<TMP_Text>().text = name;
        GameObject newConvo = Instantiate(convoWindow, viewport.transform);
        newConvo.name = "Character_" + name;
    }

    public void ButtonClick(string name)
    {

    }
}
