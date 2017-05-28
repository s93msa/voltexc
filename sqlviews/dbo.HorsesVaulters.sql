alter VIEW [dbo].HorsesVaulters
	AS SELECT Horses.HorseName, StartLists.IsTeam, VaulterOrders.Testnumber, VaulterOrders.Participant_VaulterId, VaulterOrders.StartList_StartListId, VaulterOrders.StartOrder AS VaulterStartOrder, Vaulters.VaulterId, Vaulters.Name, Vaulters.Armband, Vaulters.VaultingClub_ClubId, Vaulters.VaultingClass_CompetitionClassId, Vaulters.Active
FROM  Horses LEFT OUTER JOIN
         StartLists ON Horses.HorseId = StartLists.HorseInformation_HorseId LEFT OUTER JOIN
         VaulterOrders ON StartLists.StartListId = VaulterOrders.StartList_StartListId LEFT OUTER JOIN
         Vaulters ON VaulterOrders.Participant_VaulterId = Vaulters.VaulterId