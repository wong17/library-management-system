-- INSERT Roles
USE LibraryManagementDB
GO

EXEC [Security].uspInsertRole 'ADMIN', 'Administrador'
GO
EXEC [Security].uspInsertRole 'LIBRARIAN', 'Bibliotecario'
GO
EXEC [Security].uspInsertRole 'STUDENT', 'Estudiante'
GO