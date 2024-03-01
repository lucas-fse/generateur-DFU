
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/10/2021 10:12:08
-- Generated from EDMX file: C:\Users\fournier\source\repos\GenerateurDFUSafirV6\Models\DAL\PROD2.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PEGASE_PROD2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TEMPS_SAISIOPERATEURS]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TEMPS_SAISI] DROP CONSTRAINT [FK_TEMPS_SAISIOPERATEURS];
GO
IF OBJECT_ID(N'[dbo].[FK_TEMPS_SAISISOUSPROJET_SOUSPROJET]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TEMPS_SAISISOUSPROJET] DROP CONSTRAINT [FK_TEMPS_SAISISOUSPROJET_SOUSPROJET];
GO
IF OBJECT_ID(N'[dbo].[FK_TEMPS_SAISISOUSPROJET_TEMPS_SAISI]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TEMPS_SAISISOUSPROJET] DROP CONSTRAINT [FK_TEMPS_SAISISOUSPROJET_TEMPS_SAISI];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[OF_PROD_TRAITE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OF_PROD_TRAITE];
GO
IF OBJECT_ID(N'[dbo].[OPERATEURS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OPERATEURS];
GO
IF OBJECT_ID(N'[dbo].[ORDRE_FABRICATION_GENERE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ORDRE_FABRICATION_GENERE];
GO
IF OBJECT_ID(N'[dbo].[SOUSPROJET]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SOUSPROJET];
GO
IF OBJECT_ID(N'[dbo].[TEMPS_SAISI]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TEMPS_SAISI];
GO
IF OBJECT_ID(N'[dbo].[TEMPS_SAISISOUSPROJET]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TEMPS_SAISISOUSPROJET];
GO
IF OBJECT_ID(N'[dbo].[TEMPS_SEMAINE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TEMPS_SEMAINE];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'OPERATEURS'
CREATE TABLE [dbo].[OPERATEURS] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ANNIVERSAIRE] datetime  NULL,
    [POSTE] nvarchar(5)  NULL,
    [NOM] nvarchar(40)  NULL,
    [PRENOM] nvarchar(40)  NULL,
    [SERVICE] nchar(5)  NULL,
    [INITIAL] nchar(4)  NULL
);
GO

-- Creating table 'OF_PROD_TRAITE'
CREATE TABLE [dbo].[OF_PROD_TRAITE] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [NMROF] varchar(20)  NULL,
    [STARTTIME] datetime  NULL,
    [ENDTIME] datetime  NULL,
    [STATUSTYPE] nchar(10)  NULL,
    [QTRTHEORIQUE] int  NULL,
    [Alea] varchar(200)  NULL,
    [OPERATEUR] bigint  NULL,
    [ILOT] varchar(10)  NULL,
    [ITEMREF] varchar(30)  NULL,
    [QTRREEL] int  NULL,
    [TEMPSTHEORIQUE] float  NULL,
    [ISALIVE] bit  NOT NULL,
    [TEMPSSUPPL] float  NOT NULL
);
GO

-- Creating table 'ORDRE_FABRICATION_GENERE'
CREATE TABLE [dbo].[ORDRE_FABRICATION_GENERE] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [NUM_OF] nvarchar(20)  NOT NULL,
    [NUM_COMMANDE_CLIENT] nvarchar(20)  NULL,
    [REF_INDUSTRIELLE_MO] nvarchar(20)  NULL,
    [REF_COMMERCIALE_MO] nvarchar(20)  NULL,
    [REF_INDUSTRIELLE_MT] nvarchar(20)  NULL,
    [REF_COMMERCIALE_MT] nvarchar(20)  NULL,
    [REF_COMMERCIALE_SIM] nvarchar(20)  NULL,
    [REF_FIRMWARE_MO] nvarchar(20)  NULL,
    [REF_FIRMWARE_MT] nvarchar(20)  NULL,
    [REF_COMMERCIALE_PACK] nvarchar(20)  NULL,
    [OPTIONS_LOGICIELLES] nvarchar(20)  NULL,
    [REF_FICHE_PERSO] nvarchar(20)  NULL,
    [NUM_SERIE_PACK] nvarchar(20)  NULL,
    [NB_PACK] int  NULL,
    [NB_MO] int  NULL,
    [NB_MT] int  NULL,
    [NB_SIM] int  NULL,
    [DATE_GENERATION] datetime  NULL,
    [OPTION_MATERIEL_MO] nvarchar(32)  NULL,
    [OPTION_MATERIEL_MT] nvarchar(32)  NULL,
    [COMMANDE_SYNCHRO] nvarchar(32)  NULL,
    [GENERE] bit  NULL,
    [MARCHE] nchar(1)  NULL,
    [MODIF_MANUEL] bit  NOT NULL,
    [VERSION_LOG] nvarchar(30)  NULL
);
GO

-- Creating table 'TEMPS_SAISI'
CREATE TABLE [dbo].[TEMPS_SAISI] (
    [ID] bigint  NOT NULL,
    [Annee] smallint  NOT NULL,
    [Semaine] smallint  NOT NULL,
    [LigneProjet] int  NULL,
    [Days1] smallint  NOT NULL,
    [Days2] smallint  NOT NULL,
    [Days3] smallint  NOT NULL,
    [Days4] smallint  NOT NULL,
    [Days5] smallint  NOT NULL,
    [Days6] smallint  NOT NULL,
    [Days7] smallint  NOT NULL,
    [Complete] bit  NOT NULL,
    [OPERATEURS_ID] bigint  NOT NULL
);
GO

-- Creating table 'SOUSPROJET'
CREATE TABLE [dbo].[SOUSPROJET] (
    [ID] bigint  NOT NULL,
    [IDSOUSPROJET] bigint  NOT NULL,
    [NomSousProjet] nchar(10)  NULL
);
GO

-- Creating table 'TEMPS_SEMAINE'
CREATE TABLE [dbo].[TEMPS_SEMAINE] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Annee] smallint  NULL,
    [Semaine] smallint  NULL,
    [Complete] bit  NULL,
    [OPERATEURS_ID] bigint  NOT NULL
);
GO

-- Creating table 'TEMPS_SAISISOUSPROJET'
CREATE TABLE [dbo].[TEMPS_SAISISOUSPROJET] (
    [TEMPS_SAISI_ID] bigint  NOT NULL,
    [SOUSPROJET_ID] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'OPERATEURS'
ALTER TABLE [dbo].[OPERATEURS]
ADD CONSTRAINT [PK_OPERATEURS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'OF_PROD_TRAITE'
ALTER TABLE [dbo].[OF_PROD_TRAITE]
ADD CONSTRAINT [PK_OF_PROD_TRAITE]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ORDRE_FABRICATION_GENERE'
ALTER TABLE [dbo].[ORDRE_FABRICATION_GENERE]
ADD CONSTRAINT [PK_ORDRE_FABRICATION_GENERE]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TEMPS_SAISI'
ALTER TABLE [dbo].[TEMPS_SAISI]
ADD CONSTRAINT [PK_TEMPS_SAISI]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SOUSPROJET'
ALTER TABLE [dbo].[SOUSPROJET]
ADD CONSTRAINT [PK_SOUSPROJET]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TEMPS_SEMAINE'
ALTER TABLE [dbo].[TEMPS_SEMAINE]
ADD CONSTRAINT [PK_TEMPS_SEMAINE]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [TEMPS_SAISI_ID], [SOUSPROJET_ID] in table 'TEMPS_SAISISOUSPROJET'
ALTER TABLE [dbo].[TEMPS_SAISISOUSPROJET]
ADD CONSTRAINT [PK_TEMPS_SAISISOUSPROJET]
    PRIMARY KEY CLUSTERED ([TEMPS_SAISI_ID], [SOUSPROJET_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TEMPS_SAISI_ID] in table 'TEMPS_SAISISOUSPROJET'
ALTER TABLE [dbo].[TEMPS_SAISISOUSPROJET]
ADD CONSTRAINT [FK_TEMPS_SAISISOUSPROJET_TEMPS_SAISI]
    FOREIGN KEY ([TEMPS_SAISI_ID])
    REFERENCES [dbo].[TEMPS_SAISI]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SOUSPROJET_ID] in table 'TEMPS_SAISISOUSPROJET'
ALTER TABLE [dbo].[TEMPS_SAISISOUSPROJET]
ADD CONSTRAINT [FK_TEMPS_SAISISOUSPROJET_SOUSPROJET]
    FOREIGN KEY ([SOUSPROJET_ID])
    REFERENCES [dbo].[SOUSPROJET]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TEMPS_SAISISOUSPROJET_SOUSPROJET'
CREATE INDEX [IX_FK_TEMPS_SAISISOUSPROJET_SOUSPROJET]
ON [dbo].[TEMPS_SAISISOUSPROJET]
    ([SOUSPROJET_ID]);
GO

-- Creating foreign key on [OPERATEURS_ID] in table 'TEMPS_SAISI'
ALTER TABLE [dbo].[TEMPS_SAISI]
ADD CONSTRAINT [FK_TEMPS_SAISIOPERATEURS]
    FOREIGN KEY ([OPERATEURS_ID])
    REFERENCES [dbo].[OPERATEURS]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TEMPS_SAISIOPERATEURS'
CREATE INDEX [IX_FK_TEMPS_SAISIOPERATEURS]
ON [dbo].[TEMPS_SAISI]
    ([OPERATEURS_ID]);
GO

-- Creating foreign key on [OPERATEURS_ID] in table 'TEMPS_SEMAINE'
ALTER TABLE [dbo].[TEMPS_SEMAINE]
ADD CONSTRAINT [FK_OPERATEURSTEMPS_SEMAINE]
    FOREIGN KEY ([OPERATEURS_ID])
    REFERENCES [dbo].[OPERATEURS]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OPERATEURSTEMPS_SEMAINE'
CREATE INDEX [IX_FK_OPERATEURSTEMPS_SEMAINE]
ON [dbo].[TEMPS_SEMAINE]
    ([OPERATEURS_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------