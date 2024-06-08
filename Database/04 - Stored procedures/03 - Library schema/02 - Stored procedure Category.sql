-- CREATE STORED PROCEDURES FOR Category TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

-- INSERT Category
IF OBJECT_ID('Library.uspInsertCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertCategory;  
GO
CREATE PROC [Library].uspInsertCategory (
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Category WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Categoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Category ([Name]) VALUES (@Name)
		--
		SELECT 0 AS IsSuccess, 'Categoría registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY Category
IF OBJECT_ID('Library.uspInsertManyCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyCategory;  
GO
CREATE PROC [Library].uspInsertManyCategory (
	@Categories [Library].CategoryType READONLY
)
AS
BEGIN
	-- Verificar que el nombre de todas las Categorias no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Categories AS c WHERE c.[Name] IS NULL OR c.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las Categorias solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @Categories AS c WHERE c.[Name] LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoria solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Categorias repetidos
	IF EXISTS (SELECT 1 FROM @Categories GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Categorias están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Categorias con el mismo nombre en la base de datos
	IF EXISTS (SELECT 1 FROM @Categories AS c INNER JOIN [Library].Category AS db ON c.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Categorias ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción y capturar los ID insertados en la tabla temporal
		DECLARE @InsertedIDs TABLE (ID INT);

		INSERT INTO [Library].Category([Name]) 
		OUTPUT inserted.CategoryId INTO @InsertedIDs(ID)
		SELECT [Name] FROM @Categories
		--
		SELECT 0 AS IsSuccess, 'Categorias registradas exitosamente' AS [Message];
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

--UPDATE Category
IF OBJECT_ID('Library.uspUpdateCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateCategory;  
GO
CREATE PROC [Library].uspUpdateCategory (
	@CategoryId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Category WHERE [Name] = @Name AND CategoryId != @CategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Categoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Category SET [Name] = @Name WHERE CategoryId = @CategoryId
		--
		SELECT 0 AS IsSuccess, 'Categoría actualizada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE MANY Categories
IF OBJECT_ID('Library.uspUpdateManyCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateManyCategory;  
GO
CREATE PROC [Library].uspUpdateManyCategory (
	@Categories [Library].CategoryType READONLY
)
AS
BEGIN
	-- Verificar que el Id de la Categoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Categories AS c WHERE c.CategoryId IS NULL OR c.CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las Categorias a actualizar
	IF EXISTS (SELECT 1 FROM @Categories AS c LEFT JOIN [Library].Category AS db ON c.CategoryId = db.CategoryId WHERE db.CategoryId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Categorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el nombre de todas las Categorias no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @Categories AS c WHERE c.[Name] IS NULL OR c.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las Categorias solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @Categories AS c WHERE c.[Name] LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoria solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de Categorias repetidos
	IF EXISTS (SELECT 1 FROM @Categories GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Categorias están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan Categorias con el mismo nombre en la base de datos
	IF EXISTS (SELECT 1 FROM @Categories AS c INNER JOIN [Library].Category AS db ON c.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Categorias ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Actualizar todos los registros
		UPDATE db
		SET db.[Name] = c.[Name]
		FROM [Library].Category AS db
		INNER JOIN @Categories AS c ON db.CategoryId = c.CategoryId
		--
		SELECT 0 AS IsSuccess, 'Categorias actualizadas exitosamente' AS [Message]
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

--DELETE Category
IF OBJECT_ID('Library.uspDeleteCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteCategory;  
GO
CREATE PROC [Library].uspDeleteCategory (
	@CategoryId INT
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Categoría porque hay Sub categorías asociados a ella' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Categoría porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Category WHERE CategoryId = @CategoryId
		--
		SELECT 0 AS IsSuccess, 'Categoría eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Category
IF OBJECT_ID('Library.uspGetCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetCategory;  
GO
CREATE PROC [Library].uspGetCategory (
	@CategoryId INT = NULL
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT CategoryId, [Name] FROM [Library].Category
	END
	ELSE
	BEGIN
		SELECT CategoryId, [Name] FROM [Library].Category WHERE CategoryId = @CategoryId
	END
END
GO