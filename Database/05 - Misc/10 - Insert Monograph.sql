USE LibraryManagementDB
GO

--INSERT Monograph
EXEC [Library].uspInsertMonograph 'M001', 'Sistema de ventas e inventario para la panaderia Delipan', '', 'Ing. Joseph Stewart', '2000-02-02', NULL, 5, 1
GO
EXEC [Library].uspInsertMonograph 'M002', 'Sistema de ventas e inventario para la ferreteria LaConfiable', '', 'Ing. Joseph Stewart', '1990-02-02', NULL, 5, 1
GO