using System.ComponentModel.DataAnnotations;

namespace Catlog.Models
{
    public class ShippingType
    {
        [Key]
        public int shipping_Id {  get; set; }   
        public string Type_name { get; set; }
        public string ShippingOptions {  get; set; }    
           

        public string To { get; set; }

        public string Destination {  get; set; }    
    }
}
