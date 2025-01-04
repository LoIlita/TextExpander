# TextExpander

TextExpander to aplikacja do automatycznego rozwijania skrótów tekstowych, która pozwala na szybkie wstawianie często używanych fraz lub tekstów.

## Funkcje

- **Zarządzanie skrótami**

  - Dodawanie nowych skrótów
  - Edycja istniejących skrótów
  - Usuwanie skrótów
  - Podgląd wszystkich skrótów w przejrzystej tabeli

- **Automatyczne rozwijanie**

  - Wykrywanie wpisywanych skrótów
  - Natychmiastowe zastępowanie skrótów pełnym tekstem
  - Konfigurowalny klawisz aktywacyjny (hotkey)

- **Interfejs użytkownika**

  - Prosty i intuicyjny interfejs
  - Tryb jasny i ciemny
  - Zapamiętywanie pozycji i rozmiaru okna
  - Ikona w zasobniku systemowym

- **Dodatkowe funkcje**
  - Możliwość włączania/wyłączania nasłuchiwania skrótów
  - Automatyczne zapisywanie wszystkich ustawień
  - Szczegółowe logi działania aplikacji

## Jak używać

1. **Uruchomienie aplikacji**

   - Po uruchomieniu aplikacja pojawia się w zasobniku systemowym
   - Główne okno można zminimalizować - aplikacja działa w tle

2. **Dodawanie skrótów**

   - Kliknij przycisk "Add Shortcut"
   - Wpisz skrót i jego rozwinięcie
   - Zatwierdź przyciskiem OK

3. **Edycja skrótów**

   - Wybierz skrót z listy
   - Kliknij przycisk "Edit Shortcut" lub kliknij dwukrotnie na skrót
   - Zmodyfikuj skrót lub jego rozwinięcie
   - Zatwierdź zmiany

4. **Używanie skrótów**

   - Upewnij się, że nasłuchiwanie jest włączone (przycisk "Start Listening")
   - Wpisz zdefiniowany skrót w dowolnym miejscu
   - Naciśnij spację, aby rozwinąć skrót

5. **Zmiana motywu**

   - Użyj przycisku zmiany motywu, aby przełączać między jasnym i ciemnym tematem
   - Wybrany motyw zostanie zapamiętany

6. **Zmiana klawisza aktywacyjnego**
   - Kliknij przycisk "Change Hotkey"
   - Wybierz żądaną kombinację klawiszy
   - Zatwierdź zmiany

## Wymagania systemowe

- System Windows
- .NET Framework (wersja odpowiednia dla kompilacji)

## Pliki konfiguracyjne

- `shortcuts.json` - przechowuje zdefiniowane skróty
- `settings.json` - przechowuje ustawienia aplikacji
- `textexpander.log` - plik z logami aplikacji

## Uwagi

- Aplikacja musi mieć uprawnienia do nasłuchiwania klawiatury
- Zalecane jest regularne tworzenie kopii zapasowych pliku ze skrótami
- W przypadku problemów sprawdź plik logów
