using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace LibraryManagementSystem.Common.Helpers
{
    public static class ConverterHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[]? IFormFileToByteArray(IFormFile? file)
        {
            if (file is null)
                return null;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IFormFile? ByteArrayToIFormFile(byte[]? byteArray, string fileName = "image.jpg")
        {
            if (byteArray is null)
                return null;

            var stream = new MemoryStream(byteArray);
            return new FormFile(stream, 0, byteArray.Length, "name", fileName);
        }
    }
}