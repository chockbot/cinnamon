export function generateStarRating(rating) {
    var fullStars = Math.floor(rating);
    var halfStar = Math.round(rating - fullStars);

    let isMobile = window.matchMedia("only screen and (max-width: 767px)").matches;

    var starRating = '';
    for (var i = 0; i < fullStars; i++) {
        if (!isMobile) {
            starRating += '<i class="fa fa-star" style="width:4rem;"></i>';
        }
        else
        {
            starRating += '<i class="fa fa-star" style="width:2rem;"></i>';
        }
        
    }
    if (halfStar === 1) {
        if (!isMobile) {
            starRating += '<i class="fa fa-star-half" style="width:4rem;"></i>';
        }
        else
        {
            starRating += '<i class="fa fa-star-half" style="width:2rem;"></i>';
        }
        
    }
    $('#star-rating').html(starRating);
}
