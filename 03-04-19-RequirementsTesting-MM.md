# Requirements & Testing: 2D-Schulplan Unity (3. Stock)

## 1. Funktionale Anforderungen (Functional Requirements)

* **FR-01**: Das System muss den Grundriss des 3. Stockwerks (Gang B und C) mit allen Räumen, wo Lehrer enthalten sind und Fluren in einer 2D-Ansicht darstellen.
* **FR-02**: Das System muss an jeder Raumtür ein sichtbares Türschild mit der jeweiligen Raumnummer anzeigen.
* **FR-03**: Das System muss auf jedem Türschild die Infos der HTL-Lehrer-Homepage (Name der Lehrperson und Raumbezeichnung) anzeigen.
* **FR-04**: Das System muss es dem Benutzer ermöglichen, durch den Flur zu navigieren.
* **FR-05**: Das System muss die Daten der HTL-Lehrer anzeigen, wenn der Benutzer in dessen Raum reingeht

---

## 2. Nichtfunktionale Anforderungen (Non-Functional Requirements)

* **NFR-01 (Lesbarkeit)**: Die Schrift auf den Türschildern muss aus der Standard-Kameraperspektive klar lesbar sein, ohne dass Texte über die Schildgrenzen hinausragen.
* **NFR-02 (Performance)**: Das 2D-Karten-Layout muss flüssig mit mindestens 60 FPS geladen und gerendert werden.
* **NFR-03 (Fehlertoleranz)**: Das System darf nicht abstürzen, wenn für einen Raum keine Lehrer-Informationen vorhanden sind.

---

## 3. Akzeptanzkriterien

* **Akzeptanzkriterium zu FR-01**:
  * Alle Räume aus Gang B (B303–B305, B310–B314) und Gang C (C303–C304, C312–C314) sind an ihren korrekten Positionen im 2D-Layout sichtbar.
* **Akzeptanzkriterium zu FR-03**:
  * Jedes Türschild zeigt mindestens die Raumnummer sowie den aktuell zugewiesenen Lehrernamen gemäß der HTL-Salzburg-Webseite an.
* **Akzeptanzkriterium zu NFR-02**:
  * Die 2D-Übersichtskarte mit allen Räumen und Türschildern lädt nach dem Start der Anwendung ohne spürbare Verzögerung und läuft durchgehend flüssig mit durchschnittlich mindestens 60 FPS
---

## 4. Testfälle

| Test-ID | Typ | Beschreibung | Erwartetes Ergebnis | Zugehörige Requirement-ID |
| :--- | :--- | :--- | :--- | :--- |
| **TC-01** | Happy Path | Anwendung starten und den Grundriss von Gang B und C betrachten. | Alle Räume, Flure und Türschilder werden korrekt gerendert und die Beschriftungen sind vollständig lesbar. | `FR-01`, `FR-02`, `NFR-01` |
| **TC-02** | Edge Case | Gleichzeitiges Laden aller Räume: Das Anwendungsfenster wird so aufgespannt, dass sämtliche Räume von Gang B und C gleichzeitig im Bild sichtbar sind und gerendert werden müssen. | Die Framerate bleibt weiterhin stabil bei mindestens 60 FPS, ohne dass die Darstellung der Türschilder ins Stocken gerät. | `NFR-02` |
| **TC-03** | Edge Case | Fehlende Schriftart-Zeichen: Der Name einer Lehrperson enthält Sonderzeichen oder Akzente (z. B. "Prof. René François-Müller"), die in der gewählten Schriftart nicht standardmäßig enthalten sind. | Das Türschild stellt den Namen korrekt mit allen Sonderzeichen dar, ohne dass leere Kästchen (Fallbacks) oder Zeichenfehler angezeigt werden. | `FR-03`, `NFR-01` |
| **TC-04** | Error Case | Für einen Raum existieren in der Datenquelle keine Lehrer-Infos (Leeres Datenfeld/Null-Wert). | Das Türschild zeigt einen Standard-Platzhalter (z. B. "N.N.") an. Die Anwendung läuft stabil weiter. | `FR-03`, `NFR-03` |
| **TC-05** | Error Case | Klick auf einen Bereich außerhalb der definierten Räume/Türen (z. B. eine leere Wand). | Es öffnet sich keine Detailansicht, es wird kein Fehler geworfen und der Fokus bleibt auf der Hauptkarte. | `FR-05` |
