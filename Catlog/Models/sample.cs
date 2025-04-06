//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace Catlog.Models
//{
//    public class sample
//    {
//        sample()
//        {

//            ShipmenTOptionsList = System.Enum.GetValues(typeof(ShipmentOptions)).Cast<ShipmentOptions>().Select(e => e.new SelectListItem { Text = e.GetDisplayName(), Value = e.Tostring() }).ToList();

//        }
//        public int id { get; set; }
//        public string ShipmentOptions1 { get; set; }
//        public string UpsShipmentOptions2{ get; set; }
//        public string FedexOptions { get; set; }

//        public List<SelectListItem> ShipmenTOptionsList{get; set; }
//        public List<SelectListItem> UpsShipmentOptionsList { get; set; }
//        public List<SelectListItem> FedexOptionsList { get; set; }

//        public  ShipmentOptions? selectedShipmentOption { get; set; }


//        public enum ShipmentOptions
//        {
//            fedex,
//            ups



//        } public enum UpsShipmentOptions
//        { 
//            UpsOp1,
//            UpsOp2,


//        }
//        public enum FedexShipmentOptions
//        {

//            fedex1,
//            fedex2
//        }

//    }
//}
