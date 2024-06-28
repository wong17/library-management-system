USE LibraryManagementDB
GO

--INSERT MonographAuthor
EXEC [Library].uspInsertMonographAuthor 1, 8
GO
EXEC [Library].uspInsertMonographAuthor 2, 9
GO