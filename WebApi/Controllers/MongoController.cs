using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("MongoTest")]
        public ActionResult MongoTest()
        {
            try
            {
                string connectionString = "mongodb://admin:MyPassword@localhost:27017/admin?connect=replicaSet&replicaSet=rs0";
                var client = new MongoClient(connectionString);

                var db = client.GetDatabase("TestDB");
                var collection = "TestCollection1";
                if (db.ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", collection) }).Any())
                {
                    db.DropCollection(collection);
                }
                db.CreateCollection(collection);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

    }
}
