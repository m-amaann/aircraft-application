// Register Modal
$('#registrationForm').on('submit', function (e) {
    e.preventDefault();

    var data = $(this).serialize(); 

    $.ajax({
        url: '/Client/Register', 
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success) {
                $('#registrationModal').modal('hide');
            } else {
                
            }
        },
        error: function () {        
        }
    });
});


//PRODUCT CARD CAROASAL

let track = document.querySelector('.carousel-track');
let cards = Array.from(track.children);
let cardWidth = cards[0].getBoundingClientRect().width + 20; // Including margin
let currentSlide = 0;

function moveCarousel(direction) {
    const trackMoveAmount = cardWidth;

    if (direction === 'next') {
        currentSlide++;
        if (currentSlide >= cards.length) currentSlide = 0; // Loop back to the start
    } else if (direction === 'prev') {
        currentSlide--;
        if (currentSlide < 0) currentSlide = cards.length - 1; // Loop back to the end
    }

    track.style.transform = 'translateX(' + (-trackMoveAmount * currentSlide) + 'px)';
}



//Add to Cart Icon for product card
function slideIcon() {
    var icon = document.querySelector('.add-to-cart .cart-icon');
    icon.style.transform = 'translateX(10px)';
    setTimeout(function () {
        icon.style.transform = 'translateX(0px)';
    },
   300); 
}



// ADD TO CART FUNCTION 








