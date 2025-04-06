// This should be the interface definition
namespace Catlog.Service
{
    public interface Ishippingmodelrepository
    {
        void AddShippingType(ShipmentModel shipmentModel);
        void Save();
    }
}
