using Catlog.DTO;

namespace Catlog.Models
{
    public class ProductAndShipmentViewModel
    {


        public ProductWithSkuDto Product { get; set; }
        public ShipmentModel Shipment { get; set; }
    }
}
