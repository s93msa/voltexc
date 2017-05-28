alter VIEW [dbo].StartListHorses
	AS SELECT StartListClasses.Name AS StartlistClass, StartListClassSteps.Name AS Omgang, StartLists.StartNumber, Horses.HorseName, StartLists.HorseInformation_HorseId
FROM  StartListClasses LEFT OUTER JOIN
         StartListClassSteps ON StartListClasses.StartListClassId = StartListClassSteps.StartListClass_StartListClassId LEFT OUTER JOIN
         StartLists ON StartListClassSteps.StartListClassStepId = StartLists.StartListClassStep_StartListClassStepId Right OUTER JOIN
         Horses ON StartLists.HorseInformation_HorseId = Horses.HorseId