﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KinoCentar.Shared.Models
{
    public class AnketaModel
    {
        public int Id { get; set; }

        public int KorisnikId { get; set; }

        public string KorisnikImePrezime
        {
            get
            {
                return Korisnik?.ImePrezime;
            }
        }

        public string Naslov { get; set; }

        public DateTime Datum { get; set; }

        public DateTime? ZakljucenoDatum { get; set; }

        public KorisnikModel Korisnik { get; set; }

        public List<AnketaOdgovorModel> Odgovori { get; set; }
    }
}
