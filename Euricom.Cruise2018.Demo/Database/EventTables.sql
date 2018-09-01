USE [Euricom_Cruise2018_Demo]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Event].[Events]') AND type in (N'U'))
DROP TABLE [Event].[Events]
GO

CREATE TABLE [Event].[Events](
	[Ordering] [bigint] IDENTITY(1,1) NOT NULL,
	[PersistenceId] [nvarchar](255) NOT NULL,
	[SequenceNr] [bigint] NOT NULL,
	[Timestamp] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Manifest] [nvarchar](500) NOT NULL,
	[Payload] [varbinary](max) NOT NULL,
	[Tags] [nvarchar](100) NULL,
	[SerializerId] [int] NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Ordering] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Events] UNIQUE NONCLUSTERED 
(
	[PersistenceId] ASC,
	[SequenceNr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_Events_SequenceNr] ON [Event].[Events] 
(
	[SequenceNr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Events_Timestamp] ON [Event].[Events] 
(
	[Timestamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Event].[Metadata]') AND type in (N'U'))
DROP TABLE [Event].[Metadata]
GO

CREATE TABLE [Event].[Metadata](
	[PersistenceId] [nvarchar](255) NOT NULL,
	[SequenceNr] [bigint] NOT NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[PersistenceId] ASC,
	[SequenceNr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO