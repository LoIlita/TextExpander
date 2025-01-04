# TextExpander

TextExpander to prosta aplikacja do rozwijania skrótów tekstowych. Pozwala na zdefiniowanie własnych skrótów, które są automatycznie zamieniane na dłuższe frazy podczas pisania.

## Funkcje

- **Konfigurowalny klawisz aktywacyjny (hotkey)**

  - Możliwość wyboru kombinacji klawiszy (Ctrl/Alt/Shift + litera)
  - Domyślnie ustawiony na Ctrl+M

- **Zarządzanie skrótami**

  - Dodawanie nowych skrótów
  - Edycja istniejących skrótów
  - Usuwanie skrótów
  - Lista wszystkich skrótów z podglądem ich rozwinięć

- **Tryb nasłuchiwania**

  - Włączanie/wyłączanie nasłuchiwania klawiszy
  - Wskaźnik aktualnego stanu nasłuchiwania

- **Integracja z systemem**
  - Ikona w zasobniku systemowym
  - Minimalizacja do zasobnika systemowego
  - Automatyczne uruchamianie przy starcie systemu

## Jak używać

1. **Uruchomienie aplikacji**

   - Po uruchomieniu aplikacja pojawia się w zasobniku systemowym
   - Kliknij "Start Listening" aby rozpocząć nasłuchiwanie

2. **Dodawanie skrótów**

   - Kliknij przycisk "Dodaj"
   - Wpisz skrót i jego rozwinięcie
   - Zatwierdź przyciskiem OK

3. **Używanie skrótów**

   - Naciśnij zdefiniowany hotkey (domyślnie Ctrl+M)
   - Wpisz skrót
   - Naciśnij spację - skrót zostanie automatycznie zamieniony na jego rozwinięcie

4. **Zmiana hotkey**
   - Kliknij przycisk "Zmień hotkey"
   - Wybierz żądaną kombinację klawiszy (co najmniej jeden modyfikator + litera)
   - Zatwierdź przyciskiem OK

## Wymagania systemowe

- System Windows
- .NET Framework 6.0 lub nowszy

## Instalacja

1. Pobierz najnowszą wersję aplikacji
2. Rozpakuj archiwum do wybranego katalogu
3. Uruchom plik TextExpander.exe

## Konfiguracja

- Skróty są zapisywane w pliku `shortcuts.json`
- Ustawienia aplikacji są zapisywane w pliku `settings.json`
- Logi aplikacji znajdują się w pliku `textexpander.log`

## Wsparcie

W przypadku problemów lub pytań:

1. Sprawdź logi aplikacji w pliku `textexpander.log`
2. Zgłoś problem w zakładce Issues na GitHubie
