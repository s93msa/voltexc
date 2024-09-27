USE [WebApplication1.Classes.VaultingContextProtokoll2019];  
GO  
BACKUP DATABASE [WebApplication1.Classes.VaultingContextProtokoll2019]
TO DISK = 'C:\temp\Voltige_innanrensning\WebApplication1.Classes.VaultingContext.Bak'  
   WITH FORMAT,  
      MEDIANAME = 'temp_WebApplication1.Classes.VaultingContext',  
      NAME = 'Full Backup of WebApplication1.Classes.VaultingContext'; 

BEGIN TRANSACTION; 
truncate table vaulterOrders
Delete from Vaulters
Delete from HorseOrders
Delete from TeamLists
Delete from Teams
Delete from Horses
Delete from Lungers
Delete from Clubs
Commit