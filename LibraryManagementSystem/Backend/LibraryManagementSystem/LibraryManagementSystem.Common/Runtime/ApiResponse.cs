using System.Net;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Common.Runtime
{
    public class ApiResponse
    {
        /*
         * Resultado de la petición HTTP
         * 0: Éxito, 1: Error en la bd o con una validación, 2: No existe el recurso
         */
        [JsonPropertyName("IsSuccess")]
        public int IsSuccess { get; set; } = 1;

        /* Código de estado HTTP */
        [JsonPropertyName("StatusCode")]
        public HttpStatusCode StatusCode { get; set; }

        /* Mensaje de exito o error asociado a la petición */
        [JsonPropertyName("Message")]
        public string? Message { get; set; }

        /* Resultado al haber realizado la petición */
        [JsonPropertyName("Result")]
        public object? Result { get; set; }
    }
}