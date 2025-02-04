﻿using ComiCal.Batch.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utf8Json;

namespace ComiCal.Batch.Repositories
{
    public class RakutenComicDummyRepository : IRakutenComicRepository
    {
        private readonly HttpClient _httpClient;

        public RakutenComicDummyRepository(
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }

        public async Task<RakutenComicResponse> Fetch(int requestPage)
        {
            var json = "{" +
                "  \"Items\": [" +
                "    {" +
                "      \"Item\": {" +
                "        \"hardware\": \"\"," +
                "        \"limitedFlag\": 0," +
                "        \"author\": \"Ark Performance\"," +
                "        \"title\": \"蒼き鋼のアルペジオ　19\"," +
                "        \"listPrice\": 0," +
                "        \"itemCaption\": \"\"," +
                "        \"publisherName\": \"少年画報社\"," +
                "        \"isbn\": \"9784785966851\"," +
                "        \"largeImageUrl\": \"https://thumbnail.image.rakuten.co.jp/@0_mall/book/cabinet/6850/9784785966850.jpg?_ex=200x200\"," +
                "        \"jan\": \"\"," +
                "        \"mediumImageUrl\": \"https://thumbnail.image.rakuten.co.jp/@0_mall/book/cabinet/6850/9784785966850.jpg?_ex=120x120\"," +
                "        \"availability\": \"1\"," +
                "        \"os\": \"\"," +
                "        \"postageFlag\": 2," +
                "        \"salesDate\": \"2020年06月下旬\"," +
                "        \"smallImageUrl\": \"https://thumbnail.image.rakuten.co.jp/@0_mall/book/cabinet/6850/9784785966850.jpg?_ex=64x64\"," +
                "        \"label\": \"\"," +
                "        \"discountPrice\": 0," +
                "        \"itemPrice\": 715," +
                "        \"booksGenreId\": \"001001003049\"," +
                "        \"affiliateUrl\": \"\"," +
                "        \"reviewCount\": 4," +
                "        \"reviewAverage\": \"4.67\"," +
                "        \"artistName\": \"\"," +
                "        \"discountRate\": 0," +
                "        \"chirayomiUrl\": \"\"," +
                "        \"itemUrl\": \"https://books.rakuten.co.jp/rb/16293906/\"" +
                "      }" +
                "    }" +
                "  ]," +
                "  \"pageCount\": 1," +
                "  \"hits\": 1," +
                "  \"last\": 1," +
                "  \"count\": 1," +
                "  \"page\": 1," +
                "  \"carrier\": 0," +
                "  \"GenreInformation\": []," +
                "  \"first\": 1" +
                "}";
            return await Task.Run(() =>
            {
                return JsonSerializer.Deserialize<RakutenComicResponse>(json);
            });
        }

        async Task<BinaryData> IRakutenComicRepository.FetchImageAndConvertStream(string imageUrl)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, imageUrl);
            var res = await _httpClient.SendAsync(requestMessage);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = await res.Content.ReadAsStringAsync();
                throw new Exception($"RakutenImageFetch Error\n{errorMessage}");
            }
            Stream data = await res.Content.ReadAsStreamAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                data.CopyTo(ms);
                return new BinaryData(ms.ToArray());
            }
        }
    }
}
