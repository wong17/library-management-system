-- CREATE FUNCTIONS FOR Book TABLE
USE LibraryManagementDB
GO

-- PARA COMPROBAR SI HAY SOLICITUDES PENDIENTES POR APROBAR CON EL LIBRO QUE SE DESEA DESACTIVAR
-- SI HAY SOLICITUDES CON ESTADO 'CREADA' ENTONCES PRIMERO SE DEBEN DE ELIMINAR O BIEN PRESTAR EL 
-- LIBRO Y DESPUES DESACTIVARLO
IF OBJECT_ID (N'Library.ufnCanDeactivateBook', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCanDeactivateBook;
GO
CREATE FUNCTION [Library].ufnCanDeactivateBook (
	@BookId INT,
	@IsActive BIT
) RETURNS BIT
AS
BEGIN
	-- Verificar si el libro no esta activo y se quiere volver a desactivar, sino lo esta entonces devolver 0, no se puede volver a desactivar
	IF EXISTS ((SELECT 1 FROM [Library].Book WHERE BookId = @BookId AND IsActive = 0 AND @IsActive = 0))
	BEGIN
		RETURN 0
	END
	-- Verificar si hay una solicitud asociada al libro que tenga por estado CREADA
	IF EXISTS (SELECT 1 FROM [Library].BookLoan WHERE BookId = @BookId AND [State] = 'CREADA')
	BEGIN
		RETURN 0
	END

	RETURN 1
END
GO