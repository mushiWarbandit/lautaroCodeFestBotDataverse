using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static EchoBot2.Models.Dataverse;

namespace EchoBot2.Models
{
    public static class Events
    {
        public static async Task<List<RootFlow>> GetEvent() {
            Dataverse asd = new Dataverse();
            string resp = await asd.GetEventList();
            List<RootFlow>  s = JsonConvert.DeserializeObject<List<RootFlow>>(resp);
            
            return s;
        }

        public static async Task<bool> Suscribe(string EVECODINT,string mail,string username)
        {
            Dataverse asd = new Dataverse();
            //get data from Graph
            string I_MAIL = mail;
            string I_USERNAME = username;
            try
            {
                string resp = await asd.SuscribeEventList(EVECODINT, I_MAIL, I_USERNAME);
                return Convert.ToBoolean(resp);
            }
            catch { return false; }

           
        }



    }
}
