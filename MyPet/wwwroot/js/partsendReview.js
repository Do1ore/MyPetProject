


//$(document).ready(function submitChanges(productId) {
//    $('#reviewForm').submit(function (event) {
//        // �������� �������� �� ��������� ��� �������� �����
//        event.preventDefault();
//        var $reviewModel = {};

//        var reviewText = document.getElementById("textReviewInput").innerHTML;
//        var rating = document.getElementById("ratingReviewInput").innerHTML;

//        $reviewModel = {
//            prodId = productId,
//            prodRating = rating,
//            text = reviewText
//        }
       
//        // ���������� ������ �� ������
//        $.ajax({
//            type: 'POST',
//            url: '/UserProduct/SendReviewAsync/',
//            contentType: "application/json",
//            data: JSON.stringify({ review: $reviewModel }),
//            success: function (result) {
//                NotifySuccess("���������� ���������.")

//            },
//            error: function (xhr, textStatus, errorThrown) {
//                console.log('������: ' + xhr.responseText);
//            }
//        });
//    });
//});
