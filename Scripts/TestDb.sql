--Создание базы TestDb
IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE NAME = 'TestDb')
BEGIN
CREATE DATABASE TestDb;
END
GO

USE TestDb;

--Создание таблицы Product
IF NOT EXISTS (SELECT * FROM sys.Tables WHERE NAME = 'Product')
BEGIN
CREATE TABLE [dbo].[Product](
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL UNIQUE,
	[Description] VARCHAR(MAX) NULL
)
END
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_name')   
    DROP INDEX index_name ON dbo.Product;   
GO  
CREATE NONCLUSTERED INDEX index_name   
    ON Product(Name);   
GO 

--Создание таблицы ProductVersion
IF NOT EXISTS (SELECT * FROM sys.Tables WHERE NAME = 'ProductVersion')
BEGIN
CREATE TABLE [dbo].[ProductVersion](
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[ProductID] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(255) NOT NULL UNIQUE,
	[Description] VARCHAR(MAX) NULL,
    [CreatingDate] DATETIME NOT NULL DEFAULT GETDATE(),
	[Width] FLOAT NOT NULL CHECK ([Width] >= 0),
	[Height] FLOAT NOT NULL CHECK ([Height] >= 0),
	[Length] FLOAT NOT NULL CHECK ([Length] >= 0),
	FOREIGN KEY (ProductID) REFERENCES Product (ID) ON DELETE CASCADE
)
END
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_name_version')   
    DROP INDEX index_name_version ON dbo.ProductVersion;   
GO  
CREATE NONCLUSTERED INDEX index_name_version   
    ON ProductVersion(Name);   
GO 

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_date_version')   
    DROP INDEX index_date_version ON dbo.ProductVersion;   
GO  
CREATE NONCLUSTERED INDEX index_date_version   
    ON ProductVersion(CreatingDate);   
GO 

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_width_version')   
    DROP INDEX index_width_version ON dbo.ProductVersion;   
GO  
CREATE NONCLUSTERED INDEX index_width_version   
    ON ProductVersion(Width);   
GO 

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_height_version')   
    DROP INDEX index_height_version ON dbo.ProductVersion;   
GO  
CREATE NONCLUSTERED INDEX index_height_version   
    ON ProductVersion(Height);   
GO 

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_length_version')   
    DROP INDEX index_length_version ON dbo.ProductVersion;   
GO  
CREATE NONCLUSTERED INDEX index_length_version   
    ON ProductVersion(Length);   
GO 

--Создание таблицы EventLog
IF NOT EXISTS (SELECT * FROM sys.Tables WHERE NAME = 'EventLog')
BEGIN
CREATE TABLE [dbo].[EventLog](
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [EventDate] DATETIME NOT NULL DEFAULT GETDATE(),
	[Description] VARCHAR(MAX) NULL
)
END
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = 'index_date_event')   
    DROP INDEX index_date_event ON dbo.EventLog;   
GO  
CREATE NONCLUSTERED INDEX index_date_event   
    ON EventLog (EventDate);   
GO 

IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'Product_INSERT')
BEGIN
	DROP TRIGGER Product_INSERT
END
GO
CREATE TRIGGER Product_INSERT
ON Product
AFTER INSERT 
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Добавлено изделие ' + Name 
FROM INSERTED
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'Product_UPDATE')
BEGIN
	DROP TRIGGER Product_UPDATE
END
GO
CREATE TRIGGER Product_UPDATE
ON Product
AFTER UPDATE
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Изменено изделие ' + Name 
FROM INSERTED
GO


IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'Product_DELETE')
BEGIN
	DROP TRIGGER Product_DELETE
END
GO
CREATE TRIGGER Product_DELETE
ON Product
AFTER DELETE
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Удалено изделие ' + Name 
FROM DELETED
GO


IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'ProductVersion_INSERT')
BEGIN
	DROP TRIGGER ProductVersion_INSERT
END
GO
CREATE TRIGGER ProductVersion_INSERT
ON Product
AFTER INSERT
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Добавлена версия изделия ' + Name 
FROM INSERTED
GO


IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'ProductVersion_UPDATE')
BEGIN
	DROP TRIGGER ProductVersion_UPDATE
END
GO
CREATE TRIGGER ProductVersion_UPDATE
ON Product
AFTER UPDATE
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Изменена версия изделия ' + Name 
FROM INSERTED
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE NAME = 'ProductVersion_DELETE')
BEGIN
	DROP TRIGGER ProductVersion_DELETE
END
GO
CREATE TRIGGER ProductVersion_DELETE
ON Product
AFTER DELETE
AS
INSERT INTO EventLog (ID, EventDate, Description)
SELECT NEWID(), GETDATE(), 'Удалена версия изделия ' + Name 
FROM DELETED
GO



--Создание функции поиска
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION' and ROUTINE_NAME = 'func_ProductSearch')
begin
	drop FUNCTION [dbo].[func_ProductSearch]
end
GO

CREATE FUNCTION [dbo].[func_ProductSearch] (@name varchar(255), @nameVersion varchar(255), @minVolume float, @maxVolume float)
RETURNS TABLE
AS
RETURN
(
	SELECT v.ID, p.Name, v.Name as NameVersion, v.Width, v.Length, v.Height
    FROM Product AS p
    LEFT JOIN ProductVersion v ON v.ProductID = p.ID   
	WHERE (CHARINDEX (@name, p.Name) > 0 or @name is null or @name = '')
	AND (CHARINDEX (@nameVersion, v.Name) > 0 or @nameVersion is null or @nameVersion = '')
	AND (v.Height*v.Length*v.Width >= @minVolume or @minVolume is null)
	AND (v.Height*v.Length*v.Width <= @maxVolume or @maxVolume is null)
);
GO

IF NOT EXISTS(select * from dbo.Product)
BEGIN
--Наполнение таблицы Product
INSERT INTO Product (ID, Name, Description)
SELECT NEWID(), 'Холодильник', ''

INSERT INTO Product (ID, Name, Description)
SELECT NEWID(), 'Электричекая плита', 'Кухонная плита, работающая на электричестве'

INSERT INTO Product (ID, Name, Description)
SELECT NEWID(), 'Индукционная плита', ''

INSERT INTO Product (ID, Name, Description)
SELECT NEWID(), 'Газовая плита', ''

INSERT INTO Product (ID, Name, Description)
SELECT NEWID(), 'Стиральная машина',''

--Наполнение таблицы ProductVersion
INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
SELECT NEWID(), p.ID , 'Beko RCSK 335M20 W','Простая надежная модель',GETDATE(), 600, 2010, 540
FROM Product AS p
WHERE Name = 'Холодильник'

INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
SELECT NEWID(), p.ID , 'Атлант ХМ 6021-031',NULL,GETDATE(), 630, 1860.5 , 600
FROM Product AS p
WHERE Name = 'Холодильник'

INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
SELECT NEWID(), p.ID , 'Атлант ХМ 6024-031',NULL,GETDATE(), 630, 1950, 600
FROM Product AS p
WHERE Name = 'Холодильник'

INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
SELECT NEWID(), p.ID , 'Zanussi ZEI 5680 FB','Простая надежная модель',GETDATE(), 400, 600, 400.45
FROM Product AS p
WHERE Name = 'Индукционная плита'

INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
SELECT NEWID(), p.ID , 'Bosch HXG130B20R', NULL, GETDATE(), 500, 850 , 600
FROM Product AS p
WHERE Name = 'Газовая плита'
END