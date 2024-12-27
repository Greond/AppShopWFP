using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Policy;
using System.Windows;
using WpfAppTEST.Models;

namespace WpfAppTEST.Data
{
    class HttpRequest
    {
        // https://192.168.1.101:7274/api
        public const string BaseUrl = "https://192.168.1.101:7274/api";
        //Items 
        public static async Task<Item> GetItemById(ulong ID)
        {
            bool ServerConnect = await WiFiConnection(); // Проверка на Wi-Fi подключение
            if (!ServerConnect)
            {
                throw new Exception();
            }
            string url = BaseUrl + "/Items" + $"/{ID}";
            Item item = new Item();
            try
            {
                Console.WriteLine("Try connect to WebApi");
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                Console.WriteLine(response.Content);
                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка
                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(content);
                item = JsonConvert.DeserializeObject<Item>(obj.ToString());

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Item>> GetItemsFromWebApi(string? propertyName, string? propertyValue, int? ItemsCount)
        {
            bool ServerConnect = await WiFiConnection(); // Проверка на Wi-Fi подключение
            if (!ServerConnect)
            {
                throw new Exception();
            }
            string url = BaseUrl + "/Items";
            List<Item> items = new List<Item>();
            if (propertyName != null || propertyValue != "")
            {
                url += $"/ByProperty?{propertyName}={propertyValue}";
                // like a https://192.168.1.103:7274/api/Item/ByProperty?Stock=true
                if (ItemsCount > 0)
                {
                    url += $"&Count={ItemsCount}";
                }
            }
            try
            {
                Console.WriteLine("Try connect to WebApi");
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                Console.WriteLine(response.Content);
                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка
                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JArray arr = JArray.Parse(content);
                foreach (var item in arr)
                {
                    Item rcvdData = JsonConvert.DeserializeObject<Item>(item.ToString());
                    items.Add(rcvdData);
                }
                //seccuses
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Item>> GetItemsFromWebApi()
        {
            bool ServerConnect = await WiFiConnection(); // Проверка на Wi-Fi подключение
            if (!ServerConnect)
            {
                throw new Exception();
            }
            string url = BaseUrl + "/Items";
            List<Item> items = new List<Item>();
            try
            {
                Console.WriteLine("Try connect to WebApi");
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                Console.WriteLine(response.Content);
                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка
                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JArray arr = JArray.Parse(content);
                foreach (var item in arr)
                {
                    Item rcvdData = JsonConvert.DeserializeObject<Item>(item.ToString());
                    items.Add(rcvdData);
                }
                //seccuses
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<ItemCategory>> GetItemsCategories()
        {
            bool ServerConnect = await WiFiConnection(); // Проверка на Wi-Fi подключение
            if (!ServerConnect)
            {
                throw new Exception();
            }
            string url = BaseUrl + "/Items/Categories";
            List<ItemCategory> Categories = new List<ItemCategory>();
            try
            {
                Console.WriteLine("Try connect to WebApi");
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                Console.WriteLine(response.Content);
                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка
                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JArray arr = JArray.Parse(content);
                foreach (var category in arr)
                {
                    ItemCategory rcvdData = JsonConvert.DeserializeObject<ItemCategory>(category.ToString());
                    Categories.Add(rcvdData);
                }
                return Categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<bool> WiFiConnection()
        {
            string testUrl = BaseUrl + "/Items/Test";
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(testUrl);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    MessageBox.Show("Server Eror");
                    return false;
            }
            catch
            {
                MessageBox.Show("Something wet wrong. Check BaseUrl.");
                return false;
            }
        }
    }
}
