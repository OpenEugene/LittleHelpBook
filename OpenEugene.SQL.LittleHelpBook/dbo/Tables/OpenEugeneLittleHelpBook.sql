CREATE TABLE [dbo].[OpenEugeneLittleHelpBook] (
    [LittleHelpBookId] INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]         INT            NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [CreatedBy]        NVARCHAR (256) NOT NULL,
    [CreatedOn]        DATETIME2 (7)  NOT NULL,
    [ModifiedBy]       NVARCHAR (256) NOT NULL,
    [ModifiedOn]       DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_OpenEugeneLittleHelpBook] PRIMARY KEY CLUSTERED ([LittleHelpBookId] ASC),
    CONSTRAINT [FK_OpenEugeneLittleHelpBook_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([ModuleId]) ON DELETE CASCADE
);

