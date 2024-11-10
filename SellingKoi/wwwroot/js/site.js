function updateCartCounter() {
    fetch('/Cart/GetCartCount')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(count => {
            const counter = document.getElementById('cartCounter');
            if (counter) {
                counter.textContent = count;
                counter.style.display = count > 0 ? 'inline' : 'none';
            }
        })
        .catch(error => console.error('Error updating cart counter:', error));
}

// Update cart counter when page loads
document.addEventListener('DOMContentLoaded', updateCartCounter);

// Update cart counter every 30 seconds
setInterval(updateCartCounter, 30000);

// Function to call after adding item to cart
function refreshCartCounter() {
    updateCartCounter();
}

// Add event listener for add to cart buttons
document.addEventListener('DOMContentLoaded', function() {
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Wait a short moment for the server to process the cart update
            setTimeout(updateCartCounter, 500);
        });
    });
});
