using Catlog.Models;
using Microsoft.AspNetCore.Http;


namespace Catlog.Service
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddressRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(AddressViewModel addressViewModel)
        {
            var address = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<AddressViewModel>>("Address") ?? new List<AddressViewModel>();
            var index = address.Find(x => x.FirstName == addressViewModel.FirstName && x.FirstName == addressViewModel.FirstName);


            if (address  is null)
            {
                address.Add(addressViewModel);
            }

            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Address", address);
        }

       

        public async Task<AddressViewModel> GetAddressAsync(string userId)
        {
            var address = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<AddressViewModel>($"Address_{userId}");
            return await Task.FromResult(address); 
        }

        public async Task SaveAddressAsync(string userId, AddressViewModel address)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson($"Address_{userId}", address);
            await Task.CompletedTask;
        }
        public List<AddressViewModel> GetAddress()
        {
            return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<AddressViewModel>>("Address") ?? new List<AddressViewModel>();
        }
    }




}
