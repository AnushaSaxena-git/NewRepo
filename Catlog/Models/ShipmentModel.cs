
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Catlog.Models.EnumExtensions;
using System.Reflection;

using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models.EnumExtensions { }

public class ShipmentModel
{

    [Key]
    public int? Id { get; set; }

    public string?   ShipmentOption { get; set; }

    public string? fedexShipments { get; set; }
    public string? UpsShipment { get; set; }

    public string From { get; set; }

    public string Destination { get; set; }
    [ForeignKey("Product")]
    public int? product_Id { get; set; }
    public  ICollection <Product> Products { get; set; }    


    // Enum lists for dropdowns

    [NotMapped]
    public List<SelectListItem> shipmentOptionsList
    { get; set; }

    [NotMapped]
    public List<SelectListItem> FedexShipmentOptionsList { get; set; }
    [NotMapped]
    public List<SelectListItem> UpsShipmentOptionsList { get; set; }
    [NotMapped]

  

    // Selected values Bind to selected option
    public FedexOptions? SelectedfedexShipmentOptions { get; set; }
    public UpsOptions? SelectedupsShipmentOptions { get; set; }

    public ShipmentOptions? SelectedShipmentOption { get; set; }


    // Constructor to populate the enum lists for dropdowns
    public ShipmentModel()
    {
        // Initialize the lists with enum values for ShipmentOptions dropdown
        shipmentOptionsList = Enum.GetValues(typeof(ShipmentOptions))
            .Cast<ShipmentOptions>()
            .Select(e => new SelectListItem
            {
                Text = e.GetDisplayName(),  
                Value = e.ToString()
            }).ToList();

        FedexShipmentOptionsList = Enum.GetValues(typeof(FedexOptions))
            .Cast<FedexOptions>()
            .Select(e => new SelectListItem
            {
                Text = e.GetDisplayName(),
                Value = e.ToString()
            }).ToList();

            

        // Initialize other enum lists as needed
        UpsShipmentOptionsList = Enum.GetValues(typeof(UpsOptions))
            .Cast<UpsOptions>()
            .Select(e => new SelectListItem
            {
                Text = e.GetDisplayName(),
                Value = e.ToString()
            }).ToList();


    }

    // Enum for Shipment Options
    public enum ShipmentOptions
    {
        [Display(Name = "Fedex Options")]
        Fedex_Options,

        [Display(Name = "UPS Options")]
        Ups_Options,
      
    }




    public enum FedexOptions
    {
        [Display(Name = "Fedex Ground")]


        Fedexground,
        [Display(Name = "Fedex Express Saver")]

        FedexExpressSaver,
        [Display(Name = " Fedex 2 Day")]

        Fedex2day,
        [Display(Name="Fedex Standard Overnight")]

        FedexStandardOvernight,
        [Display(Name = "Fedex Priority  Overnight")]

        FedexPriorityOvernight,
        [Display(Name = "Fedex First Overnight")]

        FedexFirstOvernight


    }
    public enum UpsOptions
    {

        [Display(Name = " Freight Shipping")]

        FreightShipping,
        [Display(Name = "International Shipping")]

        InternationalShipping,
        [Display(Name = " OverNight Shipping")]

        OverNightShipping,
        [Display(Name = " Domestic Shipping")]

        DomesticShipping,
        [Display(Name = "UPS Standard")]

        UpsStandard,
        [Display(Name = " UPS Ground")]

        UpsGround,
        [Display(Name = " UPS® 2nd Day")]

        Ups2day,
        [Display(Name = "UPS Express")]

        UpsExpress
    }
   
   
} 

