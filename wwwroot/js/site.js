// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<<<<<<< HEAD

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
=======
>>>>>>> 09ea402fa627b6e8917a1926ea5fd64127e28f93
