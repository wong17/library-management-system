USE LibraryManagementDB
GO

--INSERT MonographAuthor
EXEC [Library].uspInsertMonographAuthor 1, 6
GO
EXEC [Library].uspInsertMonographAuthor 2, 7
GO