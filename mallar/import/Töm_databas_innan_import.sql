
--Testa f�rst att backupen funkar innan du k�r delete och truncate
USE [WebApplication1.Classes.VaultingContext];  
GO  
BACKUP DATABASE [WebApplication1.Classes.VaultingContext]
TO DISK = 'C:\temp\Voltige_innanrensning\WebApplication1.Classes.VaultingContext.Bak'  
   WITH FORMAT,  
      MEDIANAME = 'temp_WebApplication1.Classes.VaultingContext',  
      NAME = 'Full Backup of WebApplication1.Classes.VaultingContext'; 

BEGIN TRANSACTION; 
--truncate table vaulterOrders
--Delete from Vaulters
--Delete from HorseOrders
--Delete from TeamLists
--Delete from Teams
--Delete from Horses
--Delete from Lungers
--Delete from Clubs
Commit
