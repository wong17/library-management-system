# Library management system

## Tecnologías utilizadas
- Aplicación web (Angular 18 y Typescript)
- Web API (C# .NET 8)
- SignalR para notificaciones a clientes en tiempo real al momento de realizar una solicitud de préstamo de libro o monografía.
- Base de datos (SQL Server Management Studio)

## Vistas y acciones según los roles

- admin

| Vistas                   | Acciones         |
| ------------------------ | ---------------- |
| Dashboard                | Dashboard | 
| Libros                   | Crear, editar, eliminar y actualizar la información de los libros, realizar búsquedas por título, editorial, autores, categorías o sub categorías a las que pertenece y por fecha de publicación.  |
| Monografías              | Crear, editar, eliminar y actualizar la información de las monografías, realizar búsquedas por título, carrera a la que pertenece, año de presentación y autor. |
| Préstamo de libros       | Aprobar préstamos de libros realizados por los estudiantes asignando una fecha y hora límite para la devolución según el tipo de préstamo si es para sala o domicilio y así también realizar devoluciones de libros anteriormente prestados. |            
| Préstamo de Monografías  | Aprobar préstamos de monografías realizados por los estudiantes asignando una hora limité dentro de la sala de lectura y así también realizar devoluciones de monografías anteriormente prestadas. |    
| Editoriales              | Crear, editar, eliminar y actualizar información de editoriales de libros, importantes al momento de registrar un nuevo libro y sobre todo al realizar búsquedas o aplicar filtros. |
| Autores                  | Crear, editar, eliminar y actualizar información de diversos autores de libros, importantes al momento de registrar un nuevo libro y sobre todo al realizar búsquedas o aplicar filtros. |
| Categorias               | Crear, editar, eliminar y actualizar información sobre categorías que permitan clasificar o agrupar de una mejor manera los libros, importantes al registrar un nuevo libro y sobre todo al momento de realizar búsquedas o aplicar filtros. |
| Sub Categorias           | Crear, editar, eliminar y actualizar información sobre sub categorías a las que pertenecen los libros, permitiendo realiza búsquedas aún más especificas. |

- bibliotecario

| Vistas                  | Acciones |
| ----------------------- | ----------- |
| Dashboard               | Dashboard |
| Libros                  | Realizar búsquedas por título, editorial, autores, categorías o sub categorías a las que pertenece y por fecha de publicación. |
| Monografías             | Realizar búsquedas por título, carrera a la que pertenece, año de presentación y autor. |
| Préstamo de libros      | Aprobar préstamos de libros realizados por los estudiantes asignando una fecha y hora límite para la devolución según el tipo de préstamo si es para sala o domicilio y así también realizar devoluciones de libros anteriormente prestados. |          
| Préstamo de Monografías | Aprobar préstamos de monografías realizados por los estudiantes asignando una hora limité dentro de la sala de lectura y así también realizar devoluciones de monografías anteriormente prestadas. |    

- estudiante

| Vistas      | Acciones |
| ----------- | ------------ |
| Libros      | Solicitar un libro para utilizar en sala o a domicilio, así también realizar búsquedas por título, editorial, autores, categorías o sub categorías a las que pertenece y por fecha de publicación. |
| Monografías | Solicitar una monografía para uso en sala de lectura, así también realizar búsquedas por título, carrera a la que pertenece, año de presentación y autor.|

## Ejecutar aplicación web

Por defecto la API tiene configurado 3 origenes:

```javascript
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",    // admin 
            "http://localhost:4201",    // bibliotecario
            "http://localhost:4202")    // estudiante
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

```
Ejecutando cualquiera de los siguientes comandos podemos lanzar más rápido la app, cabe aclarar que una vez arranque la app no es necesario iniciar sesión con el rol que indica el comando, se dio el nombre simplemente por distinguir y hacer las pruebas con cada uno de los roles.

```bash
  npm run start:admin 
```

```bash
  npm run start:librarian 
```

```bash
  npm run start:student 
```
# Inicio de sesión

El .bak incluye 3 usuarios con las siguientes credenciales:

| Usuario       | Contraseña |
| ------------- | -------- |
| admin         | Abcd1234 |
| bibliotecario | Abcd1234 |
| estudiante    | Abcd1234 |

# Capturas
![image](https://github.com/wong17/library-management-system/assets/64237085/7aebf9f9-63de-4a02-ae51-5af5af76e6e9)
![image](https://github.com/wong17/library-management-system/assets/64237085/f7a50a14-27e7-4790-8ea3-1e21e1922901)
![image](https://github.com/user-attachments/assets/17c03825-034b-48c1-a631-4c85181f7013)
![image](https://github.com/wong17/library-management-system/assets/64237085/73679c5f-c6cc-4cbb-bb60-25e277ed9b58)
![image](https://github.com/user-attachments/assets/6829d526-755f-466e-bd45-fec10f104b3c)
![image](https://github.com/user-attachments/assets/81cc0dd4-c28b-4ae0-8507-9bc98c585daf)


