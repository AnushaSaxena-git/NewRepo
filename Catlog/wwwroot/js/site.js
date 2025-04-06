// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript c

    // Initialize select2 for newly added multi-select elements
    $('select[multiple]').select2();
//});

// Click event for deleting row
$(document).on('click', '.deleteRow', function () {
    $(this).closest('tr').remove();
});

// Function to update cart count
//function updateCartCount(count) {
//    var cartCountElement = document.getElementById('cartCount');
//    if (cartCountElement) {
//        console.log('Cart count element found');
//        cartCountElement.textContent = count;
//    } else {
//        console.log('Cart count element not found');
//    }

//    var cartBadgeElement = document.querySelector('.navbar-badge');
//    if (cartBadgeElement) {
//        console.log('Navbar badge element found');
//        cartBadgeElement.textContent = count;
//    } else {
//        console.log('Navbar badge element not found');
//    }
//}

//$("skuPrice").html(`₹${sku.Price.toFixed(2)}`);
