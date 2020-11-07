function showLoading() {
    $('.loading-container').removeClass('loading-inactive');
    $('.loading-container').addClass('loading-active');
}

function hideLoading() {
    $('.loading-container').removeClass('loading-active');
    $('.loading-container').addClass('loading-inactive');
}