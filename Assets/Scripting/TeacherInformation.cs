using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TeacherInformation : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text text;

    private const string BaseUrl = "https://www.htl-salzburg.ac.at";
    private const string ListUrl = BaseUrl + "/lehrerinnen.html";

    private HttpClient client;

    void Awake()
    {
        var issuer = Resources.Load<TextAsset>("Certificates/GlobalSignAlphaSSL2025");
        if (issuer == null) throw new InvalidOperationException("Das SSL-Zwischenzertifikat fehlt im Projekt.");
        var root = Resources.Load<TextAsset>("Certificates/GlobalSignRootR46");
        if (root == null) throw new InvalidOperationException("Das SSL-Stammzertifikat fehlt im Projekt.");
        client = SchoolHttp.CreateClient(issuer.bytes, root.bytes);
    }

    private string currentRoom = "";

    void Start()
    {
        panel.SetActive(false);
    }

    async void OnTriggerEnter2D(Collider2D collision)
    {
        string room = collision.gameObject.name;
        currentRoom = room;

        panel.SetActive(true);
        text.text = "Lade Lehrer...";

        try
        {
            string listHtml = await client.GetStringAsync(ListUrl);

            List<string> links =
                TeacherParser.ParseTeacherLinks(listHtml, room);

            string result = "LEHRERZIMMER: " + room + "\n\n";

            foreach (string link in links)
            {
                try
                {
                    Uri url = new Uri(new Uri(BaseUrl), link);

                    string html = await client.GetStringAsync(url);

                    TeacherData teacher =
                        TeacherParser.ParseTeacherDetails(html);

                    if (NormalizeRoom(teacher.roomNumber) !=
                        NormalizeRoom(room))
                        continue;

                    result += teacher.name + "\n";
                    result += "Tel-DW: " + teacher.phone + "\n";
                    result += "Sprechstunde: " +
                              teacher.consultationHours + "\n";
                    result += "E-Mail: " + teacher.email + "\n\n";
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            }

            if (currentRoom == room)
                text.text = result;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());

            if (currentRoom == room)
                text.text = "Fehler beim Laden der Lehrer.";
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (currentRoom != collision.gameObject.name)
            return;

        currentRoom = "";
        panel.SetActive(false);
        text.text = "";
    }

    static string NormalizeRoom(string room)
    {
        return Regex.Replace(
            room.ToUpperInvariant(), @"[\s_-]", "");
    }

    void OnDestroy()
    {
        client.Dispose();
    }
}