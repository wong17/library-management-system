-- CREATE STORED PROCEDURES FOR Author TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

-- INSERT Author
IF OBJECT_ID('Library.uspInsertAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertAuthor;  
GO
CREATE PROC [Library].uspInsertAuthor (
	@Name VARCHAR(100),
	@IsFormerGraduated BIT
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Author WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Autor con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Author ([Name], IsFormerGraduated) VALUES (@Name, @IsFormerGraduated)
		--
		SELECT 0 AS IsSuccess, 'Autor registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY Author
IF OBJECT_ID('Library.uspInsertManyAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyAuthor;  
GO
CREATE PROC [Library].uspInsertManyAuthor (
	@Authors [Library].AuthorType READONLY
)
AS
BEGIN
	-- Verificar que el nombre de todas los Autores no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Authors AS a WHERE a.[Name] IS NULL OR a.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todos los Autores solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @Authors AS a WHERE a.[Name] LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Autores repetidos
	IF EXISTS (SELECT [Name] FROM @Authors GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Autores están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Autores con el mismo nombre en la base de datos
	IF EXISTS (SELECT a.[Name] FROM @Authors AS a INNER JOIN [Library].Author AS db ON a.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Autores ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción y capturar los ID insertados en la tabla temporal
		DECLARE @InsertedIDs TABLE (ID INT);

		INSERT INTO [Library].Author([Name]) 
		OUTPUT inserted.AuthorId INTO @InsertedIDs(ID)
		SELECT a.[Name] FROM @Authors AS a
		--
		SELECT 0 AS IsSuccess, 'Autores registrados exitosamente' AS [Message];
		-- Devuelve los ID de los registros insertados
        SELECT ID AS InsertedID FROM @InsertedIDs;
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

--UPDATE Author
IF OBJECT_ID('Library.uspUpdateAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateAuthor;  
GO
CREATE PROC [Library].uspUpdateAuthor (
	@AuthorId INT,
	@Name VARCHAR(100),
	@IsFormerGraduated BIT
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Author WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Autor con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Author SET [Name] = @Name, IsFormerGraduated = @IsFormerGraduated WHERE AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor actualizado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE MANY Author
IF OBJECT_ID('Library.uspUpdateManyAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateManyAuthor;  
GO
CREATE PROC [Library].uspUpdateManyAuthor (
	@Authors [Library].AuthorType READONLY
)
AS
BEGIN
	-- Verificar que el Id del Autor no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Authors AS a WHERE a.AuthorId IS NULL OR a.AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Autores a actualizar
	IF EXISTS (SELECT 1 FROM @Authors AS a LEFT JOIN [Library].Author AS db ON a.AuthorId = db.AuthorId WHERE db.AuthorId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o mas Autores no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el nombre de todas los Autores no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Authors AS a WHERE a.[Name] IS NULL OR a.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todos los Autores solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @Authors AS a WHERE a.[Name] LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Autores repetidos
	IF EXISTS (SELECT 1 FROM @Authors GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Autores están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Autores con el mismo nombre en la base de datos
	IF EXISTS (SELECT 1 FROM @Authors AS a INNER JOIN [Library].Author AS db ON a.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Autores ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Actualizar todos los registros
		UPDATE db
		SET db.[Name] = a.[Name], db.IsFormerGraduated = a.IsFormerGraduated
		FROM [Library].Author AS db
		INNER JOIN @Authors AS a ON db.AuthorId = a.AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autores actualizadas exitosamente' AS [Message]
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

--DELETE Author
IF OBJECT_ID('Library.uspDeleteAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteAuthor;  
GO
CREATE PROC [Library].uspDeleteAuthor (
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Autor porque hay libros asociados a él' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].MonographAuthor WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Autor porque hay monografías asociados a él' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Author WHERE AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Author
IF OBJECT_ID('Library.uspGetAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetAuthor;  
GO
CREATE PROC [Library].uspGetAuthor (
	@AuthorId INT = NULL
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT AuthorId, [Name], IsFormerGraduated FROM [Library].Author
	END
	ELSE
	BEGIN
		SELECT AuthorId, [Name], IsFormerGraduated FROM [Library].Author WHERE AuthorId = @AuthorId
	END
END
GO