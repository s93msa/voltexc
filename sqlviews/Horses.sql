CREATE VIEW HorsesView
	AS SELECT * FROM [Horses] join Lungers on dbo.Horses.Lunger_LungerId = Lungers.LungerId