IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Polls] (
    [Id] int NOT NULL IDENTITY,
    [Title] varchar(100) NOT NULL,
    [Summary] varchar(1500) NOT NULL,
    [IsPublished] bit NOT NULL,
    [StartAt] datetime2 NOT NULL,
    [EndAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Polls] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_Polls_Title] ON [Polls] ([Title]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260305184316_init', N'8.0.3');
GO

COMMIT;
GO

