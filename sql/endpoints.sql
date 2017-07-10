USE [RabbitMQTest]
GO

/****** Object:  StoredProcedure [rmq].[pr_GetRabbitEndpoints]    Script Date: 10-Jul-17 17:05:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [rmq].[pr_GetRabbitEndpoints]
AS

SET NOCOUNT ON;

SELECT EndpointID, 
       AliasName, 
	   ServerName, 
	   Port, 
	   VHost, 
	   LoginName, 
	   CAST(LoginPassword AS nvarchar(256)) AS LoginPassword, 
	   Exchange, 
	   RoutingKey, 
	   ConnectionChannels, 
	   IsEnabled
FROM rmq.tb_RabbitEndpoint
WHERE IsEnabled = 1;


GO


