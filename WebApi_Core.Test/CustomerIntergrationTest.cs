using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WebApi_Core.Test
{
    [TestClass]
    public class CustomerIntergrationTest
    {
        private readonly HttpClient _client;

        //Creating a Server in Ctor wich will base in our web api Project
        //THis server will be work with our Web API Project
        public CustomerIntergrationTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }
        [TestMethod]
        public void GetAllCustomerTest()
        {
            //All test should have 3 sections (Arange ,Act,Assert)
            //Arange Function
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Customers");

            //Action 
            var response = _client.SendAsync(request).Result;

            //Assert Assumtion the Request pass or fail
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [TestMethod]
        [DataRow(100)]
        public void GetCustmerDetailTest(int id)
        {
            //Arrange Api Call
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Customers/{ id }");

            //Action 
            var response = _client.SendAsync(request).Result;

            //Assert Assumtion the Request pass or fail
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
