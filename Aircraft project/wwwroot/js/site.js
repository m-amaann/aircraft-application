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


