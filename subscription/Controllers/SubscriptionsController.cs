using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using subscription.Models;
using Newtonsoft.Json;
using subscription.Helpers;
using System.Net.Http;

namespace subscription.Controllers

{
    [Route("api/[controller]")]
    public class SubscriptionsController : Controller
    {
        // GET api/Subscriptions
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // GET api/Subscriptions/subscribe

        [HttpPost]
        [Route("subscribe")]
        public async Task<int> Subscribe([FromBody]Subscription subscription)
        {

            Subscribee sub = null;

            var httpClient = Helpers.CouchDBConnect.GetClient("subscriptions");
            HttpContent httpContent;
            HttpResponseMessage response;
            UserSubscriptions u = await DoesSubscriptionExist(subscription.id);
            if (u != null)
            {

                u.AddSubscribee(sub);
                httpContent = new StringContent(
                    JsonConvert.SerializeObject(u),
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );

                response = await httpClient.PutAsync("subscriptions/" + u._id, httpContent);
                return 1;
            }

            UserSubscriptions us = null;
            us._id = subscription.id;
            us.AddSubscribee(sub);
            httpContent = new StringContent(
                    JsonConvert.SerializeObject(us),
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );
            response = await httpClient.PutAsync("subscriptions/" + us._id, httpContent);
            return 2;
        }


        [HttpPost]
        public async Task<int> RemoveSubscribee([FromBody]Subscription subscription)
        {
            Subscribee sub = null;
            var httpClient = Helpers.CouchDBConnect.GetClient("subscriptions");
            HttpContent httpContent;
            HttpResponseMessage response;
            UserSubscriptions u = await DoesSubscriptionExist(subscription.id);
            if (u != null)
            {

                u.RemoveSubscribee(sub);
                httpContent = new StringContent(
                    JsonConvert.SerializeObject(u),
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );

                response = await httpClient.PutAsync("subscriptions/" + u._id, httpContent);
                return 1;
            }

            return -1;
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/subscriptions/delete
        //deletes userSubscriptions record.
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            var httpClient = Helpers.CouchDBConnect.GetClient("UserSubscripton");
            UserSubscriptions u = await DoesSubscriptionExist(id);
            if (u != null)
                await httpClient.DeleteAsync("UserSubscripton/" + u._id + "?rev=" + u._rev);
        }
    


        //Validate if userSubscriptions exist
        async Task<UserSubscriptions> DoesSubscriptionExist(string id)
        {
            var httpClient = CouchDBConnect.GetClient("subscriptions");
            var response = await httpClient.GetAsync("subscriptions/" + id);
            return response.IsSuccessStatusCode ? (UserSubscriptions)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(UserSubscriptions)) : null;
        }

    }

}