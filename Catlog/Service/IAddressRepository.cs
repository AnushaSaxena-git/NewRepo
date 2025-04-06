using Catlog.Models;

namespace Catlog.Service
{
    public interface IAddressRepository
    {
        Task<AddressViewModel> GetAddressAsync(string userId);
        List<AddressViewModel> GetAddress();
        public void AddToCart(AddressViewModel addressViewModel);


        Task SaveAddressAsync(string userId, AddressViewModel address);

    }
}
