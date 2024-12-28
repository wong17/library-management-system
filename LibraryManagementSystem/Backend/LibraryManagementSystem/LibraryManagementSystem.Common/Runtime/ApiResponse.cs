using System.Net;

namespace LibraryManagementSystem.Common.Runtime;

public class ApiResponse
{
    /*
     * Resultado de la petición HTTP
     * 0: Éxito, 1: No paso una validación, 2: No existe el recurso, 3: Error en la base de datos
     */
    public ApiResponseCode IsSuccess { get; set; } = ApiResponseCode.ValidationError;

    /* Código de estado HTTP */
    public HttpStatusCode StatusCode { get; set; }

    /* Mensaje de éxito o error asociado a la petición */
    public string? Message { get; set; }

    /* Resultado al haber realizado la petición */
    public object? Result { get; set; }
}