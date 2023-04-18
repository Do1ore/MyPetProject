


//$(document).ready(function submitChanges(productId) {
//    $('#reviewForm').submit(function (event) {
//        // Отменяем действие по умолчанию при отправке формы
//        event.preventDefault();
//        var $reviewModel = {};

//        var reviewText = document.getElementById("textReviewInput").innerHTML;
//        var rating = document.getElementById("ratingReviewInput").innerHTML;

//        $reviewModel = {
//            prodId = productId,
//            prodRating = rating,
//            text = reviewText
//        }
       
//        // Отправляем объект на сервер
//        $.ajax({
//            type: 'POST',
//            url: '/UserProduct/SendReviewAsync/',
//            contentType: "application/json",
//            data: JSON.stringify({ review: $reviewModel }),
//            success: function (result) {
//                NotifySuccess("Информация сохранена.")

//            },
//            error: function (xhr, textStatus, errorThrown) {
//                console.log('Ошибка: ' + xhr.responseText);
//            }
//        });
//    });
//});
