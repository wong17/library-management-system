USE LibraryManagementDB
GO

--INSERT Publishers
EXEC [Library].uspInsertPublisher 'Limusa'
GO
EXEC [Library].uspInsertPublisher 'McGraw Hill'
GO
EXEC [Library].uspInsertPublisher 'Mir Moscú'
GO
EXEC [Library].uspInsertPublisher 'Cengage Learning'
GO
EXEC [Library].uspInsertPublisher 'Patria'
GO