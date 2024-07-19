const imgs = document.querySelectorAll('.img-select a');
const imgBtns = [...imgs];
let imgId = 1;

imgBtns.forEach((imgItem) => {
    imgItem.addEventListener('click', (event) => {
        event.preventDefault();
        imgId = imgItem.dataset.id;
        slideImage();
    });
});

function slideImage() {
    const displayWidth = document.querySelector('.img-showcase img:first-child').clientWidth;

    document.querySelector('.img-showcase').style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
}

window.addEventListener('resize', slideImage);

function showTrailer(trailerUrl) {
    var modal = document.getElementById("trailerModal");
    var iframe = document.getElementById("trailerFrame");
    var embedUrl = convertToEmbedUrl(trailerUrl);
    iframe.src = embedUrl;
    modal.style.display = "block";
}

function closeTrailer() {
    var modal = document.getElementById("trailerModal");
    var iframe = document.getElementById("trailerFrame");
    iframe.src = "";
    modal.style.display = "none";
}

// Convert YouTube URL to embed URL
function convertToEmbedUrl(url) {
    // Regex to extract video ID from various YouTube URL formats
    var regExp = /(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/;
    var match = url.match(regExp);
    if (match && match[1].length == 11) {
        return 'https://www.youtube.com/embed/' + match[1];
    } else {
        return url;
    }
}

// Close the modal when clicking outside of it
window.onclick = function (event) {
    var modal = document.getElementById("trailerModal");
    if (event.target == modal) {
        closeTrailer();
    }
}

