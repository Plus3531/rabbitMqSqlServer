USE [RabbitMQTest]
GO

/****** Object:  Table [rmq].[tb_RabbitEndpoint]    Script Date: 10-Jul-17 17:13:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [rmq].[tb_RabbitEndpoint](
	[EndpointID] [int] IDENTITY(1,1) NOT NULL,
	[AliasName] [nvarchar](128) NOT NULL,
	[ServerName] [varchar](512) NOT NULL,
	[Port] [int] NOT NULL,
	[VHost] [nvarchar](256) NOT NULL,
	[LoginName] [varchar](256) NOT NULL,
	[LoginPassword] [varbinary](128) NOT NULL,
	[Exchange] [varchar](128) NOT NULL,
	[RoutingKey] [varchar](256) NULL,
	[ConnectionChannels] [int] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [pk_EndpointID] PRIMARY KEY CLUSTERED 
(
	[EndpointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [rmq].[tb_RabbitEndpoint] ADD  CONSTRAINT [df_RabbitEndpoint_Port]  DEFAULT ((5672)) FOR [Port]
GO

ALTER TABLE [rmq].[tb_RabbitEndpoint] ADD  CONSTRAINT [df_RabbitEndpoint_VHost]  DEFAULT ('/') FOR [VHost]
GO

ALTER TABLE [rmq].[tb_RabbitEndpoint] ADD  CONSTRAINT [df_RabbitEndpoint_ConnectionChannels]  DEFAULT ((5)) FOR [ConnectionChannels]
GO

ALTER TABLE [rmq].[tb_RabbitEndpoint] ADD  CONSTRAINT [df_RemoteServer_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
GO

