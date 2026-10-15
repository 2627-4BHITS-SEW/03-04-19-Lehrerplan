| Nr. | Edge Case | Beispiel | Erwartetes Verhalten / Problem |
|---|---|---|---|
| 1 | Leere HTML-Seite | `html = ""` | Der Parser soll eine leere Liste zurückgeben bzw. bei Lehrerdetails `"-"` verwenden, ohne abzustürzen. |
| 2 | Ungültige Raumnummer | `roomNumber = ""` | `Substring(0, 1)` verursacht eine `ArgumentOutOfRangeException`. |
| 3 | Unterschiedliche Raumschreibweisen | `C 205`, `C-205`, `c205`, `C_205` | Alle Varianten sollen als derselbe Raum erkannt werden. |
| 4 | Fehlende Lehrerinformationen | Keine E-Mail oder Sprechstunde vorhanden | Fehlende Felder sollen `"-"` zurückgeben. Wenn direkt das nächste Label folgt, darf dieses nicht als Wert übernommen werden. |
| 5 | Doppelte Lehrer-Links | Derselbe Lehrer kommt dreimal im HTML vor | Durch `HashSet<string>` soll der Link nur einmal zurückgegeben werden. |
