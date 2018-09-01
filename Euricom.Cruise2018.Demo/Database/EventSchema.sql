USE [Euricom_Cruise2018_Demo]
GO

IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Event')
DROP SCHEMA [Event]
GO

CREATE SCHEMA [Event] AUTHORIZATION [dbo]
GO