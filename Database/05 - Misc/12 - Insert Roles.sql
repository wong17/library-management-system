-- INSERT Roles
USE LibraryManagementDB
GO

EXEC [Security].uspInsertRole 'ADMIN', 'Administrador'
EXEC [Security].uspInsertRole 'LIBRARIAN', 'Bibliotecario'
EXEC [Security].uspInsertRole 'STUDENT', 'Estudiante'