Infoga en ny kolumn mellan kommentarerna och f�rsta voltig�ren. Nya kolumnen ska ha bokstaven "m". 
P� de rader som �r lag: Skriv in lagnamn i kolumn m p�.

�ndra ordning i excelarket p� ekipagefliken s� att t�vlande p� samma h�st ligger i r�tt ordning. Dvs den f�rst voltig�ren p� h�sten m�ste ligga f�re den andra startande inom samma startlisteklass.
L�gg till r�tt 

L�gg till ScoreSheetId i kolumn D i kolumnnen klasser

Kontrollera/�ndra i web.config:
ContestId (1 = SM 2 = normal)
Trahasttavling (true/false) -> om true kolla HorsePointTraHastTavling



L�gg till en nytt blad i excel som heter "startlistklasser". L�gg till vilja startlisteklasser som ska ha vilka klasser 



K�r C:\Users\magnus.sandberg\Documents\Visual Studio 2015\Projects\VoltigeClosedXML\mallar\import\T�m_databas_innan_import.sql i databasen f�r att rensa tidigare import



OLD:

I DB: G� in i StartListClassSteps och l�gg upp r�tt startlisteklasser

�ndra till r�tt typ av t�vling TeamSMCompetion, TeamOnedayCompetition, Tr�hast osv i metoden ImportTeams
�ndra till r�tt typ av t�vling TeamSMCompetion, TeamOnedayCompetition, Tr�hast osv i metoden ImportIndividuals

�ndra   StartListClassStepId = 19;            testNumber = 1; i respektive metod

 