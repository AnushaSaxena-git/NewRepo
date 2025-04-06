using Catlog.Models;

namespace Catlog.Service
{
    public class ShippingModelRepository : Ishippingmodelrepository
    {
        private readonly CatlogDBcontext _context;

        public ShippingModelRepository(CatlogDBcontext context)
        {
            _context = context;
        }

        public void AddShippingType(ShipmentModel shipmentModel)
        {
            _context.ShipmentModels.Add(shipmentModel);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
