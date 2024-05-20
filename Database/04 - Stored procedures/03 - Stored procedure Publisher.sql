-- CREATE STORED PROCEDURES FOR Publisher TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- INSERT Publisher
IF OBJECT_ID('Library.uspInsertPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertPublisher;  
GO
CREATE PROC [Library].uspInsertPublisher (
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Publisher WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Editorial con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Publisher ([Name]) VALUES (@Name)
		--
		SELECT 0 AS IsSuccess, 'Editorial registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY Publisher
IF OBJECT_ID('Library.uspInsertManyPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyPublisher;  
GO
CREATE PROC [Library].uspInsertManyPublisher (
	@Publishers [Library].PublisherType READONLY
)
AS
BEGIN
	-- Verificar que el nombre de todas las Editorial no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Publishers AS p WHERE p.[Name] IS NULL OR p.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las Editoriales solo tenga mayúsculas, minúsculas, guiones, puntos y espacios
	IF EXISTS (SELECT 1 FROM @Publishers AS p WHERE p.[Name] LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Editoriales repetidos
	IF EXISTS (SELECT [Name] FROM @Publishers GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más nombres están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Editoriales con el mismo nombre en la base de datos
	IF EXISTS (SELECT p.[Name] FROM @Publishers AS p INNER JOIN [Library].Publisher AS db ON p.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más nombres ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción y capturar los ID insertados en la tabla temporal
		DECLARE @InsertedIDs TABLE (ID INT);

		INSERT INTO [Library].Publisher ([Name]) 
		OUTPUT inserted.PublisherId INTO @InsertedIDs(ID)
		SELECT [Name] FROM @Publishers
		--
		SELECT 0 AS IsSuccess, 'Editoriales registradas exitosamente' AS [Message];
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
        SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- UPDATE Publisher
IF OBJECT_ID('Library.uspUpdatePublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdatePublisher;  
GO
CREATE PROC [Library].uspUpdatePublisher (
	@PublisherId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Publisher WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Editorial con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Publisher SET [Name] = @Name WHERE PublisherId = @PublisherId
		--
		SELECT 0 AS IsSuccess, 'Editorial actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE MANY Publisher
IF OBJECT_ID('Library.uspUpdateManyPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateManyPublisher;  
GO
CREATE PROC [Library].uspUpdateManyPublisher (
	@Publishers [Library].PublisherType READONLY
)
AS
BEGIN
	-- Verificar que el Id de la Editorial no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Publishers AS p WHERE p.PublisherId IS NULL OR p.PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las Editoriales a actualizar
	IF EXISTS (
        SELECT p.PublisherId
        FROM @Publishers AS p
        LEFT JOIN [Library].Publisher AS db ON p.PublisherId = db.PublisherId
        WHERE db.PublisherId IS NULL
    )
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Editoriales no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el nombre de todas las Editorial no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Publishers AS p WHERE p.[Name] IS NULL OR p.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las Editoriales solo tenga mayúsculas, minúsculas, guiones, puntos y espacios
	IF EXISTS (SELECT 1 FROM @Publishers AS p WHERE p.[Name] LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Editoriales repetidos
	IF EXISTS (SELECT [Name] FROM @Publishers GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más nombres están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Editoriales con el mismo nombre en la base de datos
	IF EXISTS (SELECT p.[Name] FROM @Publishers AS p INNER JOIN [Library].Publisher AS db ON p.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más nombres ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Actualizar todos los registros
		UPDATE db 
		SET db.[Name] = p.[Name]
		FROM [Library].Publisher AS db
		INNER JOIN @Publishers AS p ON db.PublisherId = p.PublisherId
		--
		SELECT 0 AS IsSuccess, 'Editoriales actualizadas exitosamente' AS [Message]
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- DELETE Publisher
IF OBJECT_ID('Library.uspDeletePublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeletePublisher;  
GO
CREATE PROC [Library].uspDeletePublisher (
	@PublisherId INT
)
AS
BEGIN
	--
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Editorial porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Publisher WHERE PublisherId = @PublisherId
		--
		SELECT 0 AS IsSuccess, 'Editorial eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- GET Publisher
IF OBJECT_ID('Library.uspGetPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetPublisher;  
GO
CREATE PROC [Library].uspGetPublisher (
	@PublisherId INT = NULL
)
AS
BEGIN
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT PublisherId, [Name] FROM [Library].Publisher
	END
	ELSE
	BEGIN
		SELECT PublisherId, [Name] FROM [Library].Publisher WHERE PublisherId = @PublisherId
	END
END
GO
