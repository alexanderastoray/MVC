﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoixResto.Models
{
    [Table("Resultat")]
    public class Resultats
    {
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public int NombreDeVotes { get; set; }
    }
}