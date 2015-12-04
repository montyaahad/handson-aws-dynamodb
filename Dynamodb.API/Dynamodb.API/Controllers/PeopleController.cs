using Dynamodb.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Dynamodb.API.Controllers
{
    public class PeopleController : Controller
    {
        public string CreatTable()
        {
            // get all people from db
            return PeopleDA.CreatPeopleTable();
        }

        public string InsertData()
        {
            // get all people from db
            return PeopleDA.InsertPeopleData();
        }

        public string GetAllPeople()
        {
            // get all people from db
            return PeopleDA.GetAllPeople();
        }
    }
}
