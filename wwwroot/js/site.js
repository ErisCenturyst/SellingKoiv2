// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function updateCartCount() {
    fetch('/Cart/GetCartCount')
        .then(response => response.json())
        .then(count => {
            document.querySelector('.cart-badge').textContent = count;
        })
        .catch(error => console.error('Error:', error));
}

// Call this function when the page loads
document.addEventListener('DOMContentLoaded', updateCartCount);
