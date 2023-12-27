// Register Modal
$("#registrationForm").on("submit", function (e) {
  e.preventDefault();

  var data = $(this).serialize();

  $.ajax({
    url: "/Client/Register",
    type: "POST",
    data: data,
    success: function (response) {
      if (response.success) {
        $("#registrationModal").modal("hide");
      } else {
      }
    },
    error: function () {},
  });
});



//Add to Cart Icon
function slideIcon() {
    var icon = document.querySelector('.add-to-cart .cart-icon');
    icon.style.transform = 'translateX(10px)';
    setTimeout(function () {
        icon.style.transform = 'translateX(0px)';
    },
   300); 
}






