﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kurier.Interfaces.Model;
using Kurier.Models.Context;
using Kurier.Models.DTO;
using Kurier.Models.DTO.Samochod;
using Kurier.Models.DTO.Uzytkownik;
using System.Text.RegularExpressions;

namespace Kurier.Models.DataAccess
{
    public class SamochodyModel : IMSamochody
    {
        private ApplicationContext _context;

        public SamochodyModel()
        {
            _context = new ApplicationContext();
        }

        public SamochodyModel(ApplicationContext context)
        {
            _context = context;
        }
        public void DodajSamochod(DaneSamochodu samochod)
        {
            _context.Samochody.Add(samochod);
            _context.SaveChanges();
        }

        public List<DaneSamochodu> PobierzListeSamochodow()
        {
            return _context.Samochody.ToList();
        }

        public DaneSamochodu PobierzSamochod(int id)
        {
            return _context.Samochody.Find(id);
        }

        public void PowiazKurieraISamochod(int idSamochodu, int idKuriera)
        {
            DaneKuriera kurier = new KurierzyModel(_context).PobierzKuriera(idKuriera);
            DaneSamochodu samochod = _context.Samochody.Find(idSamochodu);
            if (samochod != null)
                kurier.Samochod = samochod;

            _context.SaveChanges();
        }

        public void UsunSamochod(int idSamochodu)
        {
            var samochod = _context.Samochody.Find(idSamochodu);
            _context.Samochody.Remove(samochod);
            _context.SaveChanges();
        }

        public bool WalidujDaneSamochodu(DaneSamochodu samochod)
        {
            return WalidujRejestracje(samochod.NumRejestracyjny)
                && WalidujStatus(samochod.Stan);
        }

        private bool WalidujRejestracje(String NumRejestracyjny)
        {
            return Regex.IsMatch(NumRejestracyjny, @"^[a-zA-Z][a-zA-Z0-9]*$")
               && NumRejestracyjny.Length > 0
               && NumRejestracyjny.Length < 12;
        }

        private bool WalidujStatus(String Stan)
        {
            return Regex.IsMatch(Stan, @"^[a-zA-Z]*$")
               && Stan.Length > 0;
        }
    }
}