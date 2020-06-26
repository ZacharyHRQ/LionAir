using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Airport
    {
        [Display(Name = "id")]
        public long Id { get; set; }

        [Display(Name = "iata")]
        public string Iata { get; set; }

        [Display(Name = "icao")]
        public string Icao { get; set; }

        [Display(Name = "name")]
        public string Name { get; set; }

        [Display(Name = "location")]
        public string Location { get; set; }

        [Display(Name = "street_number")]
        public string StreetNumber { get; set; }

        [Display(Name = "street")]
        public string Street { get; set; }

        [Display(Name = "city")]
        public string City { get; set; }

        [Display(Name = "county")]
        public string County { get; set; }

        [Display(Name = "state")]
        public string State { get; set; }

        [Display(Name = "country_iso")]
        public string CountryIso { get; set; }

        [Display(Name = "country")]
        public string Country { get; set; }

        [Display(Name = "postal_code")]
        public long PostalCode { get; set; }

        [Display(Name = "phone")]
        public string Phone { get; set; }

        [Display(Name = "latitude")]
        public double Latitude { get; set; }

        [Display(Name = "longitude")]
        public double Longitude { get; set; }

        [Display(Name = "uct")]
        public long Uct { get; set; }

        [Display(Name = "website")]
        public Uri Website { get; set; }
    }
}
