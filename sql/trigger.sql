USE [RabbitMQTest]
GO

/****** Object:  Trigger [dbo].[UpsertTestTable]    Script Date: 10-Jul-17 17:12:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[UpsertTestTable] 
   ON [dbo].[rmqTestTable] 
   AFTER INSERT, UPDATE
AS 
BEGIN
 SET NOCOUNT ON;
  BEGIN TRY
	DECLARE @msg nvarchar(max);
	DECLARE @xml xml;
	DECLARE @endPointId int;

	IF NOT EXISTS(SELECT * FROM INSERTED) RETURN;

	SET @xml = (SELECT r.Id, r.Naam, r.Afgemeld, r.Datum, r.Lengte
		FROM rmqTestTable r INNER JOIN inserted ON r.Id = inserted.Id
		FOR XML RAW ('MyRecord'), ROOT ('Rabbits'));
	SET @msg = CONVERT(NVARCHAR(max), @xml);	
	print @msg;
	SET @endPointId = 1
	--set @msg = 'test';
	EXEC [dbo].[SqlStoredProcedure1] @msg
	 END TRY
  BEGIN CATCH
    DECLARE @errMsg nvarchar(max);
    DECLARE @errLine int;
    SELECT @errMsg = ERROR_MESSAGE(), @errLine = ERROR_LINE();
    RAISERROR('Error: %s at line: %d', 16, -1, @errMsg, @errLine);
  END CATCH
END

GO

ALTER TABLE [dbo].[rmqTestTable] ENABLE TRIGGER [UpsertTestTable]
GO

