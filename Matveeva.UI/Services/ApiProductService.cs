using Shop.Domain.Entities;
using Shop.Domain.Models;
using System.Net.Http;
using System.Text.Json;


namespace Matveeva.UI.Services
{
    public class ApiProductService(HttpClient httpClient) : IProductService
    {
        public async Task<ResponseData<Tovar>> CreateProductAsync(Tovar product, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<Tovar>();

            // Послать запрос к API для сохранения объекта
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, product);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект:{response.StatusCode}";
                return responseData;
            }

            // Если файл изображения передан клиентом
            if (formFile != null)
            {

                // получить созданный объект из ответа Api-сервиса
                var Tovar = await response.Content.ReadFromJsonAsync<Tovar>();

                // создать объект запроса
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{Tovar.Id}")
                };

                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();

                // создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());

                // добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);

                // поместить контент в запрос
                request.Content = content;

                // послать запрос к Api-сервису
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение:{response.StatusCode} ";
                }
            }
            return responseData;
        }


        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Tovar>> GetProductByIdAsync(int id)
        {
            var apiUrl = $"{httpClient.BaseAddress.AbsoluteUri}{id}";
            var response = await httpClient.GetFromJsonAsync<Tovar>(apiUrl);
            return new ResponseData<Tovar>() { Data = response };
        }

        public async Task<ResponseData<ListModel<Tovar>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;
            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }
            var query = QueryString.Create(queryData);
            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content
                .ReadFromJsonAsync<ResponseData<ListModel<Tovar>>>();
            };
            var response = new ResponseData<ListModel<Tovar>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
            return response;
        }

        public Task UpdateProductAsync(int id, Tovar product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }

}

