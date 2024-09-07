Infoga en ny kolumn mellan kommentarerna och första voltigören. Nya kolumnen ska ha bokstaven "m". 
På de rader som är lag: Skriv in lagnamn i kolumn m på.

Ändra ordning i excelarket på ekipagefliken så att tävlande på samma häst ligger i rätt ordning. Dvs den först voltigören på hästen måste ligga före den andra startande inom samma startlisteklass.
Lägg till rätt 

Lägg till ScoreSheetId i kolumn D i kolumnnen klasser

Kontrollera/ändra i web.config:
ContestId (1 = SM 2 = normal)
Trahasttavling (true/false) -> om true kolla HorsePointTraHastTavling



Lägg till en nytt blad i excel som heter "startlistklasser". Lägg till vilja startlisteklasser som ska ha vilka klasser 



Kör C:\Users\magnus.sandberg\Documents\Visual Studio 2015\Projects\VoltigeClosedXML\mallar\import\Töm_databas_innan_import.sql i databasen för att rensa tidigare import



OLD:

I DB: Gå in i StartListClassSteps och lägg upp rätt startlisteklasser

Ändra till rätt typ av tävling TeamSMCompetion, TeamOnedayCompetition, Trähast osv i metoden ImportTeams
Ändra till rätt typ av tävling TeamSMCompetion, TeamOnedayCompetition, Trähast osv i metoden ImportIndividuals

Ändra   StartListClassStepId = 19;            testNumber = 1; i respektive metod

 