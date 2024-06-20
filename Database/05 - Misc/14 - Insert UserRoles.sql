-- INSERT UserRole
USE LibraryManagementDB
GO

EXEC [Security].uspInsertUserRole 1, 1
EXEC [Security].uspInsertUserRole 2, 3
EXEC [Security].uspInsertUserRole 3, 2