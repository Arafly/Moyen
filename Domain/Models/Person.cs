using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Moyen.Domain.Models
{
    public class Person
    {
        // [JsonIgnore]
        public int PersonId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }

        [JsonIgnore]
        public List<ArticleFave> ArticleFaves { get; set; }

        //[JsonIgnore]
        //public byte[] Hash { get; set; }

        //[JsonIgnore]
        //public byte[] Salt { get; set; }
    }
}