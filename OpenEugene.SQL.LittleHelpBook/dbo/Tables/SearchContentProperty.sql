﻿CREATE TABLE [dbo].[SearchContentProperty] (
    [PropertyId]      INT           IDENTITY (1, 1) NOT NULL,
    [SearchContentId] INT           NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [Value]           NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SearchContentProperty] PRIMARY KEY CLUSTERED ([PropertyId] ASC),
    CONSTRAINT [FK_SearchContentProperty_SearchContent] FOREIGN KEY ([SearchContentId]) REFERENCES [dbo].[SearchContent] ([SearchContentId]) ON DELETE CASCADE
);

