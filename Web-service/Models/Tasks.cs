﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_service.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdUser { get; set; }
        public bool condition { get; set; }

        /*public Tasks(int id, string name, string description, int idUser)
        {
            Id = id;
            Name = name;
            Description = description;
            IdUser = idUser;
        }*/
    }
}