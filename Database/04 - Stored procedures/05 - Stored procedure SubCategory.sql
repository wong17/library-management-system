-- CREATE STORED PROCEDURES FOR SubCategory TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- INSERT SubCategory
IF OBJECT_ID('Library.uspInsertSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertSubCategory;  
GO
CREATE PROC [Library].uspInsertSubCategory (
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
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una SubCategoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].SubCategory (CategoryId, [Name]) VALUES (@CategoryId, @Name)
		--
		SELECT 0 AS IsSuccess, 'SubCategoría registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY SubCategory
IF OBJECT_ID('Library.uspInsertManySubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManySubCategory;  
GO
CREATE PROC [Library].uspInsertManySubCategory (
	@SubCategories [Library].SubCategoryType READONLY
)
AS
BEGIN
	-- Verificar que el Id de la Categoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.CategoryId IS NULL OR sc.CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las Categorias 
	IF EXISTS (
        SELECT sc.CategoryId
        FROM @SubCategories AS sc
        LEFT JOIN [Library].Category AS db ON sc.CategoryId = db.CategoryId
        WHERE db.CategoryId IS NULL
    )
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Categorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el nombre de todas las SubCategorias no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.[Name] IS NULL OR sc.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las SubCategorias solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.[Name] LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoria solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de SubCategorias repetidos
	IF EXISTS (SELECT [Name] FROM @SubCategories GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Sub categorias están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan SubCategorias con el mismo nombre en la base de datos
	IF EXISTS (SELECT sc.[Name] FROM @SubCategories AS sc INNER JOIN [Library].SubCategory AS db ON sc.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Sub categorias ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción y capturar los ID insertados en la tabla temporal
		DECLARE @InsertedIDs TABLE (ID INT);

		INSERT INTO [Library].SubCategory(CategoryId, [Name]) 
		OUTPUT inserted.SubCategoryId INTO @InsertedIDs(ID)
		SELECT sc.CategoryId, sc.[Name] FROM @SubCategories AS sc
		--
		SELECT 0 AS IsSuccess, 'SubCategorias registradas exitosamente' AS [Message];
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

-- UPDATE SubCategory
IF OBJECT_ID('Library.uspUpdateSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateSubCategory;  
GO
CREATE PROC [Library].uspUpdateSubCategory (
	@SubCategoryId INT,
	@CategoryId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la SubCategoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una SubCategoría con el Id proporcionado' AS [Message]
		RETURN
	END
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
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una SubCategoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].SubCategory SET CategoryId = @CategoryId, [Name] = @Name WHERE SubCategoryId = @SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'SubCategoría actualizada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE MANY SubCategory
IF OBJECT_ID('Library.uspUpdateManySubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateManySubCategory;  
GO
CREATE PROC [Library].uspUpdateManySubCategory (
	@SubCategories [Library].SubCategoryType READONLY
)
AS
BEGIN
	-- Verificar que el Id de la SubCategoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.SubCategoryId IS NULL OR sc.SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la SubCategoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las SubCategorias a actualizar
	IF EXISTS (
        SELECT sc.SubCategoryId
        FROM @SubCategories AS sc
        LEFT JOIN [Library].SubCategory AS db ON sc.SubCategoryId = db.SubCategoryId
        WHERE db.SubCategoryId IS NULL
    )
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más SubCategorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id de la Categoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.CategoryId IS NULL OR sc.CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las Categorias 
	IF EXISTS (
        SELECT sc.CategoryId
        FROM @SubCategories AS sc
        LEFT JOIN [Library].Category AS db ON sc.CategoryId = db.CategoryId
        WHERE db.CategoryId IS NULL
    )
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Categorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el nombre de todas las SubCategorias no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.[Name] IS NULL OR sc.[Name] = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que el nombre todas las SubCategorias solo tenga mayúsculas, minúsculas, guiones y espacios
	IF EXISTS (SELECT 1 FROM @SubCategories AS sc WHERE sc.[Name] LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategoria solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	-- Verificar que no vayan nombres de SubCategorias repetidos
	IF EXISTS (SELECT [Name] FROM @SubCategories GROUP BY [Name] HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Sub categorias están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan SubCategorias con el mismo nombre en la base de datos
	IF EXISTS (SELECT sc.[Name] FROM @SubCategories AS sc INNER JOIN [Library].SubCategory AS db ON sc.[Name] = db.[Name])
    BEGIN
        SELECT 1 AS IsSuccess, 'Uno o más Sub categorias ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Actualizar todos los registros
		UPDATE db
		SET db.CategoryId = sc.CategoryId, db.[Name] = sc.[Name]
		FROM [Library].SubCategory AS db
		INNER JOIN @SubCategories AS sc ON db.SubCategoryId = sc.SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'SubCategorias actualizadas exitosamente' AS [Message]
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

-- DELETE SubCategory
IF OBJECT_ID('Library.uspDeleteSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteSubCategory;  
GO
CREATE PROC [Library].uspDeleteSubCategory (
	@SubCategoryId INT
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la SubCategoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una SubCategoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookSubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la SubCategoría porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'SubCategoría eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET SubCategory
IF OBJECT_ID('Library.uspGetSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetSubCategory;  
GO
CREATE PROC [Library].uspGetSubCategory (
	@SubCategoryId INT = NULL
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT SubCategoryId, CategoryId, [Name] FROM [Library].SubCategory
	END
	ELSE
	BEGIN
		SELECT SubCategoryId, CategoryId, [Name] FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId
	END
END
GO